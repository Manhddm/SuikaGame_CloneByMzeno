using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void OnClickPlayButton()
    {
        SceneManager.LoadScene(1);
    }
}
