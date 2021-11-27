using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public TextMeshProUGUI ScoreText;
    public GameObject VictoryText;

    public int Score { get; private set; }
    public float LastTimeScoreChanged { get; private set; }

    public int NumberBreads;

    private void Awake()
    {
        Debug.Assert(Instance == null, "There can only be one ScoreManager");
        Instance = this;

        LastTimeScoreChanged = float.MinValue;
    }

    public void AddScore(int amount)
    {
        Score += amount;
        LastTimeScoreChanged = Time.time;
        UpdateScore();

        NumberBreads -= 1;
        if (NumberBreads == 0)
        {
            VictoryText.SetActive(true);
        }
    }

    private void UpdateScore()
    {
        ScoreText.text = Score.ToString();
    }
}
