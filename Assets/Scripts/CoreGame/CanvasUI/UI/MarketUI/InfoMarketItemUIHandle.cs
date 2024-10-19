using DG.Tweening;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoMarketItemUIHandle : MonoBehaviour
{
	[SerializeField]
	GameObject backDrop;
	[SerializeField]
	GameObject notEnoughMoneyNotification;
	[SerializeField]
	SkeletonGraphic Spine;
	[SerializeField]
	TextMeshProUGUI normalCost;
	[SerializeField]
	TextMeshProUGUI superMoneyCost;
	[SerializeField]
	Sprite lowQuality;
	[SerializeField]
	Sprite normalQuality;
	[SerializeField]
	Sprite superQuality;
	[SerializeField]
	UnityEngine.UI.Image quality;
	[SerializeField]
	Button normalBuyButton;
	[SerializeField]
	Button superBuyButton;
	[SerializeField]
	Image hideNormalBuyIMG;
	[SerializeField]
	Image hideSuperBuyIMG;
	public event Action<MarketPlayItem> OnButtonBuyClick;

	MarketPlayItem curItemHandling;
	bool CurState
	{
		set
		{
			if (value)
			{
				FadeInContainer();
			}
			else
			{
				FadeOutContainer();
			}
		}
	}
	public void FadeInContainer()
	{
		gameObject.SetActive(true);
		backDrop.SetActive(true);
		Vector2 posCam = CustomCamera.Instance.GetCurrentTransform().position;
		gameObject.transform.localPosition = new Vector2(posCam.x - 2000, posCam.y); //Left Screen
		gameObject.transform.DOLocalMoveX(0, 0.6f).SetEase(Ease.OutQuart);
	}
	public void FadeOutContainer()
	{
		Vector2 posCam = CustomCamera.Instance.GetCurrentTransform().position;
		gameObject.transform.DOLocalMoveX(posCam.x - 2000f, 0.6f).SetEase(Ease.InQuart).OnComplete(() =>
		{
			gameObject.SetActive(false);
			backDrop.SetActive(false);
			curItemHandling = null;
		});
	}
	public void Close()
	{
		if (!gameObject.activeInHierarchy) return;
		CurState = false;
	}
	public void Open()
	{
		CurState = true;
	}

	public void Init(ItemSize itemSize, MarketPlayItem it)
	{

		switch(it.ItemQuality)
		{
			case MarketPlayItemQuality.low:
				quality.sprite = lowQuality;
				break;
			case MarketPlayItemQuality.normal:
				quality.sprite = normalQuality;
				break;
			case MarketPlayItemQuality.super:
				quality.sprite = superQuality;
				break;
		}

		curItemHandling = it;
		if(it.SpineHandling.skeletonDataAsset.GetSkeletonData(false).FindAnimation("Icon") != null) {
			Spine.startingAnimation = "Icon";
		}
		Spine.initialSkinName = itemSize.skinName + it.ID;
		Spine.skeletonDataAsset = it.SpineHandling.skeletonDataAsset;
		Spine.Initialize(true);


		var transform = Spine.GetComponent<RectTransform>();
		transform.localScale = itemSize.scale;
		transform.anchoredPosition = itemSize.pos;
		Spine.Skeleton.SetSkin(itemSize.skinName + it.ID);
		Spine.Skeleton.SetSlotsToSetupPose();

		normalCost.text =  Currency.DisplayCurrency(it.Cost);
		superMoneyCost.text = it.SuperCost.ToString();
		if (it.IsItemBougth)
		{
			normalCost.text = "Đã sở hữu";
			superMoneyCost.text = "Đã sở hữu";
			normalBuyButton.interactable = false;
			superBuyButton.interactable = false;
			hideNormalBuyIMG.gameObject.SetActive(true);
			hideSuperBuyIMG.gameObject.SetActive(true);
		}
		else
		{
			normalBuyButton.interactable = true;
			superBuyButton.interactable = true;
			hideNormalBuyIMG.gameObject.SetActive(false);
			hideSuperBuyIMG.gameObject.SetActive(false);
		}
	}

	public void Buy()
	{
	
		if(curItemHandling == null) return;
		if (curItemHandling.Cost > PawManager.Instance.CurrentPaw)
		{
			notEnoughMoneyNotification.SetActive(true);
			Debug.Log("------------------");
			return;
		}
		OnButtonBuyClick?.Invoke(curItemHandling);
		Close();
	}
}
