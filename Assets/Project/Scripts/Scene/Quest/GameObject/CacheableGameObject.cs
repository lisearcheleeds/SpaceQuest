using UnityEngine;

namespace AloneSpace
{
    public abstract class CacheableGameObject : MonoBehaviour
    {
        public string CacheKey { get; private set; }

        public void SetCacheKey(string cacheKey)
        {
            CacheKey = cacheKey;
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
