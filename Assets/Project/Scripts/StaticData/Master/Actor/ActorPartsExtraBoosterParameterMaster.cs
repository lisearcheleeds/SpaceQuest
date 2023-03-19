using System.Linq;

namespace AloneSpace
{
    public class ActorPartsExtraBoosterParameterMaster
    {
        public class Row
        {
            // ID
            public int Id { get; }
         
            public float MainBoosterPower { get; }
            public float SubBoosterPower { get; }
            public float MaxSpeed { get; }
            public float RotatePower { get; }

            public Row(
                int id,
                float mainBoosterPower,
                float subBoosterPower,
                float maxSpeed,
                float rotatePower)
            {
                Id = id;
                MainBoosterPower = mainBoosterPower;
                SubBoosterPower = subBoosterPower;
                MaxSpeed = maxSpeed;
                RotatePower = rotatePower;
            }
        }

        Row[] rows;
        static ActorPartsExtraBoosterParameterMaster instance;

        public static ActorPartsExtraBoosterParameterMaster Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ActorPartsExtraBoosterParameterMaster();
                }

                return instance;
            }
        }

        public Row Get(int id)
        {
            return rows.First(x => x.Id == id);
        }

        ActorPartsExtraBoosterParameterMaster()
        {
            rows = new[]
            {
                new Row(
                    id: 1,
                    mainBoosterPower: 0.005f,
                    subBoosterPower: 0.005f,
                    maxSpeed: 1.0f,
                    rotatePower: 1.0f)
            };
        }
    }
}
