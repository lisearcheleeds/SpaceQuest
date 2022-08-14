namespace RoboQuest.Quest
{
    public interface IDamageableData
    {
        bool IsBroken { get; }
        void OnDamage(DamageData damage);
    }
}