using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupToggle : MonoBehaviour
{
	public void OnChoosing(bool isOn)
	{
		if (isOn)
		{
			RectTransform _rectTransform = GetComponent<RectTransform>();
			_rectTransform.DOScale(1.1f, 0.2f)
				.SetEase(Ease.OutQuad);
		}
		else
		{
			RectTransform _rectTransform = GetComponent<RectTransform>();
			_rectTransform.DOScale(1f, 0f)
				.SetEase(Ease.OutQuad);
		}
	}
}
