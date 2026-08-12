using Core.Fruits;
using UnityEngine;

namespace Development
{
    public interface IFruitFactory
    {
        bool CanSpawnTier(FruitTier tier);
        FruitView SpawnFruit(FruitTier tier, Vector3 position, Quaternion rotation);
        void DeSpawnFruit(FruitView fruitView);
    }
}
