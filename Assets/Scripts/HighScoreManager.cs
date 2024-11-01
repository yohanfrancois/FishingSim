using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScoreManager : MonoBehaviour
{
    private const string highScoreKey = "HighScore";
    
    public int GetHighScore()
    {
        return PlayerPrefs.GetInt(highScoreKey, 0);
    }

    public void SaveHighScore(int score)
    {
        int currentHighScore = GetHighScore();
        if (score > currentHighScore)
        {
            PlayerPrefs.SetInt(highScoreKey, score);
            PlayerPrefs.Save();
        }
    }
}
