using System;
using System.Collections.Generic;
using System.Linq;
using AloneSpace;
using UnityEngine;

namespace AloneSpace
{
    public class GameObjectCache
    {
        // ロード済みのアセット
        Dictionary<string, GameObject> loadCache = new Dictionary<string, GameObject>();

        // Instanciate済みのアセット
        Dictionary<string, List<CacheableGameObject>> assetCache = new Dictionary<string, List<CacheableGameObject>>();

        Transform variableParent;
        Transform cacheParent;

        public void Initialize(Transform variableParent, Transform cacheParent)
        {
            this.variableParent = variableParent;
            this.cacheParent = cacheParent;

            MessageBus.Instance.GetCacheAsset.AddListener(GetCacheAsset);
            MessageBus.Instance.ReleaseCacheAsset.AddListener(ReleaseCacheAsset);
            MessageBus.Instance.ReleaseCacheAssetAll.AddListener(ReleaseCacheAssetAll);
        }

        public void Finalize()
        {
            MessageBus.Instance.GetCacheAsset.RemoveListener(GetCacheAsset);
            MessageBus.Instance.ReleaseCacheAsset.RemoveListener(ReleaseCacheAsset);
            MessageBus.Instance.ReleaseCacheAssetAll.RemoveListener(ReleaseCacheAssetAll);
        }

        void GetCacheAsset(CacheableGameObjectPath path, Action<CacheableGameObject> onLoad)
        {
            if (loadCache.ContainsKey(path.Path))
            {
                GetAssetCache(path, onLoad);
                return;
            }

            // FIXME: Loadと別にする
            Scheduler.RunCoroutine(AssetLoader.LoadAsync(path, loadAsset =>
            {
                loadCache[path.Path] = loadAsset;
                GetAssetCache(path, onLoad);
            }));
        }

        void ReleaseCacheAsset(CacheableGameObject usedAsset)
        {
            usedAsset.IsUse = false;
            usedAsset.transform.SetParent(cacheParent, false);
        }

        void ReleaseCacheAssetAll()
        {
            foreach (var assets in assetCache)
            {
                foreach (var asset in assets.Value)
                {
                    asset.Release();
                }
            }
        }

        void GetAssetCache(CacheableGameObjectPath path, Action<CacheableGameObject> onLoad)
        {
            if (!assetCache.ContainsKey(path.Path))
            {
                assetCache[path.Path] = new List<CacheableGameObject>();
            }

            var cache = assetCache[path.Path].FirstOrDefault(target => !target.IsUse);

            if (cache == null)
            {
                cache = GameObject.Instantiate(loadCache[path.Path], cacheParent, false).GetComponent<CacheableGameObject>();
                assetCache[path.Path].Add(cache);
            }

            cache.IsUse = true;
            cache.transform.SetParent(variableParent, false);
            onLoad(cache);
        }
    }
}