using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MarketPlayItem : MonoBehaviour
{
	[SerializeField]
	SkeletonGraphic spineHandling;
	[SerializeField]
	TextMeshProUGUI costDisplay;
	public MarketPlayItemQuality ItemQuality { set; get; }
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

	
}

public enum MarketPlayItemQuality
{
	Low,
	Normal,
	Super
}
