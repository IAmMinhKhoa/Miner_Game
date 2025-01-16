using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SortGameScore : MonoBehaviour
{
    public TextMeshProUGUI highScoreText, currentScoreText, endScoreText;
	public float currentScore;

	private void Start()
	{
		highScoreText.text = "High Score: " + PlayerPrefs.GetFloat("minigameSortHighScore", 0);
	}

	public void CheckSetHighScore()
	{
		float lastHighScore = PlayerPrefs.GetFloat("minigameSortHighScore", 0);
		if(currentScore > lastHighScore)
		{
			endScoreText.text = "New High Score: " + currentScore;
			PlayerPrefs.SetFloat("minigameSortHighScore", currentScore);
			highScoreText.text = "High Score: " + PlayerPrefs.GetFloat("minigameSortHighScore", 0);
		}
		else
		{
			endScoreText.text = "Your Score: " + currentScore;
		}
		ScoreTracking.Instance.TrackEvent(TrackingEventType.SortGameComplete, currentScore);
		UpdateTotalScore(currentScore);
		ResetScore();
	}

	public void UpdateCurrentScore(float add)
	{
		currentScore += add;
		currentScoreText.text = "Current score: " + currentScore;
		if(currentScore % 500 == 0)
		{
			FindObjectOfType<SortGameManager>().AdjustClawDelayTime(0.8f);
		}
	}

	public void ResetScore()
	{
		currentScore = 0;
		currentScoreText.text = "Current score: " + currentScore;
	}

	public void UpdateTotalScore(float value)
	{
		PlayfabMinigame.Instance.GrantVirtualCurrency((int)value);
		PlayfabMinigame.Instance.GetVirtualCurrencies();
	}
}
