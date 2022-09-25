using AloneSpace;

namespace AloneSpace
{
    public interface IThreatData
    {
        WeaponData WeaponData { get; }
        ITargetData TargetData { get; }
        
        CollisionShape HitCollidePrediction { get; }
    }
}