using Spine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChangeMachine : MonoBehaviour
{
	[Header("Panel")]
	[SerializeField]
	Button interiorToggle;
	[SerializeField]
	Button staffToggle;
	[SerializeField]
	ExchangeItemUI exchangeItemUI;
	[SerializeField]
	Button CloseUIButton;
	[SerializeField]
	ItemBoxsGacha itemBoxsGacha;
	[SerializeField]
	ShortItemBoxGacha shortItemBoxGacha;

	[Header("Item Gacha")]
	[SerializeField]
	List<SkinChangeMachineSO> interior;
	[SerializeField]
	List<SkinChangeMachineSO> staff;
	[SerializeField]

	[Header("Text")]
	TextMeshProUGUI currentCoin;


	[Header("Prefabs")]
	[SerializeField]
	LongGachaItem longGachaItemPrefab;
	[SerializeField]
	ShortGachaItem shortGachaItemPrefab;

	int coin = 20000;
	private void Start()
	{
		interiorToggle.onClick.AddListener(OnInteriorButtonClick);
		staffToggle.onClick.AddListener(OnStaffButtonClick);
		CloseUIButton.onClick.AddListener(CloseUI);
	}
	private void OnEnable()
	{
		currentCoin.text = coin.ToString();	
	}
	void OnInteriorButtonClick()
	{
		exchangeItemUI.gameObject.SetActive(true);
		exchangeItemUI.SetUpUI(coin);
		exchangeItemUI.OnGachaButtonClick += HandleGachaInterior;
	}

	void HandleGachaInterior(int amount)
	{
		List<GachaItemInfor> listItemAvaliableGacha = GetListGachaItem(interior);
		List<ShortGachaItem> listShortItem = new();
		List<LongGachaItem> listLongItem = new();
		for (int i = 0; i < amount; i++)
		{
			var item = listItemAvaliableGacha[UnityEngine.Random.Range(0, listItemAvaliableGacha.Count - 1)];
			switch (item.type)
			{
				case InventoryItemType.ShaftBg:
				case InventoryItemType.CounterBg:
				case InventoryItemType.ShaftSecondBg:
					var initedLongItem = Instantiate(longGachaItemPrefab);
					initedLongItem.InitialData(item, "Click_" + item.skinGachaInfor.ID);
					listLongItem.Add(initedLongItem);
					break;
				default:
					var initedShortItem = Instantiate(shortGachaItemPrefab);
					initedShortItem.InitialData(item, "Icon_" + item.skinGachaInfor.ID);
					listShortItem.Add(initedShortItem);
					break;
			}
			listItemAvaliableGacha.Remove(item);
		}
		itemBoxsGacha.InitialData(listShortItem, listLongItem);
	}

	List<GachaItemInfor> GetListGachaItem(List<SkinChangeMachineSO> listSkin)
	{
		List<GachaItemInfor> listItemAvaliableGacha = new ();
		foreach (var skin in listSkin)
		{
			foreach (var item in skin.listSkinGacha)
			{
				if(!SkinManager.Instance.ItemBought[skin.type].Contains(item.ID))
				{
					listItemAvaliableGacha.Add(new(skin.type, item));
				}
			}
		}
		return listItemAvaliableGacha;
	}
	void OnStaffButtonClick()
	{
		exchangeItemUI.gameObject.SetActive(true);
		exchangeItemUI.SetUpUI(coin);
		exchangeItemUI.OnGachaButtonClick += HandleGachaStaff;
	}

	private void HandleGachaStaff(int amount)
	{
		Debug.Log("-999999");
	}
	
	void CloseUI()
	{
		gameObject.SetActive(false);
	}
}
public struct GachaItemInfor
{
	public InventoryItemType type;
	public SkinGachaInfor skinGachaInfor;

	public GachaItemInfor(InventoryItemType type, SkinGachaInfor skinGachaInfor)
	{
		this.type = type;
		this.skinGachaInfor = skinGachaInfor;
	}
}
