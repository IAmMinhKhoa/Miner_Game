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
	TextMeshProUGUI costDisplay;
	[SerializeField]
	Image ItemBoughtImage;

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
			costDisplay.text = isItemBought ? "Đã sở hữu": Currency.DisplayCurrency(_cost);
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

	public void OnItemClick()
	{
		OnItemIsBought?.Invoke(this);
	}
	public void ItemIsBought()
	{
		isItemBought = true;
		costDisplay.text = "Đã sở hữu";
	}
}

public enum MarketPlayItemQuality
{
	low,
	normal,
	super
}
