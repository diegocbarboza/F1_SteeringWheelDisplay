using System;

namespace TelemetryReceiver
{
    struct CarStatusData
    {
        public byte tractionControl;
        public byte antiLockBrakes;
        public byte fuelMix;
        public byte frontBrakeBias;
        public byte pitLimiterStatus;
        public float fuelInTank;
        public float fuelCapacity;
        public float fuelRemainingLaps;
        public ushort maxRPM;
        public ushort idleRPM;
        public byte maxGears;
        public byte drsAllowed;
        public ushort drsActivationDistance;
        public byte[] tyresWear;
        public byte actualTyreCompound;
        public byte visualTyreCompound;
        public byte tyresAgeLaps;
        public byte[] tyresDamage;
        public byte frontLeftWingDamage;
        public byte frontRightWingDamage;
        public byte rearWingDamage;
        public byte drsFault;
        public byte engineDamage;
        public byte gearBoxDamage;
        public sbyte vehicleFiaFlags;
        public float ersStoreEnergy;
        public byte ersDeployMode;
        public float ersHarvestedThisLapMGUK;
        public float ersHarvestedThisLapMGUH;
        public float ersDeployedThisLap;

        public static CarStatusData FromByteArray(byte[] data, int startIndex)
        {
            var carStatusData = new CarStatusData
            {
                tractionControl = data[startIndex],
                antiLockBrakes = data[startIndex + 1],
                fuelMix = data[startIndex + 2],
                frontBrakeBias = data[startIndex + 3],
                pitLimiterStatus = data[startIndex + 4],
                fuelInTank = BitConverter.ToSingle(data, startIndex + 5),
                fuelCapacity = BitConverter.ToSingle(data, startIndex + 9),
                fuelRemainingLaps = BitConverter.ToSingle(data, startIndex + 13),
                maxRPM = BitConverter.ToUInt16(data, startIndex + 17),
                idleRPM = BitConverter.ToUInt16(data, startIndex + 19),
                maxGears = data[startIndex + 21],
                drsAllowed = data[startIndex + 22],
                drsActivationDistance = BitConverter.ToUInt16(data, startIndex + 23),
                tyresWear = new byte[]
                {
                    data[startIndex + 25],
                    data[startIndex + 26],
                    data[startIndex + 27],
                    data[startIndex + 28],
                },
                actualTyreCompound = data[startIndex + 29],
                visualTyreCompound = data[startIndex + 30],
                tyresAgeLaps = data[startIndex + 31],
                tyresDamage = new byte[]
                {
                    data[startIndex + 32],
                    data[startIndex + 33],
                    data[startIndex + 34],
                    data[startIndex + 35],
                },
                frontLeftWingDamage = data[startIndex + 36],
                frontRightWingDamage = data[startIndex + 37],
                rearWingDamage = data[startIndex + 38],
                drsFault = data[startIndex + 39],
                engineDamage = data[startIndex + 40],
                gearBoxDamage = data[startIndex + 41],
                vehicleFiaFlags = (sbyte)data[startIndex + 42],
                ersStoreEnergy = BitConverter.ToSingle(data, startIndex + 43),
                ersDeployMode = data[startIndex + 47],
                ersHarvestedThisLapMGUK = BitConverter.ToSingle(data, startIndex + 48),
                ersHarvestedThisLapMGUH = BitConverter.ToSingle(data, startIndex + 52),
                ersDeployedThisLap = BitConverter.ToSingle(data, startIndex + 56)
            };


            return carStatusData;
        }
    }
}
