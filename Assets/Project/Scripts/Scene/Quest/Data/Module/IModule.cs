namespace AloneSpace
{
    public interface IModule
    {
        // public void ActivateModule();
        // public void DeactivateModule();
        public void OnUpdateModule(float deltaTime);
    }
}