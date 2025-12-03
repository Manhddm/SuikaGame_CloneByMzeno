using System.Collections;
using System.Collections.Generic;
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
    
    private GameObject currentFruitPrefab;
    private GameObject nextFruitPrefab;
    private bool isSpawning;
    
    void Start()
    {
        SpawnNextInBubble();
        MoveToHand();
        SpawnNextInBubble();
    }

    void Update()
    {
        if (!isSpawning && currentFruitPrefab != null && Input.GetMouseButtonDown(0))
        {
            StartCoroutine(DropFruit());
        }
    }

    void SpawnNextInBubble()
    {
        int index = RandomIndex();
        GameObject fruitPrefab = fruits[index];
        
        nextFruitPrefab = Instantiate(fruitPrefab, spawnNextFruitPoint.position, spawnNextFruitPoint.rotation);
        nextFruitPrefab.GetComponent<Rigidbody2D>().simulated = false;
        nextFruitPrefab.transform.SetParent(nextFruitBubble);
    }



    int RandomIndex()
    {
        int number  = Random.Range(1, 101);
        if (number <= 40) return 0;
        if (number <= 70) return 1;
        if (number <= 90) return 2;
        return 3;
    }
    void MoveToHand()
    {
        if (nextFruitPrefab == null) return;
        currentFruitPrefab = nextFruitPrefab;
        nextFruitPrefab = null;
        currentFruitPrefab.transform.position = spawnPoint.position;
        currentFruitPrefab.transform.SetParent(spawnPoint);
    }

    IEnumerator DropFruit()
    {
        isSpawning = true;
        currentFruitPrefab.transform.SetParent(fruitsContainer);
        currentFruitPrefab.GetComponent<Rigidbody2D>().simulated = true;
        currentFruitPrefab = null;
        yield return new WaitForSeconds(0.7f);
        
        MoveToHand();
        SpawnNextInBubble();
        isSpawning = false;
    }
    
}
