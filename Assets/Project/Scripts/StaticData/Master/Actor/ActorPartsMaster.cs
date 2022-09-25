using System.Linq;

namespace AloneSpace
{
    public class ActorPartsMaster
    {
        public class Row
        {
            // ID
            public int Id { get; }

            // 基本パラメータID
            public int ParameterId { get; }

            // モデルパス
            public string Path { get; }

            // 拡張パラメータID
            public int[] ExclusiveParameterIds { get; }
        
            // 武器パラメータID
            public int[] WeaponParameterIds { get; }

            public Row(int id, int parameterId, string path, int[] exclusiveParameterIds, int[] weaponParameterIds)
            {
                Id = id;
                ParameterId = parameterId;
                Path = path;
                ExclusiveParameterIds = exclusiveParameterIds;
                WeaponParameterIds = weaponParameterIds;
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
                new Row(1, 1, "RobotModel/Parts/core", null, null),
                new Row(2, 2, "RobotModel/Parts/head", new[] {3}, null),
                new Row(3, 3, "RobotModel/Parts/leftArm", null, null),
                new Row(4, 4, "RobotModel/Parts/rightArm", null, null),
                new Row(5, 5, "RobotModel/Parts/leg", new[] {4}, null),
                new Row(6, 6, "RobotModel/Parts/booster", null, null),
                new Row(7, 7, "RobotModel/Parts/tank", new[] {1}, null),
                new Row(8, 8, "RobotModel/Parts/weaponLeft", null, new[] {1}),
                new Row(9, 9, "RobotModel/Parts/weaponRight", null, new[] {3}),
                
                new Row(1001, 1001, "RobotModel/Standalone/Enemy1", null, null),
            };
        }
    }
}