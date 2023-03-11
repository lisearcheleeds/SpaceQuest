using System.Linq;

namespace AloneSpace
{
    public class ActorPartsExtraBoosterParameterMaster
    {
        public class Row
        {
            // ID
            public int Id { get; }
            
            public float BoosterPower { get; }
            public float MainDirectionRatio { get; }
            public float SubDirectionRatio { get; }
            public float MainRotateRatio { get; }
            public float SubRotateRatio { get; }

            public Row(
                int id,
                float boosterPower,
                float mainDirectionRatio,
                float subDirectionRatio,
                float mainRotateRatio,
                float subRotateRatio)
            {
                Id = id;
                BoosterPower = boosterPower;
                MainDirectionRatio = mainDirectionRatio;
                SubDirectionRatio = subDirectionRatio;
                MainRotateRatio = mainRotateRatio;
                SubRotateRatio = subRotateRatio;
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
                    boosterPower: 0.005f,
                    mainDirectionRatio: 1.0f,
                    subDirectionRatio: 1.0f,
                    mainRotateRatio: 200.0f,
                    subRotateRatio: 200.0f)
            };
        }
    }
}
