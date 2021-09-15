namespace TelemetryReceiver
{
    struct PacketCarStatusData
    {
        public PacketHeader header;
        public CarStatusData[] carStatusData;

        public static PacketCarStatusData FromByteArray(byte[] data)
        {
            var packetCarStatusData = new PacketCarStatusData
            {
                header = PacketHeader.FromByteArray(data),
                carStatusData = new CarStatusData[22]
            };

            for (int i = 0; i < 22; i++)
            {
                packetCarStatusData.carStatusData[i] = CarStatusData.FromByteArray(data, 24 + i * 60);
            }

            return packetCarStatusData;
        }
    }
}
