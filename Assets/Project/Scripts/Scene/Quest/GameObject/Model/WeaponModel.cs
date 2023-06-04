using System;
using System.Linq;
using UnityEngine;

namespace AloneSpace
{
    public class WeaponModel : GameObjectModel<WeaponGameObjectHandler>
    {
        [SerializeField] Transform[] outputPoint;

        protected override WeaponGameObjectHandler OnInit(IPositionData positionData)
        {
            return new WeaponGameObjectHandler(outputPoint.Select(x => new TransformPositionData(positionData, x)).ToArray());
        }
    }
}