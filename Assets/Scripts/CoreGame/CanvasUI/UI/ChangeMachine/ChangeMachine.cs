using Cysharp.Threading.Tasks;
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
	[SerializeField]
	StaffGachaItem staffGachaItemPrefab;
	[SerializeField]
	ItemBoxGacha longItemBoxGacha;
	[SerializeField]
	ItemBoxGacha shortItemBoxGacha;
	[SerializeField]
	ItemBoxGacha staffItemBoxGacha;


	int coin = 20000;
	private void Start()
	{
		interiorToggle.onClick.AddListener(OnInteriorButtonClick);
		staffToggle.onClick.AddListener(OnStaffButtonClick);
		CloseUIButton.onClick.AddListener(CloseUI);
		longItemBoxGacha.OnSkipButtonClick += OnInteriorButtonClick;
		shortItemBoxGacha.OnSkipButtonClick += OnInteriorButtonClick;
		staffItemBoxGacha.OnSkipButtonClick += OnStaffButtonClick;
	}
	private void OnEnable()
	{
		currentCoin.text = coin.ToString();	
	}
	private void OnDestroy()
	{
		longItemBoxGacha.OnSkipButtonClick -= OnInteriorButtonClick;
		shortItemBoxGacha.OnSkipButtonClick -= OnInteriorButtonClick;
		staffItemBoxGacha.OnSkipButtonClick -= OnStaffButtonClick;
	}
	void OnInteriorButtonClick()
	{
		exchangeItemUI.gameObject.SetActive(true);
		exchangeItemUI.SetUpUI(coin, true);
		exchangeItemUI.OnGachaButtonClick += HandleGachaInterior;
	}

	void HandleGachaInterior(int amount)
	{
		List<GachaItemInfor> listItemAvaliableGacha = GetListGachaItem(interior);
		List<ShortGachaItem> listShortItem = new();
		List<LongGachaItem> listLongItem = new();
	
		if(listItemAvaliableGacha.Count < amount)
		{
			Debug.Log($"Số skin có thể quay tối đa là {listItemAvaliableGacha.Count}");
			return;
		}

		coin -= 300 * amount;
		currentCoin.text = coin.ToString();

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
					initedLongItem.gameObject.SetActive(false);
					listLongItem.Add(initedLongItem);
					break;
				default:
					var initedShortItem = Instantiate(shortGachaItemPrefab);
					initedShortItem.InitialData(item, "Icon_" + item.skinGachaInfor.ID);
					initedShortItem.gameObject.SetActive(false);
					listShortItem.Add(initedShortItem);
					break;
			}
			//SkinManager.Instance.BuyNewSkin(item.type, item.skinGachaInfor.ID);
			listItemAvaliableGacha.Remove(item);
			if(amount == 1)
			{
				if (listLongItem.Count == 1)
				{
					longItemBoxGacha.InitialData(item,"Click_" ,false);
				}
				if (listShortItem.Count == 1)
				{
					shortItemBoxGacha.InitialData(item, "Icon_", true);
				}
				return;
			}
		}
		
		itemBoxsGacha.InitialData(listShortItem, listLongItem, new List<StaffGachaItem>()).Forget();
	}

	private void HandleGachaStaff(int amount)
	{
		List<GachaItemInfor> listItemAvaliableGacha = GetListGachaItem(staff);
		List<StaffGachaItem> listStaffItem = new();
		if (listItemAvaliableGacha.Count < amount)
		{
			Debug.Log($"Số skin có thể quay tối đa là {listItemAvaliableGacha.Count}");
			return;
		}
		coin -= 300 * amount;
		currentCoin.text = coin.ToString();
		for (int i = 0; i < amount; i++)
		{
			var item = listItemAvaliableGacha[UnityEngine.Random.Range(0, listItemAvaliableGacha.Count - 1)];

			var initedLongItem = Instantiate(staffGachaItemPrefab);
			initedLongItem.InitialData(item);
			initedLongItem.gameObject.SetActive(false);
			listStaffItem.Add(initedLongItem);

			//SkinManager.Instance.BuyNewSkin(item.type, item.skinGachaInfor.ID);
			listItemAvaliableGacha.Remove(item);
			if (amount == 1)
			{
				staffItemBoxGacha.InitialData(item, "Head/Skin_", true);
				return;
			}
		}
		itemBoxsGacha.InitialData(new List<ShortGachaItem>(), new List<LongGachaItem>(), listStaffItem).Forget();

	}
	List<GachaItemInfor> GetListGachaItem(List<SkinChangeMachineSO> listSkin)
	{
		List<GachaItemInfor> listItemAvaliableGacha = new();
		foreach (var skin in listSkin)
		{
			foreach (var item in skin.listSkinGacha)
			{
				if (!SkinManager.Instance.ItemBought[skin.type].Contains(item.ID))
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
		exchangeItemUI.SetUpUI(coin, false);
		exchangeItemUI.OnGachaButtonClick += HandleGachaStaff;
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
