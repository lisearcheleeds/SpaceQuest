namespace AloneSpace
{
    public class BrokenActorEventData
    {
        public ActorData BrokenActorData { get; }
        public DamageEventData LethalDamageEventData { get; }

        public BrokenActorEventData(ActorData brokenActorData, DamageEventData lethalDamageEventData)
        {
            BrokenActorData = brokenActorData;
            LethalDamageEventData = lethalDamageEventData;
        }
    }
}
