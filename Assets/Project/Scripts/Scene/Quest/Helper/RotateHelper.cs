using UnityEngine;

namespace AloneSpace
{
    public static class RotateHelper
    {
        public static Vector3? GetCatchUpToDirection(Vector3 targetVelocity, Vector3 targetPosition, Vector3 baseVelocity, Vector3 basePosition)
        {
            var relativePosition = targetPosition - basePosition;
            var relativeVelocity = targetVelocity - baseVelocity;
            if (relativeVelocity != Vector3.zero)
            {
                var collisionTime = relativePosition.magnitude / relativeVelocity.magnitude;
                var targetTimedRelativePosition = (targetPosition + targetVelocity * collisionTime) - basePosition;
                return targetTimedRelativePosition.normalized;
            }

            return null;
        }
    }
}