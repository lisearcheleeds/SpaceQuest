using UnityEngine;

namespace RoboQuest.Quest.InSide
{
    public abstract class CacheableGameObject : MonoBehaviour
    {
        public bool IsActive
        {
            get => gameObject.activeSelf;
            set => gameObject.SetActive(value);
        }

        public void Release()
        {
            GameObjectCache.Instance.ReleaseCache(this);
            OnRelease();
        }

        public void SetParent(Transform parent)
        {
            transform.SetParent(parent, false);
        }

        protected abstract void OnRelease();
    }
}