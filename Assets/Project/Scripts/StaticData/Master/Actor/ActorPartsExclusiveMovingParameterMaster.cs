using System.Linq;

namespace RoboQuest
{
    public class ActorPartsExclusiveMovingParameterMaster
    {
        public class Row
        {
            // ID
            public int Id { get; }
            
            // 移動速度
            public float MovingSpeed { get; }

            // 移動速度
            public float QuickMovingSpeed { get; }
            
            // 旋回速度
            public float RotateSpeed { get; }
                    
            // 旋回速度
            public float QuickRotateSpeed { get; }

            // フロートユニット
            public bool IsFloatUnit { get; }

            public Row(
                int id,
                float movingSpeed,
                float quickMovingSpeed,
                float rotateSpeed,
                float quickRotateSpeed,
                bool isFloatUnit)
            {
                Id = id;
                MovingSpeed = movingSpeed;
                QuickMovingSpeed = quickMovingSpeed;
                RotateSpeed = rotateSpeed;
                QuickRotateSpeed = quickRotateSpeed;
                IsFloatUnit = isFloatUnit;
            }
        }

        Row[] rows;
        static ActorPartsExclusiveMovingParameterMaster instance;

        public static ActorPartsExclusiveMovingParameterMaster Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ActorPartsExclusiveMovingParameterMaster();
                }

                return instance;
            }
        }

        public Row Get(int id)
        {
            return rows.First(x => x.Id == id);
        }

        ActorPartsExclusiveMovingParameterMaster()
        {
            rows = new[]
            {
                new Row(1, 10.0f, 15.0f, 1.0f, 3.0f, false)
            };
        }
    }
}
