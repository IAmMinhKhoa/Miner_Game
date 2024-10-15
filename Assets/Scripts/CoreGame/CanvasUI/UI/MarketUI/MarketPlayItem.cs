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

	string _cost;
	public string Cost {
		get
		{
			return _cost;
		}
		set
		{
			_cost = value;
			costDisplay.text = _cost;
		}
	}
	public SkeletonGraphic SpineHandling => spineHandling;

	public void OnItemClick()
	{
		OnItemIsBought?.Invoke(this);
	}
	public void ItemIsBought()
	{
		ItemBoughtImage.gameObject.SetActive(true);
		costDisplay.gameObject.SetActive(false);
	}
}

public enum MarketPlayItemQuality
{
	low,
	normal,
	super
}
