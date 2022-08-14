using System;
using System.Linq;
using UnityEngine;

namespace RoboQuest.Quest
{
    public class AreaTransitionInteractData : IInteractData
    {
        static readonly float DefaultScale = 100.0f;
        static readonly Vector3 DefaultPosition = new Vector3(0, 0.5f, 99);
        static readonly float InteractionRange = 2.0f;

        public Guid InstanceId { get; }

        public int AreaIndex { get; }
        public Vector3 Position { get; set; }
        public string Text => $"Transition {AreaDirection}";
        public float InteractTime => 3.0f;

        public AreaDirection? AreaDirection { get; set; }
        public int TransitionAreaIndex { get; set; }
        public Quaternion Rotation { get; set; }
        public float Scale { get; set; }

        public AreaTransitionInteractData(int areaIndex, AreaDirection areaDirection, int transitionAreaIndex, float areaSize)
        {
            InstanceId = Guid.NewGuid();

            AreaIndex = areaIndex;
            AreaDirection = areaDirection;
            TransitionAreaIndex = transitionAreaIndex;
            (Position, Rotation) = GetPositionAndRotation(areaDirection, areaSize / DefaultScale);
            Scale = areaSize;
        }
        
        public AreaTransitionInteractData(int areaIndex, int transitionAreaIndex, Vector3 position, Quaternion rotation)
        {
            InstanceId = Guid.NewGuid();

            AreaIndex = areaIndex;
            AreaDirection = null;
            TransitionAreaIndex = transitionAreaIndex;
            Position = position;
            Rotation = rotation;
            Scale = 1;
        }
        
        public Vector3 GetClosestPoint(Vector3 position)
        {
            // 線分で判定
            var startPoint = Position + Rotation * (Vector3.right * DefaultScale * 0.5f);
            var endPoint = Position + Rotation * (Vector3.right * DefaultScale * -0.5f);

            var dir = (endPoint - startPoint).normalized;
            var dot = Mathf.Clamp(Vector3.Dot(position - startPoint, dir), 0, DefaultScale);
            return startPoint + dir * dot + Vector3.up * position.y;
        }

        public bool IsInteractionRange(Vector3 position)
        {
            return (position - GetClosestPoint(position)).sqrMagnitude < InteractionRange * InteractionRange;
        }
        
        (Vector3, Quaternion) GetPositionAndRotation(AreaDirection? areaDirection, float scale)
        {
            float angle = 0;
            switch (areaDirection)
            {
                case RoboQuest.AreaDirection.Top:
                    angle = 0;
                    break;
                case RoboQuest.AreaDirection.TopLeft:
                    angle = -60;
                    break;
                case RoboQuest.AreaDirection.TopRight:
                    angle = 60;
                    break;
                case RoboQuest.AreaDirection.BottomLeft:
                    angle = -120;
                    break;
                case RoboQuest.AreaDirection.BottomRight:
                    angle = 120;
                    break;
                case RoboQuest.AreaDirection.Bottom:
                    angle = 180;
                    break;
            }

            var rotation = Quaternion.AngleAxis(angle, Vector3.up);
            var position = rotation * DefaultPosition * scale;
            return (position, rotation);
        }
    }
}