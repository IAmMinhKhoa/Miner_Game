using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class PlayerScore
{

    public static float highScore;

    public static void SetHighScore(float newScore)
    {
        highScore = newScore;
        PlayerPrefs.SetFloat("FruitHighScore", highScore);
        PlayerPrefs.Save();
    }
    public static float GetHighScore()
    {
        highScore = PlayerPrefs.GetFloat("FruitHighScore", 0);
        return highScore;
    }
}
