using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelemetryReceiver
{
    struct LapData
    {
        public float lastLapTime;
        public float currentLapTime;
        public ushort sector1TimeInMS;
        public ushort sector2TimeInMS;
        public float bestLapTime;
        public byte bestLapNum;
        public ushort bestLapSector1TimeInMS;
        public ushort bestLapSector2TimeInMS;
        public ushort bestLapSector3TimeInMS;
        public ushort bestOverallSector1TimeInMS;
        public byte bestOverallSector1LapNum;
        public ushort bestOverallSector2TimeInMS;
        public byte bestOverallSector2LapNum;
        public ushort bestOverallSector3TimeInMS;
        public byte bestOverallSector3LapNum;
        public float lapDistance;
        public float totalDistance;
        public float safetyCarDelta;
        public byte carPosition;

        public static LapData FromByteArray(byte[] data, int startIndex)
        {
            var lapData = new LapData
            {
                lastLapTime = BitConverter.ToSingle(data, startIndex),
                currentLapTime = BitConverter.ToSingle(data, startIndex + 4),
                sector1TimeInMS = BitConverter.ToUInt16(data, startIndex + 8),
                sector2TimeInMS = BitConverter.ToUInt16(data, startIndex + 10),
                bestLapTime = BitConverter.ToSingle(data, startIndex + 12),
                bestLapNum = data[startIndex + 16],
                bestLapSector1TimeInMS = BitConverter.ToUInt16(data, startIndex + 17),
                bestLapSector2TimeInMS = BitConverter.ToUInt16(data, startIndex + 19),
                bestLapSector3TimeInMS = BitConverter.ToUInt16(data, startIndex + 21),

                bestOverallSector1TimeInMS = BitConverter.ToUInt16(data, startIndex + 23),
                bestOverallSector1LapNum = data[startIndex + 25],
                bestOverallSector2TimeInMS = BitConverter.ToUInt16(data, startIndex + 26),
                bestOverallSector2LapNum = data[startIndex + 28],
                bestOverallSector3TimeInMS = BitConverter.ToUInt16(data, startIndex + 29),
                bestOverallSector3LapNum = data[startIndex + 31],

                lapDistance = BitConverter.ToSingle(data, startIndex + 32),
                totalDistance = BitConverter.ToSingle(data, startIndex + 36),
                safetyCarDelta = BitConverter.ToSingle(data, startIndex + 40),
                carPosition = data[startIndex + 44]
            };

            return lapData;
        }
    }
}
