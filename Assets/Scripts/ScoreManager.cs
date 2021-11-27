using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public int Score { get; private set; }

    private void Awake()
    {
        Debug.Assert(Instance == null, "There can only be one ScoreManager");
        Instance = this;
    }

    public void AddScore(int amount)
    {
        Score += amount;
    }
}
