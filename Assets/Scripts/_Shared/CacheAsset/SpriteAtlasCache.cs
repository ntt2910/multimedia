using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using Object = UnityEngine.Object;

namespace BW.Assets
{
    public class SpriteAtlasCache : IAssetCache
    {
        private readonly SpriteAtlas _atlas;
        private readonly Dictionary<string, object> _cache;
        private readonly List<string> _tempCacheIds;

        public SpriteAtlasCache(SpriteAtlas atlas)
        {
            this._atlas = atlas;
            this._cache = new Dictionary<string, object>();
            this._tempCacheIds = new List<string>(30);
        }

        public void CacheAsset<T>(string id, T asset, bool permanent = false)
        {
            if (this._cache.ContainsKey(id))
            {
                Debug.LogError("Dupplicate caching id " + id);
                return;
            }

            this._cache.Add(id, asset);

            if (!permanent)
            {
                this._tempCacheIds.Add(id);
            }
        }

        public void CacheAssetAsync<T>(string id, Action<T> setter, bool permanent = false) where T : Object
        {
        }

        public bool TryGetCache<T>(string id, out T asset)
        {
            asset = default(T);

            if (!this._cache.ContainsKey(id))
            {
                Debug.LogError("Dupplicate caching id " + id);
                return false;
            }

            var cachedAsset = this._cache[id];

            if (cachedAsset.GetType() != typeof(T))
            {
                Debug.LogError("Wrong type of cached asset id " + id);
                return false;
            }

            asset = (T) this._cache[id];
            return true;
        }

        public T GetCache<T>(string id)
        {
            var asset = default(T);

            if (!this._cache.ContainsKey(id))
            {
                Debug.LogError("Dupplicate caching id " + id);
                return asset;
            }

            var cachedAsset = this._cache[id];

            if (cachedAsset.GetType() != typeof(T))
            {
                Debug.LogError("Wrong type of cached asset id " + id);
                return asset;
            }

            asset = (T) this._cache[id];
            return asset;
        }

        public bool HasCache(string id)
        {
            return this._cache.ContainsKey(id);
        }

        public void Clear()
        {
            foreach (var id in this._tempCacheIds)
            {
                this._cache.Remove(id);
            }

            this._tempCacheIds.Clear();
        }
    }
}