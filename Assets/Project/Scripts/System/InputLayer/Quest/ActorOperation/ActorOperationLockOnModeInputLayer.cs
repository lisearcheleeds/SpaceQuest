using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace AloneSpace
{
    public class ActorOperationLockOnModeInputLayer : ActorOperationInputLayer
    {
        UserData userData;

        public ActorOperationLockOnModeInputLayer(UserData userData)
        {
            this.userData = userData;
        }

        public override bool UpdateInput(ButtonControl[] usedKey)
        {
            CheckWeaponKeys(usedKey);
            return false;
        }
    }
}
