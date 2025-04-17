using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public IntVariable scoreValue;
    public TextMeshProUGUI scoreText;

    private int score;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("More than one GameManager found. Destroying the new one.");
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        score = 0;
        scoreValue.value = score;
        UpdateScoreText();
    }

    public void AddScore(int value)
    {
        score += value;
        scoreValue.value = score;
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Eaten: " + score;
        }
    }

    public void ResetScore()
    {
        score = 0;
        scoreValue.value = score;
        UpdateScoreText();
    }
}
