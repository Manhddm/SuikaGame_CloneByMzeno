using UnityEngine;

public class TopBox : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Fruit" && other.gameObject.GetComponent<Fruit>().inBox)
        {
            GameManager.Instance.GameOver();
        }
    }
}
