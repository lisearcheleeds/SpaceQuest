using System.Linq;

namespace AloneSpace
{
    public class ActorPartsParameterMaster
    {
        public class Row
        {
            // ID
            public int Id { get; }
        
            // 耐久力
            public float Endurance { get; }
         
            // 対物性
            public float KineticResistant { get; }

            // 耐熱性
            public float HeatResistant { get; }

            // 耐爆性
            public float BlastResistant { get; }

            public Row(
                int id,
                float endurance,
                float kineticResistant,
                float heatResistant,
                float blastResistant)
            {
                Id = id;
                Endurance = endurance;
                KineticResistant = kineticResistant;
                HeatResistant = heatResistant;
                BlastResistant = blastResistant;
            }
        }
        
        Row[] rows;
        static ActorPartsParameterMaster instance;

        public static ActorPartsParameterMaster Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ActorPartsParameterMaster();
                }

                return instance;
            }
        }

        public Row Get(int id)
        {
            return rows.First(x => x.Id == id);
        }

        ActorPartsParameterMaster()
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
            
            rows = new[]
            {
                new Row(1, 10, 10, 10, 10),
                new Row(2, 10, 10, 10, 10),
                new Row(3, 10, 10, 10, 10),
                new Row(4, 10, 10, 10, 10),
                new Row(5, 10, 10, 10, 10),
                new Row(6, 10, 10, 10, 10),
                new Row(7, 10, 10, 10, 10),
                new Row(8, 10, 10, 10, 10),
                new Row(9, 10, 10, 10, 10),
                
                new Row(1001, 10, 10, 10, 10),
            };
        }
    }
}
