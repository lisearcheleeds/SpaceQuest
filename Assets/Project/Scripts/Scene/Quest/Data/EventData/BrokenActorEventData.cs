namespace AloneSpace
{
    public class BrokenActorEventData
    {
        ActorData BrokenActorData { get; }
        DamageEventData LethalDamageEventData { get; }

        public BrokenActorEventData(ActorData brokenActorData, DamageEventData lethalDamageEventData)
        {
            BrokenActorData = brokenActorData;
            LethalDamageEventData = lethalDamageEventData;
        }
    }
}
