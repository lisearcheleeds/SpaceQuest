namespace AloneSpace
{
    public abstract class GraphicEffect : CacheableGameObject
    {
        public bool IsCompleted { get; protected set; }

        protected GraphicEffectSpecVO GraphicEffectSpecVO { get; private set; }
        protected IGraphicEffectHandler GraphicEffectHandler { get; private set; }

        public void Init(GraphicEffectSpecVO graphicEffectSpecVO, IGraphicEffectHandler graphicEffectHandler)
        {
            IsCompleted = false;

            GraphicEffectSpecVO = graphicEffectSpecVO;
            GraphicEffectHandler = graphicEffectHandler;

            OnInit();
        }

        protected abstract void OnInit();

        public virtual void OnLateUpdate(float deltaTime)
        {
        }
    }
}
