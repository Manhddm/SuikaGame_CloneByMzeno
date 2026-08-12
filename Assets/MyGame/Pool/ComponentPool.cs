using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MyGame.Pool
{
    public abstract class ComponentPool<T> : BaseObjectPool<T>
        where T : Component
    {
        private readonly T _prefab;
        private readonly Transform _poolParentInactive;

        protected ComponentPool(
            T prefab,
            bool collectionCheck = true,
            int defaultPoolSize = 20,
            int maxPoolSize = 100,
            bool dontDestroyOnLoad = false)
            : base(
                collectionCheck,
                defaultPoolSize,
                maxPoolSize)
        {
            if (prefab == null)
            {
                throw new ArgumentNullException(
                    nameof(prefab),
                    $"{typeof(T).Name} prefab is missing."
                );
            }

            _prefab = prefab;

            var container =
                new GameObject($"{_prefab.name} Inactive Pool");

            if (dontDestroyOnLoad)
            {
                Object.DontDestroyOnLoad(container);
            }

            _poolParentInactive = container.transform;
        }

        protected override T Create()
        {
            T item = Object.Instantiate(
                _prefab,
                _poolParentInactive
            );

            return item;
        }

        protected override void OnCreate(T item)
        {
            item.gameObject.SetActive(false);
            OnCreateComponent(item);
        }

        protected override void OnRent(T item)
        {
            item.transform.SetParent(null, true);
            item.gameObject.SetActive(true);

            OnRentComponent(item);
        }

        protected override void OnReturn(T item)
        {
            OnReturnComponent(item);

            item.gameObject.SetActive(false);
            item.transform.SetParent(
                _poolParentInactive,
                false
            );
        }

        protected override void OnDestroy(T item)
        {
            OnDestroyComponent(item);

            if (item != null)
            {
                Object.Destroy(item.gameObject);
            }
        }

        protected override void OnDispose()
        {
            OnDisposeComponentPool();

            if (_poolParentInactive != null)
            {
                Object.Destroy(
                    _poolParentInactive.gameObject
                );
            }
        }

        protected virtual void OnCreateComponent(T item)
        {
        }

        protected virtual void OnRentComponent(T item)
        {
        }

        protected virtual void OnReturnComponent(T item)
        {
        }

        protected virtual void OnDestroyComponent(T item)
        {
        }

        protected virtual void OnDisposeComponentPool()
        {
        }
    }
}