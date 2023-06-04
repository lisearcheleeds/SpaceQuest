using System.Collections.Generic;
using UnityEngine;

namespace AloneSpace
{
    public abstract class WeaponStateData
    {
        public IPositionData TargetData { get; set; }
        public Quaternion OffsetRotation { get; set; } = Quaternion.identity;

        public Vector3 LookAtDirection { get; set; } = Vector3.forward;

        public bool IsReloadable { get; set; }
        public bool IsExecutable { get; set; }
        public bool IsExecute { get; set; }

        public float FireTime { get; set; }
        public float ReloadRemainTime { get; set; }
        public int ResourceIndex { get; set; }

        public List<WeaponEffectData> WeaponEffectDataList { get; set; } = new List<WeaponEffectData>();
    }
}