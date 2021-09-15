using System;
namespace TelemetryReceiver
{
    struct CarTelemetryData
    {
        public ushort speed;
        public float throttle;
        public float steer;
        public float brake;
        public byte clutch;
        public sbyte gear;
        public ushort engineRPM;
        public byte drs;
        public byte revLightsPercent;
        public ushort[] brakesTemperature;
        public byte[] tyresSurfaceTemperature;
        public byte[] tyresInnerTemperature;
        public ushort engineTemperature;
        public float[] tyresPressure;
        public byte[] surfaceType;

        public static CarTelemetryData FromByteArray(byte[] data, int startIndex)
        {
            var carTelemetryData = new CarTelemetryData
            {
                speed = BitConverter.ToUInt16(data, startIndex),
                throttle = BitConverter.ToSingle(data, startIndex + 2),
                steer = BitConverter.ToSingle(data, startIndex + 6),
                brake = BitConverter.ToSingle(data, startIndex + 10),
                clutch = data[startIndex + 14],
                gear = (sbyte)data[startIndex + 15],
                engineRPM = BitConverter.ToUInt16(data, startIndex + 16),
                drs = data[startIndex + 18],
                revLightsPercent = data[startIndex + 19],
                brakesTemperature = new ushort[]
                {
                    BitConverter.ToUInt16(data, startIndex + 20),
                    BitConverter.ToUInt16(data, startIndex + 22),
                    BitConverter.ToUInt16(data, startIndex + 24),
                    BitConverter.ToUInt16(data, startIndex + 26),
                },
                tyresSurfaceTemperature = new byte[]
                {
                    data[startIndex + 28],
                    data[startIndex + 29],
                    data[startIndex + 30],
                    data[startIndex + 31],
                },
                tyresInnerTemperature = new byte[]
                {
                    data[startIndex + 32],
                    data[startIndex + 33],
                    data[startIndex + 34],
                    data[startIndex + 35],
                },
                engineTemperature = BitConverter.ToUInt16(data, startIndex + 36),
                tyresPressure = new float[]
                {
                    BitConverter.ToSingle(data, startIndex + 38),
                    BitConverter.ToSingle(data, startIndex + 42),
                    BitConverter.ToSingle(data, startIndex + 46),
                    BitConverter.ToSingle(data, startIndex + 50)
                },
                surfaceType = new byte[]
                {
                    data[startIndex + 54],
                    data[startIndex + 55],
                    data[startIndex + 56],
                    data[startIndex + 57],
                }
            };

            return carTelemetryData;
        }
    }
}