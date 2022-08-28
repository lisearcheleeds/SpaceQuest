using UnityEngine;

namespace AloneSpace.InSide
{
    public class ActorDebugger : MonoBehaviour
    {
        [SerializeField] string actorId;
        
        ActorAI actorAI;
        
        public void Setup(ActorAI actorAI)
        {
            this.actorAI = actorAI;
            actorId = actorAI.ActorAIHandler.ActorData.InstanceId.ToString();
        }

        void OnDrawGizmosSelected()
        {
            if (actorAI == null)
            {
                return;
            }

            DrawGizmosCheckJump();
            DrawGizmoActorPathFinder();
        }

        void DrawGizmosCheckJump()
        {
            /*
            var currentPosition = transform.position;
            var relativePosition = actorAI.ActorAIHandler.TargetMovePoint - transform.position;
            
            var checkDistance = 5.0f;
            var offsetZ = 2.0f;
            
            var wayDirection = new Vector3(relativePosition.x, 0, relativePosition.z);
            
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(currentPosition + wayDirection.normalized * offsetZ, currentPosition + wayDirection);
            */            
        }

        void DrawGizmoActorPathFinder()
        {
            if (!actorAI.ActorAIHandler.ActorPathFinder.HasPath)
            {
                return;
            }

            var corners = actorAI.ActorAIHandler.ActorPathFinder.Corners;
            var currentPosition = actorAI.ActorAIHandler.ActorData.Position;
            var endPosition = actorAI.ActorAIHandler.ActorPathFinder.EndPosition;
            var relativePosition = endPosition - currentPosition;
            relativePosition.y = 0;
            var directionXZ = relativePosition.normalized;
            
            Gizmos.color = Color.green;

            for (var i = 0; i < corners?.Length - 1; i++)
            {
                Gizmos.DrawLine(corners[i], corners[i + 1]);                
            }

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(currentPosition, endPosition);
           
            var offset = 2.0f;
            var directRay = new Ray(actorAI.ActorAIHandler.ActorData.Position + Vector3.up * offset, directionXZ);
            if (Physics.Raycast(directRay, out var directRaycastHit))
            {
                Gizmos.DrawSphere(directRaycastHit.point, 1.0f);
            }
        }
    }
}