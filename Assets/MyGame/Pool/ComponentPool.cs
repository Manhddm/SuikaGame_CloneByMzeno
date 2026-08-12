using UnityEngine;

namespace MyGame.Pool
{
    public abstract class ComponentPool<T> : BaseObjectPool<T> where T : Component
    {
        private readonly T _prefab;
        private readonly Transform _poolParentInactive;
        protected ComponentPool(T prefab, bool collectionCheck = true, int defaultPoolSize = 20, int maxPoolSize = 100)
            : base(collectionCheck, defaultPoolSize, maxPoolSize)
        {
            if (prefab == null)
            {
                throw new MissingReferenceException(
                    $"{typeof(T).Name} prefab is missing."
                );
            }
            _prefab = prefab;
            _poolParentInactive = new GameObject($"{_prefab.name} Inactive Pool").transform;
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

        protected override void OnDestroy(T item)
        {
            if (item == null) return;
            Object.Destroy(item.gameObject);
        }

        protected override void OnReturn(T item)
        {
            item.gameObject.SetActive(false);
            item.transform.SetParent(_poolParentInactive, false);
        }
    }
}