using UnityEngine;

namespace AloneSpace
{
    public class ActorPartsExtraBoosterParameterVO
    {
        public int Id => row.Id;

        public float MainBoosterPower => row.MainBoosterPower;
        public float SubBoosterPower => row.SubBoosterPower;
        public float MaxSpeed => row.MaxSpeed;
        public float RotatePower => row.RotatePower;

        ActorPartsExtraBoosterParameterMaster.Row row;
        
        public ActorPartsExtraBoosterParameterVO(int id)
        {
            row = ActorPartsExtraBoosterParameterMaster.Instance.Get(id);
        }
    }
}