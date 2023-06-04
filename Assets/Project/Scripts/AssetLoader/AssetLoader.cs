﻿using System;
using System.Collections;
using UnityEngine;

namespace AloneSpace
{
    public static class AssetLoader
    {
        public static IEnumerator LoadAsync(AssetPath path, Action<GameObject> onLoad)
        {
            yield return LoadAsync<GameObject>(path, onLoad);
        }

        public static IEnumerator LoadAsync<T>(AssetPath path, Action<T> onLoad) where T : UnityEngine.Object
        {
            var loader = Resources.LoadAsync<T>(path.Path);
            yield return loader;
            onLoad(loader.asset as T);
        }
    }
}
