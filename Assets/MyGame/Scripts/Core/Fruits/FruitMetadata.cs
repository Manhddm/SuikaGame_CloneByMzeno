using Development;
using UnityEngine;

namespace Core.Fruits
{
    public enum FruitTier
    {
        Tier1 = 0,
        Tier2 = 1,
        Tier3 = 2,
        Tier4 = 3,
        Tier5 = 4,
        Tier6 = 5,
        Tier7 = 6,
        Tier8 = 7,
        Tier9 = 8,
        Tier10 = 9,
        Tier11 = 10
    }
    [CreateAssetMenu(fileName = "FruitMetadata", menuName = "Fruit/FruitMetadata", order = 1)]
    public class FruitMetadata : ScriptableObject
    {
        public FruitTier tier;
        public FruitView view;

        [Header("Gameplay")]
        public int scoreOnMerge = 10;
        
        public float mass = 1;
        public float radius = 1;
    }
}
