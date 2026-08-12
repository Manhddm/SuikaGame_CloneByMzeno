using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MyGame.Pool
{
    public abstract class ComponentPool<T> : BaseObjectPool<T> where T : Component
    {
        private readonly T _prefab;
        private readonly Transform _poolParentInactive;

        protected ComponentPool(
            T prefab,
            bool collectionCheck = true,
            int defaultPoolSize = 20,
            int maxPoolSize = 100,
            bool dontDestroyOnLoad = false)
            : base(collectionCheck, defaultPoolSize, maxPoolSize)
        {
            if (prefab == null)
            {
                throw new ArgumentNullException(nameof(prefab), $"{typeof(T).Name} prefab is missing.");
            }

            _prefab = prefab;

            var container = new GameObject($"{_prefab.name} Inactive Pool");
            if (dontDestroyOnLoad)
            {
                Object.DontDestroyOnLoad(container);
            }

            _poolParentInactive = container.transform;
        }

        protected override T Create()
        {
            return Object.Instantiate(_prefab, _poolParentInactive);
        }

        protected override void OnCreate(T item)
        {
            item.gameObject.SetActive(false);
        }

        protected override void OnRent(T item)
        {
            item.transform.SetParent(null, true);
            item.gameObject.SetActive(true);
        }

        protected override void OnReturn(T item)
        {
            item.transform.SetParent(_poolParentInactive, false);
            item.gameObject.SetActive(false);
        }

        protected override void OnDestroy(T item)
        {
            if (item == null) return;
            Object.Destroy(item.gameObject);
        }

        protected override void OnDispose()
        {
            if (_poolParentInactive != null)
            {
                Object.Destroy(_poolParentInactive.gameObject);
            }
        }
    }
}