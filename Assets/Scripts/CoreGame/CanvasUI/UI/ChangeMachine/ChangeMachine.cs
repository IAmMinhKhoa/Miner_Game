using Cysharp.Threading.Tasks;
using DG.Tweening;
using NOOD.Sound;
using PlayFab;
using PlayFab.ClientModels;
using Sirenix.OdinInspector;
using Spine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.SmartFormat.Utilities;
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
	[SerializeField] BuySpecialItemUI buySpecialItemUI;
	

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
	[SerializeField] BuyableGachaItem buyableGachaItem;
	[Header("GameOBJ")]
	[SerializeField] GameObject containerSpecialSkin;


	List<BuyableGachaItem> buyableGachaItems = new();


	float coin = 0;
	private void Awake()
	{
		interiorToggle.onClick.AddListener(OnInteriorButtonClick);
		staffToggle.onClick.AddListener(OnStaffButtonClick);
		CloseUIButton.onClick.AddListener(FadeOutContainer);
		longItemBoxGacha.OnSkipButtonClick += OnInteriorButtonClick;
		shortItemBoxGacha.OnSkipButtonClick += OnInteriorButtonClick;
		staffItemBoxGacha.OnSkipButtonClick += OnStaffButtonClick;
		buySpecialItemUI.OnButtonBuyClick += BuyItem;
	}
	private void OnEnable()
	{
		FadeInContainer();
		GetVirtualCurrencies();
		InitBuyableGachaItem();
	}

	private void InitBuyableGachaItem()
	{
		foreach(var item in buyableGachaItems)
		{
			Destroy(item.gameObject);
		}
		buyableGachaItems.Clear();
		foreach(var it in interior)
		{
			foreach(var item in it.listSkinGacha)
			{
				if(item.isSpecialSKin)
				{
					var skinSpecial = Instantiate(buyableGachaItem, containerSpecialSkin.transform);
					skinSpecial.InfoItem = new GachaItemInfor(it.type, item);
					skinSpecial.Init("Icon_" + item.ID);
					skinSpecial.buyItemClicked += OpenSpecialItemUI;
					buyableGachaItems.Add(skinSpecial);
				}
			}
		}
		foreach (var it in staff)
		{
			foreach (var item in it.listSkinGacha)
			{
				if (item.isSpecialSKin)
				{
					var skinSpecial = Instantiate(buyableGachaItem, containerSpecialSkin.transform);
					skinSpecial.InfoItem = new GachaItemInfor(it.type, item);
					skinSpecial.InitStaff();
					skinSpecial.buyItemClicked += OpenSpecialItemUI;
					buyableGachaItems.Add(skinSpecial);
				}
			}
		}
	}
	//Open UI buy special Item
	void OpenSpecialItemUI(BuyableGachaItem item)
	{
		GetVirtualCurrencies();
		buySpecialItemUI.Initiallize(item, coin >= item.Itemprice, coin);
	}
	private void BuyItem(BuyableGachaItem item)
	{
		GetVirtualCurrencies();
		if (coin < item.Itemprice) return;
		SkinManager.Instance.BuyNewSkin(item.InfoItem.type, item.InfoItem.skinGachaInfor.ID);
		BuyItemFromChangeMachine((int)item.Itemprice);
		item.ItemBougth(true);
	}

	private void OnDestroy()
	{
		interiorToggle.onClick.RemoveAllListeners();
		staffToggle.onClick.RemoveAllListeners();
		CloseUIButton.onClick.RemoveAllListeners();
		longItemBoxGacha.OnSkipButtonClick -= OnInteriorButtonClick;
		shortItemBoxGacha.OnSkipButtonClick -= OnInteriorButtonClick;
		staffItemBoxGacha.OnSkipButtonClick -= OnStaffButtonClick;
		buySpecialItemUI.OnButtonBuyClick -= BuyItem;
	}
	void OnInteriorButtonClick()
	{
		SoundManager.PlaySound(SoundEnum.openloot);
		RectTransform _rectTransform = interiorToggle.GetComponentInChildren<RectTransform>();
		Vector3 defaultScale = _rectTransform.localScale;
		_rectTransform.DOScale(1f, 0.1f)
			.SetEase(Ease.OutQuad)
			.OnComplete(() =>
			{
				_rectTransform.DOScale(defaultScale, 0.1f).SetEase(Ease.InQuad).OnComplete(() =>
				{
					exchangeItemUI.gameObject.SetActive(true);
					exchangeItemUI.SetUpUI(coin, true);
					exchangeItemUI.OnGachaButtonClick += HandleGachaInterior;
				});
			});

		
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

		BuyItemFromChangeMachine(300 * amount);

		while(listLongItem.Count + listShortItem.Count < amount)
		{
			var item = listItemAvaliableGacha[UnityEngine.Random.Range(0, listItemAvaliableGacha.Count - 1)];

			switch (item.type)
			{
				case InventoryItemType.ShaftBg:
				case InventoryItemType.CounterBg:
				case InventoryItemType.ShaftSecondBg:
					var initedLongItem = Instantiate(longGachaItemPrefab);
					if(!initedLongItem.InitialData(item, "Click_" + item.skinGachaInfor.ID))
					{
						Destroy(initedLongItem.gameObject);
						continue;
					}
					initedLongItem.gameObject.SetActive(false);
					listLongItem.Add(initedLongItem);
					Debug.Log(item.type + amount);
					break;
				default:
					var initedShortItem = Instantiate(shortGachaItemPrefab);
					if(!initedShortItem.InitialData(item, "Icon_" + item.skinGachaInfor.ID))
					{
						Destroy(initedShortItem.gameObject);
						continue;
					}
					initedShortItem.gameObject.SetActive(false);
					listShortItem.Add(initedShortItem);
					Debug.Log(item.type + amount);
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

		BuyItemFromChangeMachine(300 * amount);


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
		SoundManager.PlaySound(SoundEnum.openloot);
		RectTransform _rectTransform = staffToggle.GetComponentInChildren<RectTransform>();
		Vector3 defaultScale = _rectTransform.localScale;
		_rectTransform.DOScale(1f, 0.1f)
			.SetEase(Ease.OutQuad)
			.OnComplete(() =>
			{
				_rectTransform.DOScale(defaultScale, 0.1f).SetEase(Ease.InQuad).OnComplete(() =>
				{
					exchangeItemUI.gameObject.SetActive(true);
					exchangeItemUI.SetUpUI(coin, false);
					exchangeItemUI.OnGachaButtonClick += HandleGachaStaff;
				});
			});
	}

	public void BuyItemFromChangeMachine(int price)
	{
		var request = new SubtractUserVirtualCurrencyRequest
		{
			VirtualCurrency = "MC",
			Amount = price
		};
		PlayFabClientAPI.SubtractUserVirtualCurrency(request, OnBuySuccessID0, OnError);
	}

	private void OnError(PlayFabError error)
	{
		throw new NotImplementedException();
	}

	private void OnBuySuccessID0(ModifyUserVirtualCurrencyResult result)
	{
		GetVirtualCurrencies();
	}

	public void GetVirtualCurrencies()
	{
		PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), OnGetUserInventorySuccess, OnError);
	}

	private void OnGetUserInventorySuccess(GetUserInventoryResult result)
	{
		coin = result.VirtualCurrency["MC"];
		PlayfabMinigame.Instance.GetVirtualCurrencies();
	}

	void CloseUI()
	{
		gameObject.SetActive(false);
	}

	#region AnimateUI
	[Button]
	public void FadeInContainer()
	{
		gameObject.SetActive(true);
		Vector2 posCam = CustomCamera.Instance.GetCurrentTransform().position;
		gameObject.transform.localPosition = new Vector2(posCam.x - 2000, posCam.y); //Left Screen
		gameObject.transform.DOLocalMoveX(0, 0.6f).SetEase(Ease.OutQuart);


	}
	[Button]
	public void FadeOutContainer()
	{
		Vector2 posCam = CustomCamera.Instance.GetCurrentTransform().position;
		gameObject.transform.DOLocalMoveX(posCam.x - 2000f, 0.6f).SetEase(Ease.InQuart).OnComplete(() =>
		{
			CloseUI();
		});

	}
	#endregion
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
