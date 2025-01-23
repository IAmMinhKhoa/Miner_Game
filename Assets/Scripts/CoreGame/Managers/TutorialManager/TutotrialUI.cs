using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutotrialUI : MonoBehaviour
{
	[SerializeField] TextMeshProUGUI tutorialTextUI;
	[SerializeField] Image tutorialImgUI;

	public void SetTextTutorial(string text)
	{
		tutorialImgUI.gameObject.SetActive(true);
		tutorialTextUI.text = text;
	}
	public void HideTutorialText()
	{
		tutorialImgUI.gameObject.SetActive(false);
	}
}
