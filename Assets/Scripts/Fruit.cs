using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Fruit : MonoBehaviour
{
    public int fruitLevel;
    public GameObject nextFruitPrefab;
    public int scoreValue = 10;
    public GameObject vfxMergePrefab;

    public float mass = 1;
    private bool hasMerged = false;
    public bool inBox = false;
    private bool canMerge;
    
    void Start()
    {
        GetComponent<Rigidbody2D>().mass = mass;
        StartCoroutine(SpawnProcess());
    }
    private void OnCollisionStay2D(Collision2D other)
    {
        if (!canMerge) return;
        if (other.gameObject.CompareTag("Fruit") || other.gameObject.CompareTag("Boder"))
        {
            inBox = true;
        }
        if (other.gameObject.CompareTag("Fruit"))
        {
            var oFruit = other.gameObject.GetComponent<Fruit>();
            if (oFruit != null && oFruit.canMerge)
            {
                if (GetInstanceID() > oFruit.GetInstanceID() && fruitLevel == oFruit.fruitLevel
                                                             && !hasMerged && !oFruit.hasMerged)
                {
                    Merge(oFruit);
                }
            }
        }
    }

    private void Merge(Fruit otherFruit)
    {
        hasMerged = true;
        otherFruit.hasMerged = true;
        Vector3 newPosition = (otherFruit.transform.position + transform.position)/2f;
        AudioManager.Instance.PlayMergeSfx();
        if (vfxMergePrefab != null)
        {
            GameObject vfx = Instantiate(vfxMergePrefab, newPosition, Quaternion.identity);
            vfx.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
            Destroy(vfx, 2f);
        }
        Destroy(otherFruit.gameObject);
        Destroy(gameObject);
        if (nextFruitPrefab != null)
        {
            GameObject newFruit = Instantiate(nextFruitPrefab, newPosition, Quaternion.identity);
            //Them luc day
            newFruit.GetComponent<Rigidbody2D>().linearVelocity  = Vector2.zero;
            GameManager.Instance.AddScore(scoreValue);
            GameManager.Instance.TriggerMerge();
        }
        
    }

    IEnumerator SpawnProcess()
    {
        canMerge = false;
        yield return new WaitForSeconds(0.5f);
        canMerge = true;
    }
}
