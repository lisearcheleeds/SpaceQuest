using UnityEngine;

namespace RoboQuest
{
    public class ActorPartsExclusiveMovingParameterVO : IActorPartsExclusiveParameterVO
    {
        public int Id => row.Id;

        public ActorPartsExclusiveType ActorPartsExclusiveType => ActorPartsExclusiveType.Moving;
            
        // 移動速度
        public float MovingSpeed => row.MovingSpeed;

        // 移動速度
        public float QuickMovingSpeed => row.QuickMovingSpeed;
            
        // 旋回速度
        public float RotateSpeed => row.RotateSpeed;
                    
        // 旋回速度
        public float QuickRotateSpeed => row.QuickRotateSpeed;

        // フロートユニット
        public bool IsFloatUnit => row.IsFloatUnit;

        ActorPartsExclusiveMovingParameterMaster.Row row;
        
        public ActorPartsExclusiveMovingParameterVO(int id)
        {
            row = ActorPartsExclusiveMovingParameterMaster.Instance.Get(id);
        }
    }
}