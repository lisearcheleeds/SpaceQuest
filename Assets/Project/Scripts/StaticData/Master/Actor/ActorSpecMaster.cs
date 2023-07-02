using System.Linq;

namespace AloneSpace
{
    public class ActorSpecMaster
    {
        public class Row
        {
            public int Id { get; }
            public AssetPath Path { get; }

            // 破壊グラフィックアセット
            public int BrokenActorGraphicEffectSpecMasterId { get; }

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
            public float PitchRotatePower { get; }
            public float YawRotatePower { get; }
            public float RollRotatePower { get; }

            // インベントリ
            public int CapacityWidth { get; }
            public int CapacityHeight { get; }

            // 距離
            public float VisionSensorDistance { get; }
            public float RadarSensorPerformance { get; }

            public Row(
                int id,
                AssetPath path,
                int brokenActorGraphicEffectSpecMasterId,
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
                float pitchRotatePower,
                float yawRotatePower,
                float rollRotatePower,
                int capacityWidth,
                int capacityHeight,
                float visionSensorDistance,
                float radarSensorPerformance)
            {
                Id = id;
                Path = path;
                BrokenActorGraphicEffectSpecMasterId = brokenActorGraphicEffectSpecMasterId;
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
                PitchRotatePower = pitchRotatePower;
                YawRotatePower = yawRotatePower;
                RollRotatePower = rollRotatePower;
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
                    path: new AssetPath("Prefab/Actor/Krishna"),
                    brokenActorGraphicEffectSpecMasterId: 10001,
                    enduranceValue: 200,
                    shieldValue: 100,
                    shieldTruncateValue: 2,
                    shieldAutoRecoveryResilienceTime: 2,
                    shieldAutoRecoveryValue: 1,
                    electronicProtectionValue: 100,
                    electronicProtectionTruncateValue: 2,
                    electronicProtectionAutoRecoveryResilienceTime:10 ,
                    electronicProtectionAutoRecoveryValue: 2,
                    weaponSlotCount: 6,
                    mainBoosterPower: 2f,
                    subBoosterPower: 2f,
                    maxSpeed: 50.0f,
                    pitchRotatePower: 60.0f,
                    yawRotatePower: 30.0f,
                    rollRotatePower: 240.0f,
                    capacityWidth: 8,
                    capacityHeight: 6,
                    visionSensorDistance: 100,
                    radarSensorPerformance: 300),
                new Row(
                    id: 2,
                    path: new AssetPath("Prefab/Actor/Arjuna"),
                    brokenActorGraphicEffectSpecMasterId: 10002,
                    enduranceValue: 100,
                    shieldValue: 100,
                    shieldTruncateValue: 2,
                    shieldAutoRecoveryResilienceTime: 2,
                    shieldAutoRecoveryValue: 1,
                    electronicProtectionValue: 100,
                    electronicProtectionTruncateValue: 2,
                    electronicProtectionAutoRecoveryResilienceTime:10 ,
                    electronicProtectionAutoRecoveryValue: 2,
                    weaponSlotCount: 5,
                    mainBoosterPower: 0.2f,
                    subBoosterPower: 0.2f,
                    maxSpeed: 50.0f,
                    pitchRotatePower: 60.0f,
                    yawRotatePower: 30.0f,
                    rollRotatePower: 240.0f,
                    capacityWidth: 8,
                    capacityHeight: 6,
                    visionSensorDistance: 100,
                    radarSensorPerformance: 300),
                new Row(
                    id: 3,
                    path: new AssetPath("Prefab/Actor/Ilis"),
                    brokenActorGraphicEffectSpecMasterId: 10003,
                    enduranceValue: 100,
                    shieldValue: 100,
                    shieldTruncateValue: 2,
                    shieldAutoRecoveryResilienceTime: 2,
                    shieldAutoRecoveryValue: 1,
                    electronicProtectionValue: 100,
                    electronicProtectionTruncateValue: 2,
                    electronicProtectionAutoRecoveryResilienceTime:10 ,
                    electronicProtectionAutoRecoveryValue: 2,
                    weaponSlotCount: 1,
                    mainBoosterPower: 0.2f,
                    subBoosterPower: 0.2f,
                    maxSpeed: 50.0f,
                    pitchRotatePower: 60.0f,
                    yawRotatePower: 30.0f,
                    rollRotatePower: 240.0f,
                    capacityWidth: 8,
                    capacityHeight: 6,
                    visionSensorDistance: 100,
                    radarSensorPerformance: 300),
                new Row(
                    id: 4,
                    path: new AssetPath("Prefab/Actor/Transporter"),
                    brokenActorGraphicEffectSpecMasterId: 10004,
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
                    mainBoosterPower: 0.2f,
                    subBoosterPower: 0.2f,
                    maxSpeed: 50.0f,
                    pitchRotatePower: 60.0f,
                    yawRotatePower: 30.0f,
                    rollRotatePower: 240.0f,
                    capacityWidth: 8,
                    capacityHeight: 6,
                    visionSensorDistance: 100,
                    radarSensorPerformance: 300),
                new Row(
                    id: 5,
                    path: new AssetPath("Prefab/Actor/WarShip"),
                    brokenActorGraphicEffectSpecMasterId: 10005,
                    enduranceValue: 10000,
                    shieldValue: 100,
                    shieldTruncateValue: 2,
                    shieldAutoRecoveryResilienceTime: 2,
                    shieldAutoRecoveryValue: 1,
                    electronicProtectionValue: 100,
                    electronicProtectionTruncateValue: 2,
                    electronicProtectionAutoRecoveryResilienceTime:10 ,
                    electronicProtectionAutoRecoveryValue: 2,
                    weaponSlotCount: 10,
                    mainBoosterPower: 0.2f,
                    subBoosterPower: 0.2f,
                    maxSpeed: 25.0f,
                    pitchRotatePower: 5.0f,
                    yawRotatePower: 10.0f,
                    rollRotatePower: 5.0f,
                    capacityWidth: 8,
                    capacityHeight: 6,
                    visionSensorDistance: 100,
                    radarSensorPerformance: 300),
            };
        }
    }
}
