using System;

namespace TelemetryReceiver
{
    struct PacketHeader
    {
        public ushort packetFormat;
        public byte gameMajorVersion;
        public byte gameMinorVersion;
        public byte packetVersion;
        public byte packetId;
        public ulong sessionUID;
        public float sessionTime;
        public uint frameIdentifier;
        public byte playerCarIndex;
        public byte secondaryPlayerCarIndex;

        public static PacketHeader FromByteArray(byte[] data)
        {
            var packetHeader = new PacketHeader
            {
                packetFormat = BitConverter.ToUInt16(data, 0),
                gameMajorVersion = data[2],
                gameMinorVersion = data[3],
                packetVersion = data[4],
                packetId = data[5],
                sessionUID = BitConverter.ToUInt64(data, 6),
                sessionTime = BitConverter.ToSingle(data, 14),
                frameIdentifier = BitConverter.ToUInt32(data, 18),
                playerCarIndex = data[22],
                secondaryPlayerCarIndex = data[23]
            };

            return packetHeader;
        }
    }
}
