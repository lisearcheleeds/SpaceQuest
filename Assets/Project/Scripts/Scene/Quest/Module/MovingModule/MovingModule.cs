using UnityEngine;

namespace AloneSpace
{
    /// <summary>
    /// 毎フレーム呼び出される移動処理
    /// </summary>
    public class MovingModule : IModule
    {
        IPositionData positionData;

        public Vector3 MovementVelocity { get; private set; }
        public Quaternion QuaternionVelocityLHS { get; private set; } = Quaternion.identity;
        public Quaternion QuaternionVelocityRHS { get; private set; } = Quaternion.identity;

        public MovingModule(IPositionData positionData)
        {
            this.positionData = positionData;
        }

        public void ActivateModule()
        {
            MessageBus.Instance.RegisterMovingModule.Broadcast(this);
        }

        public void DeactivateModule()
        {
            MessageBus.Instance.UnRegisterMovingModule.Broadcast(this);
        }

        public void SetMovementVelocity(Vector3 movementVelocity)
        {
            MovementVelocity = movementVelocity;
        }

        public void SetQuaternionVelocityLHS(Quaternion quaternion)
        {
            QuaternionVelocityLHS = quaternion;
        }

        public void SetQuaternionVelocityRHS(Quaternion quaternion)
        {
            QuaternionVelocityRHS = quaternion;
        }

        public void OnUpdateModule(float deltaTime)
        {
            positionData.SetPosition(positionData.Position + MovementVelocity * deltaTime);
            positionData.SetRotation(QuaternionVelocityLHS * positionData.Rotation * QuaternionVelocityRHS);
        }
    }
}
