using System;
using UnityEngine;
using UnityEngine.UI;

namespace AloneSpace
{
    public class RadarPointMarker : MonoBehaviour
    {
        [SerializeField] Image image;

        [SerializeField] Color friendActorColor;
        [SerializeField] Color neutralActorColor;
        [SerializeField] Color enemyActorColor;
        [SerializeField] Color interactItemColor;

        public IPositionData MarkerTarget { get; private set; }
        MarkerType markerType;
        
        public enum MarkerType
        {
            FriendActor,
            NeutralActor,
            EnemyActor,
            InteractItem,
        }

        public void SetDirection(Vector3 direction, float distanceScale)
        {
            // -1~1 to 0~1
            var depth = (direction.z * 0.5f + 0.5f);
            
            transform.localPosition = direction * distanceScale;
            transform.localScale = Vector3.one * (2.0f - depth * 1.0f);
            
            image.color = GetColorFromMarkerTypeAndDepth(markerType, Mathf.Sign(direction.z) * -0.1f);
        }

        public void SetMarkerTarget(MarkerType markerType, IPositionData positionData)
        {
            MarkerTarget = positionData;
            this.markerType = markerType;
            
            image.color = GetColorFromMarkerTypeAndDepth(markerType, 0.0f);
        }

        Color GetColorFromMarkerTypeAndDepth(MarkerType markerType, float addColor)
        {
            switch (markerType)
            {
                case MarkerType.FriendActor:
                    return friendActorColor + new Color(addColor, addColor, addColor, 1.0f);
                case MarkerType.NeutralActor:
                    return neutralActorColor + new Color(addColor, addColor, addColor, 1.0f);
                case MarkerType.EnemyActor:
                    return enemyActorColor + new Color(addColor, addColor, addColor, 1.0f);
                case MarkerType.InteractItem:
                    return interactItemColor + new Color(addColor, addColor, addColor, 1.0f);
                default:
                    return interactItemColor + new Color(addColor, addColor, addColor, 1.0f);
            }
        }
    }
}