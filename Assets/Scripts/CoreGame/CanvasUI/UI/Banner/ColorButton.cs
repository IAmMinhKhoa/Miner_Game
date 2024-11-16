using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ColorButton : MonoBehaviour
{
	[SerializeField] Image imgColor;
	[SerializeField] Button btn;
	[SerializeField] Image boder;
	public Action Select;
	private void Start()
	{
		btn.onClick.AddListener(OnSelect);
	}
	public void SetData(Color _color, Action _select)
	{
		Select = _select;
		imgColor.color = _color;

	}
	private void OnDestroy()
	{
		btn.onClick.RemoveAllListeners();
	}

	public void HideBoder() => boder.gameObject.SetActive(false);

	private void OnSelect()
	{
		// Invoke the Select event
		Select?.Invoke();
		boder.gameObject.SetActive(true);
		// Create a scale effect using DOTween
		gameObject.transform.DOScale(Vector3.one * 1.16f, 0.15f) // Scale up to 120% over 0.3 seconds
			.SetEase(Ease.OutBack) // Use an 'Ease Out Back' effect for a bouncy animation
			.OnComplete(() =>
			{
				// Scale back to the original size
				gameObject.transform.DOScale(Vector3.one, 0.1f) // Scale down to original size over 0.2 seconds
					.SetEase(Ease.InOutQuad); // Smooth easing for the return animation
			});
	}

}
