using System;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    public int fruitLevel;
    public GameObject nextFruitPrefab;
    public int scoreValue = 10;

    public float mass = 1;
    private bool hasMerged = false;
    public bool inBox = false;
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Fruit") || other.gameObject.CompareTag("Boder"))
        {
            inBox = true;
        }
        if (other.gameObject.CompareTag("Fruit"))
        {
            var oFruit = other.gameObject.GetComponent<Fruit>();
            if (GetInstanceID() > oFruit.GetInstanceID() && fruitLevel == oFruit.fruitLevel )
            {
                Merge(oFruit);
            }
        }
    }

    private void Merge(Fruit otherFruit)
    {
        hasMerged = true;
        otherFruit.hasMerged = true;
        Vector3 newPosition = (otherFruit.transform.position + transform.position)/2f;
        Destroy(otherFruit.gameObject);
        Destroy(gameObject);
        if (nextFruitPrefab != null)
        {
            GameObject newFruit = Instantiate(nextFruitPrefab, newPosition, Quaternion.identity);
            //Them luc day
            newFruit.GetComponent<Rigidbody2D>().linearVelocity  = Vector2.zero;
            
            GameManager.Instance.AddScore(scoreValue);
        }
        
    }
}
