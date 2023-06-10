using UnityEngine;

namespace AloneSpace
{
    public abstract class CacheableGameObject : MonoBehaviour
    {
        public bool IsUse
        {
            get => gameObject.activeSelf;
            set => gameObject.SetActive(value);
        }

        public void Release()
        {
            MessageBus.Instance.ReleaseCacheAsset.Broadcast(this);
            OnRelease();
        }

        protected virtual void OnRelease()
        {
        }
    }
}