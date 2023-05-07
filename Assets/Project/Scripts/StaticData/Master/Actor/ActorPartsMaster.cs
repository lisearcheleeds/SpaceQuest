using System.Linq;

namespace AloneSpace
{
    public class ActorPartsMaster
    {
        public class Row
        {
            public int Id { get; }
            public int ParameterId { get; }
            public string Path { get; }
            public int ChildSlot { get; }
            
            public int? InventoryParameterId { get; }
            public int? SensorParameterId { get; }
            public int? BoosterParameterId { get; }
            public int? WeaponParameterId { get; }


            public Row(
                int id,
                int parameterId,
                string path,
                int childSlot,
                int? inventoryParameterId,
                int? sensorParameterId,
                int? boosterParameterId,
                int? weaponParameterId)
            {
                Id = id;
                ParameterId = parameterId;
                Path = path;
                ChildSlot = childSlot;
                InventoryParameterId = inventoryParameterId;
                SensorParameterId = sensorParameterId;
                BoosterParameterId = boosterParameterId;
                WeaponParameterId = weaponParameterId;
            }
        }

        Row[] rows;
        static ActorPartsMaster instance;

        public static ActorPartsMaster Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ActorPartsMaster();
                }

                return instance;
            }
        }

        public Row Get(int id)
        {
            return rows.First(x => x.Id == id);
        }

        ActorPartsMaster()
        {
            // FIXME 仮の想定
            // 1 Body
            // 2 Head
            // 3 LeftArm
            // 4 RightArm
            // 5 Leg
            // 6 Booster
            // 7 Tank
            // 8 LeftArmWeapon
            // 9 RightArmWeapon
            
            // 1001 Enemy1

            rows = new[]
            {
                new Row(1, 1, "RobotModel/Parts/core", 0, null, null, null, null),
                new Row(2, 2, "RobotModel/Parts/head", 0, null, null, null, null),
                new Row(3, 3, "RobotModel/Parts/leftArm", 0, null, null, null, null),
                new Row(4, 4, "RobotModel/Parts/rightArm", 0, null, null, null, null),
                new Row(5, 5, "RobotModel/Parts/leg", 0, null, null, 1, null),
                new Row(6, 6, "RobotModel/Parts/booster", 0, null, null, null, null),
                new Row(7, 7, "RobotModel/Parts/tank", 0, 1, null, null, null),
                new Row(8, 8, "RobotModel/Parts/weaponLeft", 0, null, null, null, 1),
                new Row(9, 9, "RobotModel/Parts/weaponRight", 0, null, null, null, 3),
                
                new Row(1001, 1001, "RobotModel/Standalone/Enemy1", 0, null, null, null, null),
            };
        }
    }
}