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
            GameObjectCache.Instance.ReleaseCache(this);
            OnRelease();
        }

        protected virtual void OnRelease()
        {
        }
    }
}