using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ModalShowEvent : MonoBehaviour
{
	public CanvasGroup canvasGroup;


	//set event in button
	public void OpenMarket()
	{
		canvasGroup.DOFade(0, 0.3f).SetEase(Ease.OutQuad).OnComplete(() =>
		{
			//open market
			GameUI.Instance.OpenStore();
		});
	}


	//set event in button
	public void CloseModal()
	{
		canvasGroup.DOFade(0, 0.3f).SetEase(Ease.OutQuad).OnComplete(() =>
		{
			Destroy(gameObject);
		});
	}
}
