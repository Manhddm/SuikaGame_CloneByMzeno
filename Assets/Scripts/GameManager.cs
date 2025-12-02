using System;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public TMP_Text scoreText;
    public GameObject gameOverPanel;
    private int currentScore = 0;
    private bool gameOver = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        UpdateScoreUI();
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

    public void GameOver()
    {
        gameOver = true;
        Debug.Log("Game Over");
        if (gameOverPanel) gameOverPanel.SetActive(true);
    }
}
