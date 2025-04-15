using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class ModalShowEvent : MonoBehaviour
{
	public CanvasGroup canvasGroup;
	public GameObject mainObj;

	public ParticleSystem fx;
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
	[Button]
	public void CloseModal()
	{
		canvasGroup.DOFade(0, 0.3f).SetEase(Ease.OutQuad).OnComplete(() =>
		{
			Destroy(gameObject);
		});
	}

	private IEnumerator IEOpenModal()
	{
		mainObj.transform.localScale = Vector3.zero;
		mainObj.transform.DOScale(1, 0.5f).SetEase(Ease.InOutBack);
		yield return new WaitForSeconds(0.38f);
		fx.Play();
	}
	public void OpenModal()
	{
		StartCoroutine(IEOpenModal());
	}
}
