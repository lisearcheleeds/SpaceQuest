using RoboQuest;

namespace AloneSpace.InSide
{
    public interface ICauseDamage
    {
        WeaponData WeaponData { get; }
        ItemVO ResourceItemVO { get; }
    }
}
