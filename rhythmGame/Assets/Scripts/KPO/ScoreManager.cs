using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Timing
{
    Perfect = 4,
    Great = 3,
    Good = 2,
    Bad = 1,
    Miss = 0
}
public class ScoreManager : MonoBehaviour
{
    public float HP { get; private set; }

    public float score { get; private set; }

    public int combo { get; private set; }

    public int MaxCombo { get; private set; }

    private int[] ScoreCount = new int[5];          //0번부터 Miss, Bad, Good, Great, Perfect의 개수


    private void Start()
    {
        ResetScore();
    }

    public void AddScore(Timing timing)
    {
        score += (int)timing * (1 + combo * 0.1f);
        ScoreCount[(int)timing] += 1;

        if (timing == Timing.Miss || timing == Timing.Bad)
        {
            combo = 0;
            if (timing == Timing.Miss)
            {
                HP -= (HP * 0.05f + 5);
                Debug.Log(HP);
            }
            return;
        }

        combo++;
        if (combo > MaxCombo)
        {
            MaxCombo = combo;
        }
        
    }

    public void ResetScore()
    {
        HP = 100;
        score = 0;
        MaxCombo = 0;
        combo = 0;
        ScoreCount = new int[5];
    }

    public int[] CheckCount()
    {
        return ScoreCount;
    }

    public void SendScore()
    {
        GameManager.Instance.SendScore(score, combo, ScoreCount);
    }

}
