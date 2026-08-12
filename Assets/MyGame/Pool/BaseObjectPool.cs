using System;
using UnityEngine.Pool;

namespace MyGame.Pool
{
    public abstract class BaseObjectPool<T> : IPool<T>, IDisposable
        where T : class
    {
        private readonly ObjectPool<T> _pool;
        private bool _disposed;

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
            ThrowIfDisposed();
            return _pool.Get();
        }

        public void Return(T item)
        {
            ThrowIfDisposed();
            _pool.Release(item);
        }

        public void Clear()
        {
            _pool.Clear();
        }

        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;

            try
            {
                _pool.Dispose();
            }
            finally
            {
                OnDispose();
            }
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
        
        protected virtual void OnDispose()
        {
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().Name);
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