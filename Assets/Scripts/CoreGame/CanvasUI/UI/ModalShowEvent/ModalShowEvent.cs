using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using Spine.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[Serializable]
public class DataEvent
{
	public Sprite imgBanner;
	public string content;

}
public class ModalShowEvent : MonoBehaviour
{
	//---Data---
	[SerializeField]public List<DataEvent> DataEvents = new List<DataEvent>();
	//-----

	public CanvasGroup canvasGroup;
	public GameObject mainObj;
	public SkeletonGraphic spineStrokle;
	public ParticleSystem fx;
	public Image imgBanner;
	public TMP_Text textContinutes;
	private int currentPage = 0;
	#region Open/Close Modal

	//set event in button
	public void ContinuteAction()
	{
		SlideBannerByIndex(currentPage);
		currentPage++;
		if (currentPage == DataEvents.Count)
		{
			textContinutes.text = "Open Market";
		}
		if (currentPage > DataEvents.Count)
		{
			//convert to button open market
			CloseModal();
			canvasGroup.DOFade(0, 0.3f).SetEase(Ease.OutQuad).OnComplete(() =>
			{
				//open market
				GameUI.Instance.OpenStore();
			});
		}

	}


	//set event in button
	[Button]
	public void CloseModal()
	{
		Common.DOFadeSkeletonGraphic(spineStrokle, 0f,0.3f);
		canvasGroup.DOFade(0, 0.3f).SetEase(Ease.OutQuad).OnComplete(() =>
		{
			Destroy(gameObject);
		});
	}

	public float timingFx = 0.2f;
	private IEnumerator IEOpenModal()
	{
		mainObj.transform.localScale = Vector3.zero;


		Common.DOFadeSkeletonGraphic(spineStrokle, 1f,0.5f);
		mainObj.transform.DOScale(1, 0.5f).SetEase(Ease.InOutBack);
		yield return new WaitForSeconds(timingFx);
		fx.Play();
	}
	[Button]
	public void OpenModal()
	{
		StartCoroutine(IEOpenModal());
	}

	#endregion

	private void SlideBannerByIndex(int index)
	{
		/*
		 *  animation scale from 1 -> 0
		 *  Fade out alpha from 1 -> 0
		 * ---Change data----
		 *  fade in and scale from 0 -> 1
		 */
		mainObj.transform.localScale = Vector3.one;
		Sequence tweenBanner = DOTween.Sequence();
		tweenBanner.Join(mainObj.transform.DOScale(0,0.2f).SetEase(Ease.OutQuad));
		tweenBanner.Join(canvasGroup.DOFade(0,0.2f).SetEase(Ease.OutQuad));
		tweenBanner.Play();

		//---- Change data ---
		tweenBanner.onComplete += () =>
		{
			imgBanner.sprite = DataEvents[index].imgBanner;

			//---Fade in---
			Sequence tweenBannerIn = DOTween.Sequence();
			tweenBannerIn.Join(mainObj.transform.DOScale(1, 0.2f).SetEase(Ease.OutQuad));
			tweenBannerIn.Join(canvasGroup.DOFade(1, 0.2f).SetEase(Ease.OutQuad));
			tweenBannerIn.Play();
		};


	}
}
