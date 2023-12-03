using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AloneSpace
{
    public class AssetLoader : MonoSingleton<AssetLoader>
    {
        Dictionary<string, GameObject> gameObjectCache = new Dictionary<string, GameObject>();
        Dictionary<string, Texture2D> texture2DCache = new Dictionary<string, Texture2D>();
        List<string> loadingResources = new List<string>();

        public Coroutine StartLoadAsync<T>(AssetPath path, Action<T> onLoad) where T : Component
        {
            return StartCoroutine(InnerLoadAsync(path.Path, onLoad));
        }

        public Coroutine StartLoadAsyncCache<T>(AssetPath path, Action<T> onLoad) where T : Component
        {
            return StartLoadAsyncCache(path, asset =>
            {
                onLoad(asset.GetComponent<T>());
            });
        }

        public Coroutine StartLoadAsyncCache(AssetPath path, Action<GameObject> onLoad)
        {
            if (gameObjectCache.ContainsKey(path.Path))
            {
                onLoad(gameObjectCache[path.Path]);
                return null;
            }

            if (loadingResources.Contains(path.Path))
            {
                return StartCoroutine(WaitForLoad(
                    path.Path,
                    () =>
                    {
                        onLoad(gameObjectCache[path.Path]);
                    }));
            }

            loadingResources.Add(path.Path);
            return StartCoroutine(InnerLoadAsync<GameObject>(
                path.Path,
                loadAsset =>
                {
                    gameObjectCache[path.Path] = loadAsset;
                    loadingResources.Remove(path.Path);
                    onLoad(loadAsset);
                }));
        }

        public Coroutine StartLoadAsyncTextureCache(Texture2DPathVO path, Action<Texture2D> onLoad)
        {
            if (texture2DCache.ContainsKey(path.Path))
            {
                onLoad(texture2DCache[path.Path]);
                return null;
            }

            if (loadingResources.Contains(path.Path))
            {
                return StartCoroutine(WaitForLoad(
                    path.Path,
                    () =>
                    {
                        onLoad(texture2DCache[path.Path]);
                    }));
            }

            loadingResources.Add(path.Path);
            return StartCoroutine(InnerLoadAsync<Texture2D>(
                path.Path,
                loadAsset =>
                {
                    texture2DCache[path.Path] = loadAsset;
                    loadingResources.Remove(path.Path);
                    onLoad(loadAsset);
                }));
        }

        IEnumerator InnerLoadAsync<T>(string path, Action<T> onLoad) where T : UnityEngine.Object
        {
            var loader = Resources.LoadAsync<T>(path);
            yield return loader;
            onLoad(loader.asset as T);
        }

        IEnumerator WaitForLoad(string path, Action onComplete)
        {
            while (loadingResources.Contains(path))
            {
                yield return null;
            }

            onComplete();
        }
    }
}
