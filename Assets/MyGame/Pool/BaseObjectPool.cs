using System;
using System.Collections.Generic;

namespace MyGame.Pool
{
    public abstract class BaseObjectPool<T> : IPool<T>
        where T : class
    {
        private const int DefaultMaxSize = 30;

        private readonly Stack<T> _items;
        private readonly Func<T> _factory;
        private readonly HashSet<T> _inactiveItems = new();
        private readonly HashSet<T> _ownedItems = new();
        private readonly int _maxInactiveSize;

        public int CountAll { get; private set; }

        public int CountInactive => _items.Count;

        public int CountActive => CountAll - CountInactive;

        public int MaxInactiveSize => _maxInactiveSize;

        protected BaseObjectPool(
            Func<T> factory,
            int maxInactiveSize = DefaultMaxSize,
            int initCapacity = 0)
        {
            _factory = factory
                ?? throw new ArgumentNullException(nameof(factory));

            if (maxInactiveSize <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(maxInactiveSize),
                    "Max size must be greater than 0."
                );
            }

            if (initCapacity < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(initCapacity),
                    "Initial capacity must be greater than or equal to 0."
                );
            }

            if (initCapacity > maxInactiveSize)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(initCapacity),
                    "Initial capacity cannot exceed max size."
                );
            }

            _maxInactiveSize = maxInactiveSize;

            _items = new Stack<T>(initCapacity);
        }

        public T Rent()
        {
            T item;

            if (_items.Count > 0)
            {
                item = _items.Pop();
                _inactiveItems.Remove(item);
            }
            else
            {
                item = Create();

                CountAll++;
            }

            OnRent(item);

            return item;
        }

        public void Return(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            if (!_ownedItems.Contains(item))
            {
                throw new InvalidOperationException(
                    "The item does not belong to this pool."
                );
            }
            if (!_inactiveItems.Add(item))
            {
                throw new InvalidOperationException(
                    "The item is already in the pool."
                );
            }
            OnReturn(item);

            if (_items.Count >= _maxInactiveSize)
            {
                _inactiveItems.Remove(item);
                CountAll--;
                OnDestroy(item);

                return;
            }

            _items.Push(item);
        }

        #region Helper Methods

        private T Create()
        {
            T item = _factory();

            if (item == null)
            {
                throw new InvalidOperationException(
                    "The pool factory returned null."
                );
            }

            if (!_ownedItems.Add(item))
            {
                throw new InvalidOperationException(
                    "The pool factory returned an item that is already owned by this pool."
                );
            }

            OnCreate(item);

            return item;
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

        #endregion
    }
}