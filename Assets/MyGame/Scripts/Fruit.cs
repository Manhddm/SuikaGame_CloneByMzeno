using System;
using System.Collections;
using Core.Fruits;
using Development;
using UnityEngine;
using Random = UnityEngine.Random;

public class Fruit : FruitView
{
    public static Action OnFruitMerged;
    private static MergeSystem _mergeSystem;

    public int fruitLevel;
    public GameObject nextFruitPrefab;
    public int scoreValue = 10;
    public GameObject vfxMergePrefab;

    public float mass = 1;
    private bool hasMerged;
    public bool inBox;
    private bool canMerge;

    public static void SetMergeSystem(MergeSystem mergeSystem)
    {
        _mergeSystem = mergeSystem;
    }

    private void Awake()
    {
        if (_mergeSystem == null)
        {
            _mergeSystem = FindAnyObjectByType<MergeSystem>();
        }
    }

    private void Start()
    {
        SetTier((FruitTier)Mathf.Max(0, fruitLevel));

        var rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.mass = mass;
        }

        StartCoroutine(SpawnProcess());
    }

    public override void ApplyMetadata(FruitMetadata fruitMetadata)
    {
        base.ApplyMetadata(fruitMetadata);
        fruitLevel = (int)fruitMetadata.tier;
        mass = fruitMetadata.mass;
        scoreValue = fruitMetadata.scoreOnMerge;
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (!canMerge) return;
        if (other.gameObject.CompareTag("Fruit") || other.gameObject.CompareTag("Boder"))
        {
            inBox = true;
        }

        if (!other.gameObject.CompareTag("Fruit"))
        {
            return;
        }

        var otherFruit = other.gameObject.GetComponent<Fruit>();
        if (otherFruit == null || !otherFruit.canMerge)
        {
            return;
        }

        if (GetHashCode() > otherFruit.GetHashCode() &&
            fruitLevel == otherFruit.fruitLevel &&
            !hasMerged &&
            !otherFruit.hasMerged)
        {
            Merge(otherFruit);
        }
    }

    private void Merge(Fruit otherFruit)
    {
        if (_mergeSystem == null)
        {
            return;
        }

        hasMerged = true;
        otherFruit.hasMerged = true;
        Vector3 newPosition = (otherFruit.transform.position + transform.position) / 2f;
        bool merged = _mergeSystem.TryMergeFruit(this, otherFruit);
        if (!merged)
        {
            hasMerged = false;
            otherFruit.hasMerged = false;
            return;
        }

        AudioManager.Instance?.PlayMergeSfx();
        if (vfxMergePrefab != null)
        {
            GameObject vfx = Instantiate(vfxMergePrefab, newPosition, Quaternion.identity);
            vfx.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
            Destroy(vfx, 2f);
        }

        GameManager.Instance?.AddScore(scoreValue);
        OnFruitMerged?.Invoke();
    }

    private IEnumerator SpawnProcess()
    {
        canMerge = false;
        yield return new WaitForSeconds(0.5f);
        canMerge = true;
    }
}
