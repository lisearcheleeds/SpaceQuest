namespace AloneSpace
{
    public class BrokenActorObject : InteractionObject
    {
        public override IInteractData InteractData => BrokenActorInteractData;
        public override InteractionType InteractionType => InteractionType.BrokenActor;

        public BrokenActorInteractData BrokenActorInteractData { get; private set; }

        public void Apply(BrokenActorInteractData brokenActorInteractData)
        {
            BrokenActorInteractData = brokenActorInteractData;
            transform.position = brokenActorInteractData.Position;
        }

        protected override void OnRelease()
        {
            InteractData.SetPosition(transform.position);
        }
    }
}
