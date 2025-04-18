using NOOD.Sound;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MarketPlayItem : MonoBehaviour
{
	[SerializeField]
	SkeletonGraphic spineHandling;
	[SerializeField]
	SkeletonGraphic seconSpine;
	[SerializeField]
	TextMeshProUGUI costDisplay;
	[SerializeField]
	Image ItemBoughtImage;
	[SerializeField]
	Image hideImage;

	public event Action<MarketPlayItem> OnItemIsBought;
	public MarketPlayItemQuality ItemQuality { set; get; }
	public string ID { get; set; }

	bool isItemBought = false;
	public bool IsItemBougth => isItemBought;

	double _cost;
	public double Cost {
		get
		{
			return _cost;
		}
		set
		{
			_cost = value;
			costDisplay.text = isItemBought ? LanguageKeys.marketHadBought.Text(): Currency.DisplayCurrency(_cost);
		}
	}
	private double _superCost = 40.0;
	public double SuperCost
	{
		get => _superCost;
		set
		{
			_superCost = value;
		}
	}
	public SkeletonGraphic SpineHandling => spineHandling;
	public SkeletonGraphic SecondSpine => seconSpine;
	public void OnItemClick()
	{
		SoundManager.PlaySound(SoundEnum.mobileClickBack);
		OnItemIsBought?.Invoke(this);
	}
	public void ItemIsBought()
	{
		isItemBought = true;
		hideImage.gameObject.SetActive(true);
		costDisplay.text = LanguageKeys.marketHadBought.Text();

	}
	public void ActiveSecondSpine()
	{
		seconSpine.gameObject.SetActive(true);
	}
}

public enum MarketPlayItemQuality
{
	low,
	normal,
	super,
	ultra
}
