using System.Linq;

namespace AloneSpace
{
    public class ActorSpecMaster
    {
        public class Row : IAssetPath
        {
            public int Id { get; }
            public string Path { get; }

            // 耐久
            public float EnduranceValue { get; }

            public float ShieldValue { get; }
            public float ShieldTruncateValue { get; }
            public float ShieldAutoRecoveryResilienceTime { get; }
            public float ShieldAutoRecoveryValue { get; }

            public float ElectronicProtectionValue { get; }
            public float ElectronicProtectionTruncateValue { get; }
            public float ElectronicProtectionAutoRecoveryResilienceTime { get; }
            public float ElectronicProtectionAutoRecoveryValue { get; }

            // 武器
            public int WeaponSlotCount { get; }

            // ブースター
            public float MainBoosterPower { get; }
            public float SubBoosterPower { get; }
            public float MaxSpeed { get; }
            public float RotatePower { get; }

            // インベントリ
            public int CapacityWidth { get; }
            public int CapacityHeight { get; }

            // 距離
            public float VisionSensorDistance { get; }
            public float RadarSensorPerformance { get; }

            public Row(
                int id,
                string path,
                float enduranceValue,
                float shieldValue,
                float shieldTruncateValue,
                float shieldAutoRecoveryResilienceTime,
                float shieldAutoRecoveryValue,
                float electronicProtectionValue,
                float electronicProtectionTruncateValue,
                float electronicProtectionAutoRecoveryResilienceTime,
                float electronicProtectionAutoRecoveryValue,
                int weaponSlotCount,
                float mainBoosterPower,
                float subBoosterPower,
                float maxSpeed,
                float rotatePower,
                int capacityWidth,
                int capacityHeight,
                float visionSensorDistance,
                float radarSensorPerformance)
            {
                Id = id;
                Path = path;
                EnduranceValue = enduranceValue;
                ShieldValue = shieldValue;
                ShieldTruncateValue = shieldTruncateValue;
                ShieldAutoRecoveryResilienceTime = shieldAutoRecoveryResilienceTime;
                ShieldAutoRecoveryValue = shieldAutoRecoveryValue;
                ElectronicProtectionValue = electronicProtectionValue;
                ElectronicProtectionTruncateValue = electronicProtectionTruncateValue;
                ElectronicProtectionAutoRecoveryResilienceTime = electronicProtectionAutoRecoveryResilienceTime;
                ElectronicProtectionAutoRecoveryValue = electronicProtectionAutoRecoveryValue;
                WeaponSlotCount = weaponSlotCount;
                MainBoosterPower = mainBoosterPower;
                SubBoosterPower = subBoosterPower;
                MaxSpeed = maxSpeed;
                RotatePower = rotatePower;
                CapacityWidth = capacityWidth;
                CapacityHeight = capacityHeight;
                VisionSensorDistance = visionSensorDistance;
                RadarSensorPerformance = radarSensorPerformance;
            }
        }

        Row[] rows;
        static ActorSpecMaster instance;

        public static ActorSpecMaster Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ActorSpecMaster();
                }

                return instance;
            }
        }

        public Row Get(int id)
        {
            return rows.First(x => x.Id == id);
        }

        ActorSpecMaster()
        {
            rows = new[]
            {
                new Row(
                    id: 1,
                    path: "Prefab/Actor/Krishna",
                    enduranceValue: 100,
                    shieldValue: 100,
                    shieldTruncateValue: 2,
                    shieldAutoRecoveryResilienceTime: 2,
                    shieldAutoRecoveryValue: 1,
                    electronicProtectionValue: 100,
                    electronicProtectionTruncateValue: 2,
                    electronicProtectionAutoRecoveryResilienceTime:10 ,
                    electronicProtectionAutoRecoveryValue: 2,
                    weaponSlotCount: 4,
                    mainBoosterPower: 0.05f,
                    subBoosterPower: 0.05f,
                    maxSpeed: 10.0f,
                    rotatePower: 60.0f,
                    capacityWidth: 8,
                    capacityHeight: 6,
                    visionSensorDistance: 100,
                    radarSensorPerformance: 300),
                new Row(
                    id: 2,
                    path: "Prefab/Actor/Arjuna",
                    enduranceValue: 100,
                    shieldValue: 100,
                    shieldTruncateValue: 2,
                    shieldAutoRecoveryResilienceTime: 2,
                    shieldAutoRecoveryValue: 1,
                    electronicProtectionValue: 100,
                    electronicProtectionTruncateValue: 2,
                    electronicProtectionAutoRecoveryResilienceTime:10 ,
                    electronicProtectionAutoRecoveryValue: 2,
                    weaponSlotCount: 4,
                    mainBoosterPower: 0.05f,
                    subBoosterPower: 0.05f,
                    maxSpeed: 10.0f,
                    rotatePower: 60.0f,
                    capacityWidth: 8,
                    capacityHeight: 6,
                    visionSensorDistance: 100,
                    radarSensorPerformance: 300),
                new Row(
                    id: 3,
                    path: "Prefab/Actor/Ilis",
                    enduranceValue: 100,
                    shieldValue: 100,
                    shieldTruncateValue: 2,
                    shieldAutoRecoveryResilienceTime: 2,
                    shieldAutoRecoveryValue: 1,
                    electronicProtectionValue: 100,
                    electronicProtectionTruncateValue: 2,
                    electronicProtectionAutoRecoveryResilienceTime:10 ,
                    electronicProtectionAutoRecoveryValue: 2,
                    weaponSlotCount: 4,
                    mainBoosterPower: 0.05f,
                    subBoosterPower: 0.05f,
                    maxSpeed: 10.0f,
                    rotatePower: 60.0f,
                    capacityWidth: 8,
                    capacityHeight: 6,
                    visionSensorDistance: 100,
                    radarSensorPerformance: 300),
                new Row(
                    id: 4,
                    path: "Prefab/Actor/Transporter",
                    enduranceValue: 100,
                    shieldValue: 100,
                    shieldTruncateValue: 2,
                    shieldAutoRecoveryResilienceTime: 2,
                    shieldAutoRecoveryValue: 1,
                    electronicProtectionValue: 100,
                    electronicProtectionTruncateValue: 2,
                    electronicProtectionAutoRecoveryResilienceTime:10 ,
                    electronicProtectionAutoRecoveryValue: 2,
                    weaponSlotCount: 4,
                    mainBoosterPower: 0.05f,
                    subBoosterPower: 0.05f,
                    maxSpeed: 10.0f,
                    rotatePower: 60.0f,
                    capacityWidth: 8,
                    capacityHeight: 6,
                    visionSensorDistance: 100,
                    radarSensorPerformance: 300),
                new Row(
                    id: 5,
                    path: "Prefab/Actor/WarShip",
                    enduranceValue: 100,
                    shieldValue: 100,
                    shieldTruncateValue: 2,
                    shieldAutoRecoveryResilienceTime: 2,
                    shieldAutoRecoveryValue: 1,
                    electronicProtectionValue: 100,
                    electronicProtectionTruncateValue: 2,
                    electronicProtectionAutoRecoveryResilienceTime:10 ,
                    electronicProtectionAutoRecoveryValue: 2,
                    weaponSlotCount: 4,
                    mainBoosterPower: 0.05f,
                    subBoosterPower: 0.05f,
                    maxSpeed: 10.0f,
                    rotatePower: 60.0f,
                    capacityWidth: 8,
                    capacityHeight: 6,
                    visionSensorDistance: 100,
                    radarSensorPerformance: 300),
            };
        }
    }
}