using System.Collections.Generic;
using UnityEngine;

namespace Core.Fruits
{
    [CreateAssetMenu(menuName = "Fruits/FruitDatabase")]
    public class FruitDatabase : ScriptableObject
    {
        [SerializeField] private List<FruitMetadata> fruitMetadataList;
        private Dictionary<FruitTier, FruitMetadata> _fruitMetadataDictionary;

        public FruitMetadata GetFruitMetadata(FruitTier tier)
        {
            _fruitMetadataDictionary ??= Build();
            if (_fruitMetadataDictionary.TryGetValue(tier, out var metadata))
            {
                return metadata;
            }

            Debug.LogError($"FruitMetadata not found for tier: {tier}", this);
            return null;
        }

        public bool ContainsTier(FruitTier tier)
        {
            _fruitMetadataDictionary ??= Build();
            return _fruitMetadataDictionary.ContainsKey(tier);
        }

        private Dictionary<FruitTier, FruitMetadata> Build()
        {
            var dict = new Dictionary<FruitTier, FruitMetadata>();
            foreach (var f in fruitMetadataList)
            {
                if (f == null) continue;
                dict[f.tier] = f;
            }
            return dict;
        }
    }
}
