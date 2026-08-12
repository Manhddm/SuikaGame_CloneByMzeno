using System.Collections.Generic;
using Core.Fruits;
using UnityEngine;

namespace Development
{
    public class FruitFactory : MonoBehaviour, IFruitFactory
    {
        [SerializeField] private FruitDatabase fruitDatabase;
        [SerializeField] private List<FruitView> fallbackFruitPrefabs;
        private GameObject _container;
        private Dictionary<FruitTier, FruitMetadata> _runtimeMetadata;

        public void Awake()
        {
            CreateContainer();
            BuildRuntimeMetadataFromFallbacks();
        }

        public FruitView SpawnFruit(FruitTier tier, Vector3 position, Quaternion rotation)
        {
            if (!TryGetMetadata(tier, out var data))
            {
                Debug.LogError($"Cannot find metadata for tier {tier}.", this);
                return null;
            }

            if (data == null || data.view == null)
            {
                Debug.LogError($"Cannot spawn fruit for tier {tier}. Metadata or view is missing.", this);
                return null;
            }

            var instance = Instantiate(data.view, position, rotation, _container.transform);
            instance.ApplyMetadata(data);
            return instance;
        }

        public bool CanSpawnTier(FruitTier tier)
        {
            if (TryGetMetadata(tier, out var metadata))
            {
                return metadata != null && metadata.view != null;
            }

            return false;
        }

        public void DeSpawnFruit(FruitView fruitView)
        {
            if (fruitView == null)
            {
                return;
            }
            Destroy(fruitView.gameObject);
        }

        private void CreateContainer()
        {
            if (_container != null)
            {
                return;
            }

            _container = new GameObject("FruitContainer");
            _container.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        }

        public void RegisterLegacyPrefabs(IReadOnlyList<GameObject> prefabs)
        {
            if (prefabs == null)
            {
                return;
            }

            if (fallbackFruitPrefabs == null)
            {
                fallbackFruitPrefabs = new List<FruitView>();
            }

            fallbackFruitPrefabs.Clear();
            foreach (var prefab in prefabs)
            {
                if (prefab == null)
                {
                    continue;
                }

                if (prefab.TryGetComponent<FruitView>(out var fruitView))
                {
                    fallbackFruitPrefabs.Add(fruitView);
                }
            }

            BuildRuntimeMetadataFromFallbacks();
        }

        private bool TryGetMetadata(FruitTier tier, out FruitMetadata metadata)
        {
            metadata = null;

            if (fruitDatabase != null)
            {
                metadata = fruitDatabase.GetFruitMetadata(tier);
                if (metadata != null)
                {
                    return true;
                }
            }

            _runtimeMetadata ??= new Dictionary<FruitTier, FruitMetadata>();
            return _runtimeMetadata.TryGetValue(tier, out metadata);
        }

        private void BuildRuntimeMetadataFromFallbacks()
        {
            _runtimeMetadata = new Dictionary<FruitTier, FruitMetadata>();
            if (fallbackFruitPrefabs == null)
            {
                return;
            }

            foreach (var prefabView in fallbackFruitPrefabs)
            {
                if (prefabView == null)
                {
                    continue;
                }

                var rb = prefabView.GetComponent<Rigidbody2D>();
                var col = prefabView.GetComponent<CircleCollider2D>();
                var tier = GetTierFromPrefab(prefabView);

                var runtimeMetadata = ScriptableObject.CreateInstance<FruitMetadata>();
                runtimeMetadata.tier = tier;
                runtimeMetadata.view = prefabView;
                runtimeMetadata.mass = rb != null ? rb.mass : 1f;
                runtimeMetadata.radius = col != null ? col.radius : 0.5f;
                if (prefabView is Fruit fruit)
                {
                    runtimeMetadata.scoreOnMerge = fruit.scoreValue;
                }

                _runtimeMetadata[tier] = runtimeMetadata;
            }
        }

        private FruitTier GetTierFromPrefab(FruitView fruitView)
        {
            if (fruitView is Fruit fruit)
            {
                return (FruitTier)Mathf.Max(0, fruit.fruitLevel);
            }

            return FruitTier.Tier1;
        }
    }
}
