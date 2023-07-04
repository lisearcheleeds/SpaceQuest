﻿using System;
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

        // 現在未使用中のアセット
        Dictionary<string, List<CacheableGameObject>> unUsedAssetCache = new Dictionary<string, List<CacheableGameObject>>();

        Transform variableParent;

        public void Initialize(Transform variableParent)
        {
            this.variableParent = variableParent;

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
            // ロード済み
            if (loadCache.ContainsKey(path.Path))
            {
                GetAssetCache(path, onLoad);
                return;
            }

            // FIXME: Loadと別にする
            // まだロードしてない
            Scheduler.RunCoroutine(AssetLoader.LoadAsync(path, loadAsset =>
            {
                loadCache[path.Path] = loadAsset;
                assetCache[path.Path] = new List<CacheableGameObject>();
                unUsedAssetCache[path.Path] = new List<CacheableGameObject>();
                GetAssetCache(path, onLoad);
            }));
        }

        void ReleaseCacheAsset(CacheableGameObject usedAsset)
        {
            unUsedAssetCache[usedAsset.CacheKey].Add(usedAsset);
            usedAsset.gameObject.SetActive(false);
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
            var cache = unUsedAssetCache[path.Path].FirstOrDefault();
            if (cache == null)
            {
                cache = GameObject.Instantiate(loadCache[path.Path], variableParent, false).GetComponent<CacheableGameObject>();
                cache.SetCacheKey(path.Path);
                assetCache[path.Path].Add(cache);
            }
            else
            {
                unUsedAssetCache[path.Path].Remove(cache);
            }

            cache.gameObject.SetActive(true);
            onLoad(cache);
        }
    }
}
