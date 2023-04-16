using UnityEngine;

namespace AloneSpace
{
    public abstract class WeaponStateData
    {
        public IPositionData TargetData { get; set; }
        public Quaternion OffsetRotation { get; set; }

        public Vector3 LookAtDirection { get; set; }

        public bool IsReloadable { get; set; }
        public bool IsExecutable { get; set; }
        public bool IsExecute { get; set; }

        public float FireTime { get; set; }
        public float ReloadTime { get; set; }
        public int ResourceIndex { get; set; }
    }
}