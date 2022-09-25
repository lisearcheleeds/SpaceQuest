using AloneSpace;

namespace AloneSpace
{
    public class DamageData
    {
        public ICauseDamageData CauseDamageData { get; }
        public IDamageableData DamageableData { get; }
        public int DamageValue { get; }

        public DamageData(ICauseDamageData causeDamageData, IDamageableData damageableData)
        {
            CauseDamageData = causeDamageData;
            DamageableData = damageableData;

            DamageValue = 1;
        }
    }
}