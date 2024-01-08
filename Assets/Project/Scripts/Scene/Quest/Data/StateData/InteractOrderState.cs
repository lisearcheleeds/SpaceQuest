namespace AloneSpace
{
    public class InteractOrderState
    {
        public IInteractData InteractData { get; }
        public float Time { get; set; }
        public bool InProgress { get; set; }
        public float ProgressRatio { get; set; }
        public PullItemGraphicEffectHandler PullItemGraphicEffectHandler { get; set; }

        public InteractOrderState(IInteractData interactData)
        {
            InteractData = interactData;
        }
    }
}