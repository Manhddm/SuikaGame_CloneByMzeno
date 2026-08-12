using System;
using System.Collections.Generic;
using UnityEngine.Pool;

namespace MyGame.Pool
{
    public abstract class BaseObjectPool<T> : IPool<T>
        where T : class
    {
        private readonly ObjectPool<T> _pool;
        public int CountAll => _pool.CountAll;
        public int CountActive => _pool.CountActive;
        public int CountInactive => _pool.CountInactive;

        protected BaseObjectPool(bool collectionCheck = true, int defaultPoolSize = 20, int maxPoolSize = 100)
        {
            _pool = new ObjectPool<T>(
                createFunc: CreateInternal,
                actionOnGet: OnRent,
                actionOnRelease: OnReturn,
                actionOnDestroy: OnDestroy,
                collectionCheck: collectionCheck,
                defaultCapacity: defaultPoolSize,
                maxSize: maxPoolSize
            );
        }

        protected abstract T Create();

        public T Rent()
        {
            return _pool.Get();
        }

        public void Return(T item)
        {
            _pool.Release(item);
        }

        public void Clear()
        {
            _pool.Clear();
        }

        protected virtual void OnCreate(T item)
        {
        }

        protected virtual void OnRent(T item)
        {
        }

        protected virtual void OnReturn(T item)
        {
        }

        protected virtual void OnDestroy(T item)
        {
        }

        private T CreateInternal()
        {
            T item = Create();

            if (item == null)
            {
                throw new InvalidOperationException(
                    $"{GetType().Name}.Create() returned null."
                );
            }

            OnCreate(item);

            return item;
        }
    }
}