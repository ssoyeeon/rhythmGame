using RhythmGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int score;
    public TMPro.TextMeshPro scoretext;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public LevelData LevelData { get; private set; }

    public float Score { get; private set; }

    public int MaxCombo { get; private set; }

    public void SetLevelData(LevelData levelData)
    {
        LevelData = levelData;
    }

    public void SendScore(float score ,int maxCombo)
    {
        Score = score;
        MaxCombo = maxCombo;
    }
}
