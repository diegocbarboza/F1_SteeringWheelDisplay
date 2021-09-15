using System;
using System.Globalization;
using System.IO.Ports;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace TelemetryReceiver
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture; // to make sure floats use dots instead of commas as decimal separator

            UdpClient receivingUdpClient = new(20777);
            IPEndPoint RemoteIpEndPoint = new(IPAddress.Any, 0);

            bool arduinoIsReadyToReceive = true;

            var serialPort = new SerialPort
            {
                PortName = "COM4",
                BaudRate = 115200                
            };
            serialPort.DataReceived += (sender, e) => { arduinoIsReadyToReceive = true; };

            ushort speed = 0;
            int gear = 1;
            int rpmLevel = 1;
            float time = 0;
            byte drs = 0;
            ushort engineRpm = 0;
            int drsAllowed = 0;
            byte[] tyreWear = new byte[4] { 0, 0, 0, 0 };
            int ersMode = 1;
            float ersStore = 0;
            float ersLap = 0;
            float deltaSector1 = 0;
            float deltaSector2 = 0;
            float deltaLap = 0;

            try
            {
                serialPort.Open();
            }
            catch (Exception e)
            {
                ExitWithError($"Could not connect to Arduino. Error: {e.Message}");
            }

            while (true)
            {
                if (receivingUdpClient.Available > 0)
                {
                    byte[] receiveBytes = receivingUdpClient.Receive(ref RemoteIpEndPoint);

                    var packetHeader = PacketHeader.FromByteArray(receiveBytes);

                    if (packetHeader.packetFormat != 2020)
                    {
                        ExitWithError($"Packet format {packetHeader.packetFormat} not supported. For now only F1 2020 is supported.");
                    }

                    if (packetHeader.packetId == 2) // Lap data
                    {
                        var packet = PacketLapData.FromByteArray(receiveBytes);

                        time = packet.lapData[packetHeader.playerCarIndex].currentLapTime;
                        var sector1 = packet.lapData[packetHeader.playerCarIndex].sector1TimeInMS;
                        var sector2 = packet.lapData[packetHeader.playerCarIndex].sector2TimeInMS;
                        var bestSector1 = packet.lapData[packetHeader.playerCarIndex].bestLapSector1TimeInMS;
                        var bestSector2 = packet.lapData[packetHeader.playerCarIndex].bestLapSector2TimeInMS;
                        var bestSector3 = packet.lapData[packetHeader.playerCarIndex].bestLapSector3TimeInMS;

                        if (time >= 10) // clear lap data after 10 seconds
                        {
                            deltaSector1 = deltaSector2 = deltaLap = 0;
                        }

                        deltaSector1 = sector1 != 0 ? (sector1 - bestSector1) / 1000.0f : deltaSector1;
                        deltaSector2 = sector2 != 0 ? ((sector1 + sector2) - (bestSector1 + bestSector2)) / 1000.0f : deltaSector2;
                        deltaLap = deltaLap != 0 ? packet.lapData[packetHeader.playerCarIndex].lastLapTime - packet.lapData[packetHeader.playerCarIndex].bestLapTime : deltaLap;
                    }
                    else if (packetHeader.packetId == 6) // Car telemetry
                    {
                        var packet = PacketCarTelemetryData.FromByteArray(receiveBytes);
                        
                        speed = packet.carTelemetryData[packetHeader.playerCarIndex].speed;
                        gear = packet.carTelemetryData[packetHeader.playerCarIndex].gear;
                        rpmLevel = packet.carTelemetryData[packetHeader.playerCarIndex].revLightsPercent;
                        engineRpm = packet.carTelemetryData[packetHeader.playerCarIndex].engineRPM;
                        drs = packet.carTelemetryData[packetHeader.playerCarIndex].drs;
                    }
                    else if (packetHeader.packetId == 7) // Car status
                    {
                        var packet = PacketCarStatusData.FromByteArray(receiveBytes);
                        
                        drsAllowed = packet.carStatusData[packetHeader.playerCarIndex].drsAllowed;
                        tyreWear = packet.carStatusData[packetHeader.playerCarIndex].tyresWear;
                        ersLap = packet.carStatusData[packetHeader.playerCarIndex].ersDeployedThisLap;
                        ersMode = packet.carStatusData[packetHeader.playerCarIndex].ersDeployMode;
                        ersStore = packet.carStatusData[packetHeader.playerCarIndex].ersStoreEnergy;
                    }
                }
                else
                {
                    Thread.Sleep(50);
                }

                if (arduinoIsReadyToReceive)
                {
                    arduinoIsReadyToReceive = false;

                    var timeSpan = TimeSpan.FromSeconds(time);
                    float remainingErs = 1 - ersLap / 4000000;
                    remainingErs = Math.Clamp(remainingErs, 0, 1);

                    string data = $"{speed},{gear},{timeSpan.Minutes},{timeSpan.Seconds},{timeSpan.Milliseconds},{deltaSector1},{deltaSector2},{deltaLap}";
                    data += $",{rpmLevel / 10},{engineRpm},{tyreWear[0]},{tyreWear[1]},{tyreWear[2]},{tyreWear[3]},{ersStore / 4000000},{remainingErs},{ersMode},{drsAllowed}>";

                    serialPort.Write($"<{data}>");
                }
            }
        }

        static void ExitWithError(string error)
        {
            Console.WriteLine(error);
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
            Environment.Exit(0);
        }
    }
}
