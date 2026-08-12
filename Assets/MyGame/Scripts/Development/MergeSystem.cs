using System;
using Core.Fruits;
using UnityEngine;

namespace Development
{
    public class MergeSystem : MonoBehaviour
    {
        [SerializeField] private MonoBehaviour fruitFactorySource;
        private IFruitFactory _fruitFactory;

        private void Awake()
        {
            if (_fruitFactory != null)
            {
                return;
            }

            _fruitFactory = fruitFactorySource as IFruitFactory;
        }

        public void SetFactory(IFruitFactory fruitFactory)
        {
            _fruitFactory = fruitFactory;
        }

        public bool TryMergeFruit(FruitView firstFruit, FruitView secondFruit)
        {
            if (_fruitFactory == null)
            {
                return false;
            }

            if (firstFruit == null || secondFruit == null)
            {
                return false;
            }
            if (firstFruit.IsMerging || secondFruit.IsMerging)
            {
                return false;
            }

            if (firstFruit.Tier != secondFruit.Tier)
            {
                return false;
            }

            if (!TryGetNextTier(firstFruit.Tier, out var nextTier))
            {
                return false;
            }

            var pos = (firstFruit.transform.position + secondFruit.transform.position) / 2f;

            firstFruit.Merging();
            secondFruit.Merging();
            _fruitFactory.DeSpawnFruit(firstFruit);
            _fruitFactory.DeSpawnFruit(secondFruit);
            _fruitFactory.SpawnFruit(nextTier, pos, Quaternion.identity);
            return true;
        }

        private bool TryGetNextTier(FruitTier currentTier, out FruitTier nextTier)
        {
            nextTier = currentTier + 1;

            if (!Enum.IsDefined(typeof(FruitTier), nextTier))
            {
                return false;
            }

            if (!_fruitFactory.CanSpawnTier(nextTier))
            {
                return false;
            }

            return true;
        }
    }
}
