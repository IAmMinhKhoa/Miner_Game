using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ComboUI : Patterns.Singleton<ComboUI>
{
	public TMP_Text combo_lb;
	private Coroutine hideCoroutine;

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
}
