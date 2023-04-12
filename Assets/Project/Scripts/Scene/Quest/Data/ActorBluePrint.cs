using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AloneSpace
{
    public class ActorBluePrint
    {
        public Dictionary<int, int[]> PartsHierarchy { get; } = new Dictionary<int, int[]>();

        public ActorBluePrint()
        {
            // Body
            PartsHierarchy.Add(0, new[] {1});
            
            // Head, LeftArm, RightArm, Leg, Booster, Tank
            PartsHierarchy.Add(1, new[] {2, 3, 4, 5, 6, 7});
            
            // LeftArmWeapon
            PartsHierarchy.Add(3, new[] {8});
            
            // RightArmWeapon
            PartsHierarchy.Add(4, new[] {9});
        }
        
        public ActorBluePrint(int standaloneId)
        {
            // Body
            PartsHierarchy.Add(0, new[] { standaloneId });
        }
    }
}
