using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public float score { get; private set; }

    public int combo { get; private set; }

    public int MaxCombo { get; private set; }


    private void Start()
    {
        ResetScore();
    }

    public void AddScore(float point)
    {
        combo++;
        if (combo > MaxCombo)
        {
            MaxCombo = combo;
        }
        score += point * (1 + combo * 0.1f);
    }

    public void ResetScore()
    {
        score = 0;
        MaxCombo = 0;
        combo = 0;
    }

    public void ResetCombo()
    {
        combo = 0;
    }
}
