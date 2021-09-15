namespace TelemetryReceiver
{
    struct PacketCarTelemetryData
    {
        public PacketHeader header;
        public CarTelemetryData[] carTelemetryData;
        public uint buttonStatus;
        public byte mfdPanelIndex;
        public byte mfdPanelIndexSecondaryPlayer;
        public sbyte suggestedGear;

        public static PacketCarTelemetryData FromByteArray(byte[] data)
        {
            var packetCarTelemetryData = new PacketCarTelemetryData
            {
                header = PacketHeader.FromByteArray(data),
                carTelemetryData = new CarTelemetryData[22],
            };

            for (int i = 0; i < 22; i++)
            {
                packetCarTelemetryData.carTelemetryData[i] = CarTelemetryData.FromByteArray(data, 24 + i * 58);
            }

            return packetCarTelemetryData;
        }
    }
}