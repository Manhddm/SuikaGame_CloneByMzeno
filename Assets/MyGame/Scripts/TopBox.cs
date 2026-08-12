using UnityEngine;

public class TopBox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Fruit" && other.gameObject.GetComponent<Fruit>().inBox)
        {
            GameManager.Instance.GameOver();
        }
        else if (other.tag == "Fruit" && !other.gameObject.GetComponent<Fruit>().inBox)
        {
            other.gameObject.GetComponent<Fruit>().inBox = true;
        }
    }
}
