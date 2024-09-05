using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static PlayFabDemo.BuyButton;

namespace PlayFabDemo
{
	public class BuyButton : MonoBehaviour
	{
		public List<ItemCanBuy> listItemBuy;
		
		public enum ItemName
		{
			Bear,
			Dog,
			Tiger
		}
	}
	[Serializable]
	public class ItemCanBuy
	{
		public ItemName itemName;
		public TMP_InputField amountBuy;
		public TextMeshProUGUI currentAmount;
		public ItemCanBuy(ItemName itemName, TMP_InputField amountBuy, TextMeshProUGUI currentAmount)
		{
			this.itemName = itemName;
			this.amountBuy = amountBuy;
			this.currentAmount = currentAmount;
		}
	}
}
