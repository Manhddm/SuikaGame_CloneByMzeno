using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public TMP_Text scoreText;
    public GameObject gameOverPanel;
    public CanvasGroup gameOverPanelCanvasGroup;
    public float fadeDuration = 1f;
    public event Action OnMergeEvent;
    public event Action GameOverEvent;
    private int currentScore = 0;
    public bool gameOver = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        UpdateScoreUI();
        if (gameOverPanelCanvasGroup)
        {
            gameOverPanelCanvasGroup.alpha = 0;
        }
    }
    public void AddScore(int score)
    {
        if (gameOver) return;
        currentScore += score;
        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        scoreText.text = currentScore.ToString();
    }

    public void TriggerMerge()
    {
        OnMergeEvent?.Invoke();
    }
    
    public void GameOver()
    {
        gameOver = true;
        GameOverEvent?.Invoke();

        if (gameOverPanel)
        {
            gameOverPanel.SetActive(true);
            StartCoroutine(FadeInGameOverPanel());
        }
    }
    
    IEnumerator FadeInGameOverPanel()
    {
        float elapsed = 0f;
        AudioManager.Instance.PlayGameOverSfx();
        while (elapsed < fadeDuration)
        {
            AudioManager.Instance.MuteMusic(true);
            elapsed += Time.deltaTime;
            gameOverPanelCanvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsed / fadeDuration);
            yield return null;
        }
        AudioManager.Instance.MuteMusic(false);
        gameOverPanelCanvasGroup.alpha = 1f;
    }

    public void OnClickBack()
    {
        SceneManager.LoadScene(0);
    }
}
