using System;
using System.Collections.Generic;
using System.Linq;
using AloneSpace;
using UnityEngine;

namespace AloneSpace
{
    public class GameObjectCache : MonoSingleton<GameObjectCache>
    {
        // ロード済みのアセット
        Dictionary<string, GameObject> loadCache = new Dictionary<string, GameObject>();

        // Instanciate済みのアセット
        Dictionary<string, List<CacheableGameObject>> assetCache = new Dictionary<string, List<CacheableGameObject>>();

        Transform cacheRoot;

        public void GetAsset<T>(CacheableGameObjectPath path, Action<T> onLoad) where T : CacheableGameObject
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

        public void AllRelease()
        {
            foreach (var assets in assetCache)
            {
                foreach (var asset in assets.Value)
                {
                    asset.Release();
                }
            }
        }

        public void ReleaseCache(CacheableGameObject usedAsset)
        {
            usedAsset.IsUse = false;
            usedAsset.transform.SetParent(cacheRoot, false);
        }

        protected override void OnInitialize()
        {
            cacheRoot = new GameObject("CacheRoot").transform;
            cacheRoot.SetParent(transform, false);
        }

        void GetAssetCache<T>(CacheableGameObjectPath path, Action<T> onLoad) where T : CacheableGameObject
        {
            if (!assetCache.ContainsKey(path.Path))
            {
                assetCache[path.Path] = new List<CacheableGameObject>();
            }

            var cache = (T)assetCache[path.Path].FirstOrDefault(target => !target.IsUse);

            if (cache == null)
            {
                cache = Instantiate(loadCache[path.Path], cacheRoot, false).GetComponent<T>();
                assetCache[path.Path].Add(cache);
            }

            cache.IsUse = true;
            onLoad(cache);
        }
    }
}