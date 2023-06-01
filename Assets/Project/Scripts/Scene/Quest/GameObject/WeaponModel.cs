using System;
using System.Linq;
using UnityEngine;

namespace AloneSpace
{
    public class WeaponModel : MonoBehaviour
    {
        [SerializeField] Transform[] outputPoint;

        IPositionData positionData;

        public void Init(IPositionData positionData)
        {
            this.positionData = positionData;
        }

        public WeaponFeedback GetWeaponFeedback()
        {
            return new WeaponFeedback(outputPoint.Select(x => new TransformPositionData(positionData, x)).ToArray());
        }
    }
}