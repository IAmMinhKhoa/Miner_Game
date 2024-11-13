using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MinigameCoin : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI coinText;

	private void Awake()
	{
		UpdateUI();
	}
	private void OnEnable()
	{
		UpdateUI();
	}

	public float GetTotalScore()
	{
		return PlayerPrefs.GetFloat("TotalScoreMinigame", 0);
	}

	public void UpdateTotalScore(float value)
	{
		float currentScore = PlayerPrefs.GetFloat("TotalScoreMinigame", 0);
		PlayerPrefs.SetFloat("TotalScoreMinigame", currentScore + value);

		UpdateUI();
	}

	private void UpdateUI()
	{
		coinText.text = "" + GetComponent<MinigameCoin>().GetTotalScore();
	}
}
