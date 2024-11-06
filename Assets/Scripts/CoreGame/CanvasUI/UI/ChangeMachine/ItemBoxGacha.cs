using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemBoxGacha : MonoBehaviour
{
	[SerializeField]
	Image lowBg;
	[SerializeField]
	Image normalBg;
	[SerializeField]
	Image superBg;
	[SerializeField]
	Image ultraBg;
	[SerializeField]
	Image low;
	[SerializeField]
	Image normal;
	[SerializeField]
	Image super;
	[SerializeField]
	Image ultra;
	[SerializeField]
	Image type;
	[SerializeField]
	SkeletonGraphic skeletonGraphic;
	[SerializeField]
	SkeletonGraphic subSkeletonGraphic;


	[Header("Sprite")]
	[SerializeField]
	Sprite lowType;
	[SerializeField]
	Sprite normalType;
	[SerializeField]
	Sprite superType;
	[SerializeField]
	Sprite ultraType;

	[Header("Button")]
	[SerializeField]
	Button closeUIButton;
	[SerializeField]
	Button skipButton;
	[Header("Text")]
	[SerializeField]
	TextMeshProUGUI titile;
	[SerializeField]
	TextMeshProUGUI description;

	public event Action OnSkipButtonClick;
	private void Start()
	{
		closeUIButton.onClick.AddListener(CloseUIOnClick);
		skipButton.onClick.AddListener(SkipOnClick);
	}

	private void SkipOnClick()
	{
		OnSkipButtonClick?.Invoke();
		gameObject.SetActive(false);
	}

	private void CloseUIOnClick()
	{
		gameObject.SetActive(false);
	}

	public void InitialData(GachaItemInfor itemInfo,string initialSkiName ,bool isUpdatePos)
	{
		gameObject.SetActive(true);
		var items = new[] { low, normal, super, ultra };
		var backgrounds = new[] { lowBg, normalBg, superBg, ultraBg };
		foreach (var item in items) item.gameObject.SetActive(false);
		foreach (var bg in backgrounds) bg.gameObject.SetActive(false);

		switch (itemInfo.skinGachaInfor.quality)
		{
			case MarketPlayItemQuality.low:
				low.gameObject.SetActive(true);
				lowBg.gameObject.SetActive(true);
				type.sprite = lowType;
				break;
			case MarketPlayItemQuality.normal:
				normal.gameObject.SetActive(true);
				normalBg.gameObject.SetActive(true);
				type.sprite = normalType;
				break;
			case MarketPlayItemQuality.super:
				super.gameObject.SetActive(true);
				superBg.gameObject.SetActive(true);
				type.sprite = superType;
				break;
			case MarketPlayItemQuality.ultra:
				ultra.gameObject.SetActive(true);
				ultraBg.gameObject.SetActive(true);
				type.sprite = ultraType;
				break;
		}

		skeletonGraphic.skeletonDataAsset = SkinManager.Instance.SkinGameDataAsset.SkinGameData[itemInfo.type];
		skeletonGraphic.initialSkinName = initialSkiName + itemInfo.skinGachaInfor.ID;
		skeletonGraphic.Initialize(true);
		if(subSkeletonGraphic != null)
		{
			subSkeletonGraphic.skeletonDataAsset = SkinManager.Instance.SkinGameDataAsset.SkinGameData[itemInfo.type];
			subSkeletonGraphic.initialSkinName = "Body/Skin_" + itemInfo.skinGachaInfor.ID;
			subSkeletonGraphic.Initialize(true);
			var spine = subSkeletonGraphic.GetComponent<RectTransform>();
			spine.localScale = itemInfo.skinGachaInfor.ScaleSingle;
			spine.anchoredPosition = itemInfo.skinGachaInfor.PositonSingle;
		}

		var iconAnimation = skeletonGraphic.skeletonDataAsset.GetSkeletonData(false).FindAnimation("Icon");
		if (iconAnimation != null)
		{
			skeletonGraphic.AnimationState.SetAnimation(0, "Icon", false);
		}
		else
		{
			skeletonGraphic.AnimationState.ClearTrack(0);
		}

		titile.text = itemInfo.skinGachaInfor.Name;
		description.text = itemInfo.skinGachaInfor.Description;

		if(isUpdatePos) {
			var spine = skeletonGraphic.GetComponent<RectTransform>();
			spine.localScale = itemInfo.skinGachaInfor.ScaleSingle;
			spine.anchoredPosition = itemInfo.skinGachaInfor.PositonSingle;
		}
	}
}
