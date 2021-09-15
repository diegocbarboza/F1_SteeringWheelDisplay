
namespace TelemetryReceiver
{
    struct PacketLapData
    {
        public PacketHeader header;
        public LapData[] lapData;

        public static PacketLapData FromByteArray(byte[] data)
        {
            var packetLapData = new PacketLapData
            {
                header = PacketHeader.FromByteArray(data),
                lapData = new LapData[22]
            };

            for (int i = 0; i < 22; i++)
            {
                packetLapData.lapData[i] = LapData.FromByteArray(data, 24 + i * 53);
            }

            return packetLapData;
        }
    }
}
