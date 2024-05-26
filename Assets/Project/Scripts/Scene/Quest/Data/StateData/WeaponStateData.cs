using System.Collections.Generic;
using UnityEngine;

namespace AloneSpace
{
    public abstract class WeaponStateData
    {
        public IPositionData TargetData { get; set; }
        public virtual Quaternion OffsetRotation { get; set; } = Quaternion.identity;

        public virtual Vector3 LookAtDirection { get; set; } = Vector3.forward;

        public virtual bool IsReloadable { get; set; }
        public virtual bool IsExecutable { get; set; }
        public virtual bool IsTargetInRange { get; set; }
        public virtual bool IsTargetInAngle { get; set; }
        public bool IsExecute { get; set; }

        public virtual float FireTime { get; set; }
        public virtual float ReloadRemainTime { get; set; }
        public virtual int ResourceIndex { get; set; }

        public virtual int CollideCount { get; set; }

        public List<WeaponEffectData> WeaponEffectDataList { get; set; } = new List<WeaponEffectData>();
    }
}
