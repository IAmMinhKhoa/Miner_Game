using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ComboUI : Patterns.Singleton<ComboUI>
{
	public TMP_Text combo_lb;
	private Coroutine hideCoroutine;
	public List<Image> comboImages;
	public UnityEvent OnImageAnimation;
	public void Hide(GameObject comboLabel, float delay)
	{
		if (hideCoroutine != null)
		{
			StopCoroutine(hideCoroutine);
		}

		hideCoroutine = StartCoroutine(HideComboLabel(comboLabel, delay));
	}
	private IEnumerator HideComboLabel(GameObject comboLabel, float delay)
	{
		yield return new WaitForSeconds(delay);
		transform.DOScale(Vector3.zero, 0.3f)
			.SetEase(Ease.InBack)
			.OnComplete(() => comboLabel.SetActive(false));
	}
	public void ShowComboUI()
	{
		transform.localScale = Vector3.one;
		gameObject.SetActive(true);
		OnImageAnimation?.Invoke();
		//transform.localScale = Vector3.zero;
		//transform.DOScale(Vector3.one, 0.5f)
		//	.SetEase(Ease.OutBack);
	}

	public void UpdateComboUI(int comboCount)
	{
		string comboString = comboCount.ToString(); 
		for (int i = 0; i < comboImages.Count; i++)
		{
			if (i < comboString.Length)
			{
				int digit = int.Parse(comboString[i].ToString());
				comboImages[i].sprite = Resources.Load<Sprite>($"UI/Combo/{digit}");
				comboImages[i].gameObject.SetActive(true);
			}
			else
			{
				comboImages[i].gameObject.SetActive(false); 
			}
		}
	}

}
