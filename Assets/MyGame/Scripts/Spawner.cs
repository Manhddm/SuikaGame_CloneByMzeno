using System.Collections;
using System.Collections.Generic;
using Core.Fruits;
using Development;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Next Fruit Settings")]
    [SerializeField] private List<GameObject> fruits;
    [SerializeField] private Transform nextFruitBubble;
    [SerializeField] private Transform spawnNextFruitPoint;

    [Header("Player Hand")]
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform fruitsContainer;
    [SerializeField] private FruitFactory fruitFactory;
    [SerializeField] private MergeSystem mergeSystem;

    private readonly List<FruitTier> _spawnableTiers = new();
    private FruitView _currentFruit;
    private FruitView _nextFruit;
    private bool isSpawning;

    private void Awake()
    {
        EnsureSystems();
        fruitFactory.RegisterLegacyPrefabs(fruits);
        BuildSpawnableTiers();
    }

    private void Start()
    {
        SpawnNextInBubble();
        MoveToHand();
        SpawnNextInBubble();
    }

    private void EnsureSystems()
    {
        if (fruitFactory == null)
        {
            fruitFactory = FindAnyObjectByType<FruitFactory>();
        }

        if (fruitFactory == null)
        {
            fruitFactory = gameObject.AddComponent<FruitFactory>();
        }

        if (mergeSystem == null)
        {
            mergeSystem = FindAnyObjectByType<MergeSystem>();
        }

        if (mergeSystem == null)
        {
            mergeSystem = gameObject.AddComponent<MergeSystem>();
        }

        mergeSystem.SetFactory(fruitFactory);
        Fruit.SetMergeSystem(mergeSystem);
    }

    private void BuildSpawnableTiers()
    {
        _spawnableTiers.Clear();
        if (fruits == null)
        {
            return;
        }

        foreach (var prefab in fruits)
        {
            if (prefab == null)
            {
                continue;
            }

            if (!prefab.TryGetComponent<Fruit>(out var fruit))
            {
                continue;
            }

            var tier = (FruitTier)Mathf.Max(0, fruit.fruitLevel);
            _spawnableTiers.Add(tier);
        }
    }

    private void SpawnNextInBubble()
    {
        if (_spawnableTiers.Count == 0)
        {
            Debug.LogError("No spawnable fruit tiers configured on Spawner.", this);
            return;
        }

        var tier = RandomTier();
        _nextFruit = fruitFactory.SpawnFruit(tier, spawnNextFruitPoint.position, spawnNextFruitPoint.rotation);
        if (_nextFruit == null)
        {
            return;
        }

        SetFruitSimulated(_nextFruit, false);
        _nextFruit.transform.SetParent(nextFruitBubble);
    }

    private FruitTier RandomTier()
    {
        int number = Random.Range(1, 101);
        if (_spawnableTiers.Count >= 4)
        {
            if (number <= 40) return _spawnableTiers[0];
            if (number <= 70) return _spawnableTiers[1];
            if (number <= 90) return _spawnableTiers[2];
            return _spawnableTiers[3];
        }

        int index = Random.Range(0, _spawnableTiers.Count);
        return _spawnableTiers[index];
    }

    private void MoveToHand()
    {
        if (_nextFruit == null) return;
        _currentFruit = _nextFruit;
        _nextFruit = null;
        _currentFruit.transform.position = spawnPoint.position;
        _currentFruit.transform.SetParent(spawnPoint);
    }

    private IEnumerator DropFruit()
    {
        if (_currentFruit == null)
        {
            yield break;
        }

        isSpawning = true;
        _currentFruit.transform.SetParent(fruitsContainer);
        SetFruitSimulated(_currentFruit, true);
        AudioManager.Instance?.PlayDropSfx();
        _currentFruit = null;
        yield return new WaitForSeconds(0.5f);

        MoveToHand();
        SpawnNextInBubble();
        isSpawning = false;
    }

    public void DropCurrentFruit()
    {
        if (isSpawning) return;
        StartCoroutine(DropFruit());
    }

    private static void SetFruitSimulated(FruitView fruitView, bool simulated)
    {
        if (fruitView == null)
        {
            return;
        }

        var rb = fruitView.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.simulated = simulated;
        }
    }
}
