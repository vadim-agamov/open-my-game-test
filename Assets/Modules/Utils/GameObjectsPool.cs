using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

namespace Modules.Utils
{
    public interface IPooledGameObject
    {
        string Id { get; }
        GameObject GameObject { get; }
        void Release();
    }

    public sealed class GameObjectsPool
    {
        private readonly Dictionary<string, IObjectPool<IPooledGameObject>> _pools = new ();
        private Transform _gameObjectsHolder;
        private static readonly Lazy<GameObjectsPool> _instance = new Lazy<GameObjectsPool>(CreatePool);
        
        public static GameObjectsPool Instance => _instance.Value;

        private static GameObjectsPool CreatePool()
        {
            var holder = new GameObject("[POOL]").transform;
            Object.DontDestroyOnLoad(holder);
            return new GameObjectsPool
            {
                _gameObjectsHolder =  holder.transform
            };
        }
        
        public static void ReleaseAll()
        {
            foreach (var (_, pool) in Instance._pools)
            {
                pool.Clear();
            }
        }
        
        public static T Get<T>(T prefab) where T : MonoBehaviour, IPooledGameObject => Get(prefab, null, null);
        public static T Get<T>(T prefab, Transform parent) where T : MonoBehaviour, IPooledGameObject => Get(prefab, parent, null);
        public static T Get<T>(T prefab, Transform parent, Func<T,T> customCreate) where T : MonoBehaviour, IPooledGameObject
        {
            if (!Instance._pools.TryGetValue(prefab.Id, out var pool))
            {
                pool = new ObjectPool<IPooledGameObject>(
                    createFunc: customCreate != null ? () => customCreate.Invoke(prefab) : () => CreateView(prefab),
                    actionOnGet: OnGet,
                    actionOnRelease: OnRelease);
                Instance._pools[prefab.Id] = pool;
            }

            var component = (T) pool.Get();
            if(parent != null)
            {
                component.transform.SetParent(parent, false);
            }
            
            return component;
        }
        
        public void Release<T>(T view) where T : IPooledGameObject
        {
            if (!_pools.TryGetValue(view.Id, out var pool))
            {
                throw new InvalidOperationException($"Pool for {view.GameObject.name}|{view.Id} not found");
            }
            
            pool.Release(view);
        }

        private static T CreateView<T>(T prefab) where T : MonoBehaviour, IPooledGameObject => Object.Instantiate(prefab);
        private static void OnGet<T>(T view) where T : IPooledGameObject => view.GameObject.SetActive(true);
        private static void OnRelease<T>(T view) where T : IPooledGameObject
        {
            view.Release();
            view.GameObject.transform.SetParent(Instance._gameObjectsHolder, false);
            view.GameObject.SetActive(false);
        }
    }
}
