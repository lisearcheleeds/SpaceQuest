using System;
using AloneSpace.Common;
using UnityEngine;

namespace AloneSpace.UI
{
    public class SpaceMapViewCell : MonoBehaviour
    {
        [SerializeField] Circle3D circle3D;
        [SerializeField] LineRenderer leg;
        
        [SerializeField] Color userControlledActorColor = Color.white;
        [SerializeField] Color playerActorColor = Color.white;
        [SerializeField] Color otherActorColor = Color.white;
        [SerializeField] Color interactColor = Color.white;

        ActorData actorData;
        IInteractData interactData;

        public void Apply(ActorData actorData, bool isUserControlledActor, bool isPlayerActor)
        {
            this.actorData = actorData;
            this.interactData = null;

            transform.localPosition = actorData.Position;

            if (isUserControlledActor)
            {
                circle3D.SetColor(userControlledActorColor);
            }
            else if (isPlayerActor)
            {
                circle3D.SetColor(playerActorColor);
            }
            else
            {
                circle3D.SetColor(otherActorColor);
            }

            var axisPosition = transform.localPosition;
            axisPosition.y = 0;
            leg.positionCount = 2;
            leg.SetPositions(new[] { transform.localPosition, axisPosition });
        }

        public void Apply(IInteractData interactData)
        {
            this.actorData = null;
            this.interactData = interactData;

            transform.localPosition = interactData.Position;

            circle3D.SetColor(interactColor);
            
            var axisPosition = transform.localPosition;
            axisPosition.y = 0;
            leg.positionCount = 2;
            leg.SetPositions(new[] { transform.localPosition, axisPosition });
        }
    }
}
