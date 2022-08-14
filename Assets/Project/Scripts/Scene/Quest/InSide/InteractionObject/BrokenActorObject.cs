namespace RoboQuest.Quest.InSide
{
    public class BrokenActorObject : InteractionObject
    {
        public override IInteractData InteractData => BrokenActorInteractData;
        public override InteractionType InteractionType => InteractionType.BrokenActor;

        public BrokenActorInteractData BrokenActorInteractData { get; private set; }

        public void Apply(BrokenActorInteractData brokenActorInteractData)
        {
            BrokenActorInteractData = brokenActorInteractData;

            transform.position = brokenActorInteractData.Position + GetPlaceOffsetHeight();
            MessageBus.Instance.SendInteractionObject.Broadcast(this, true);
        }

        protected override void OnRelease()
        {
            InteractData.Position = transform.position;
            MessageBus.Instance.SendInteractionObject.Broadcast(this, false);
        }
    }
}
