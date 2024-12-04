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
	public Image combo_image;
	public Image X_image;
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
		gameObject.SetActive(true);
		transform.localScale = Vector3.zero;
		transform.DOScale(Vector3.one, 0.5f)
			.SetEase(Ease.OutBack);
	}

	public void UpdateComboUI(int comboCount)
	{
		string comboString = comboCount.ToString();
		string path=string.Empty;
		if(comboCount<=20)
		{
			path = "UI/ComBO/";
			combo_image.overrideSprite = Resources.Load<Sprite>("UI/ComBO/combo");
			X_image.overrideSprite = Resources.Load<Sprite>("UI/ComBO/x");
		}
		else if(comboCount>21&&comboCount<=50)
		{
			path = "UI/ComBO/b_";
			combo_image.overrideSprite = Resources.Load<Sprite>("UI/ComBO/combo_b");
			X_image.overrideSprite = Resources.Load<Sprite>("UI/ComBO/x_b");
		}
		else
		{
			path = "UI/ComBO/c_";
			combo_image.overrideSprite = Resources.Load<Sprite>("UI/ComBO/combo_c");
			X_image.overrideSprite = Resources.Load<Sprite>("UI/ComBO/x_c");
		}	
		for (int i = 0; i < comboImages.Count; i++)
		{
			if (i < comboString.Length)
			{
				int digit = int.Parse(comboString[i].ToString());
				comboImages[i].sprite = Resources.Load<Sprite>(path + digit);
				comboImages[i].gameObject.SetActive(true);
			}
			else
			{
				comboImages[i].gameObject.SetActive(false); 
			}
		}
	}

}
