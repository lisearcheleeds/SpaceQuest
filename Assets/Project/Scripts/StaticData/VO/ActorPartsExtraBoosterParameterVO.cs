using UnityEngine;

namespace AloneSpace
{
    public class ActorPartsExtraBoosterParameterVO
    {
        public int Id => row.Id;

        public float BoosterPower => row.BoosterPower;
        public float MainDirectionRatio => row.MainDirectionRatio;
        public float SubDirectionRatio => row.SubDirectionRatio;
        public float MainRotateRatio => row.MainRotateRatio;
        public float SubRotateRatio => row.SubRotateRatio;

        ActorPartsExtraBoosterParameterMaster.Row row;
        
        public ActorPartsExtraBoosterParameterVO(int id)
        {
            row = ActorPartsExtraBoosterParameterMaster.Instance.Get(id);
        }
    }
}