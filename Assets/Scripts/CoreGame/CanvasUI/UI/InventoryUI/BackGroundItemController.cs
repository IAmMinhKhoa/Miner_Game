using Newtonsoft.Json;
using PlayFab.EconomyModels;
using Spine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Inventory
{
    public class BackGroundItemController : MonoBehaviour
    {
		[SerializeField]
		GameObject content;
		[SerializeField]
		BackGroundItem item;
		[SerializeField]
		int amountItem;
		[SerializeField]
		UnityEngine.UI.Image imgSelectedBg;
		[SerializeField]
		TextMeshProUGUI tileSelectedBg;
		[SerializeField]
		TextMeshProUGUI descSelectedBg;
		

		List<BackGroundItem> items;
		
		InventoryItemType currentItemHandle;
		int index;
		int currentIndexImage;
		private void Awake()
		{
			items = new();
			Init();
		}
		private void Init()
		{
			for (int i = 0; i < amountItem; i++)
			{
				var _item = Instantiate(item, Vector3.zero, Quaternion.identity);
				_item.transform.SetParent(content.transform, false);
				_item.gameObject.SetActive(false);
				_item.OnBackGroundItemClick += HandleBgItemClick;
				items.Add(_item);
			}
		
		}

		private void HandleBgItemClick(int indexImg)
		{
			currentIndexImage = indexImg;
			imgSelectedBg.sprite = items[indexImg].img;
			tileSelectedBg.text = items[indexImg].iName;
			descSelectedBg.text= items[indexImg].desc;

		}
		public void ChangeBgItem()
		{
			if (currentIndexImage == -1) return;
			var shaftManager = ShaftManager.Instance;
			

			switch (currentItemHandle)
			{
				case InventoryItemType.ShaftBg:
					
					shaftManager.Shafts[this.index].shaftSkin.idBackGround = currentIndexImage.ToString();
					shaftManager.Shafts[this.index].UpdateUI();
					shaftManager.OnUpdateShaftInventoryUI?.Invoke(this.index);
					break;
				case InventoryItemType.ShaftSecondBg:
					shaftManager.Shafts[this.index].shaftSkin.idSecondBg = currentIndexImage.ToString();
					shaftManager.Shafts[this.index].UpdateUI();
					shaftManager.OnUpdateShaftInventoryUI?.Invoke(this.index);
					break;
				case InventoryItemType.ElevatorBg:
					var elevatorSystem = ElevatorSystem.Instance;
					elevatorSystem.elevatorSkin.idBackGround = currentIndexImage.ToString();
					elevatorSystem.UpdateUI();
					elevatorSystem.OnUpdateElevatorInventoryUI?.Invoke();
					break;
				case InventoryItemType.CounterBg:
					var counter = Counter.Instance;
					counter.counterSkin.idBackGround = currentIndexImage.ToString();
					counter.UpdateUI();
					counter.OnUpdateCounterInventoryUI?.Invoke();
					break;
				
			}
		}

		private void UnactiveAllItems()
		{
			for (int i = 0; i < items.Count; i++)
			{
				if(items[i].gameObject.activeInHierarchy)
					items[i].gameObject.SetActive(false);
			}
		}
		private void LoadBg(InventoryItemType itemType)
		{
			UnactiveAllItems();
			Dictionary<InventoryItemType, List<DataSkinImage>> data = new()
			{
				{InventoryItemType.ShaftBg, SkinManager.Instance.skinResource.skinBgShaft },
				{InventoryItemType.ElevatorBg, SkinManager.Instance.skinResource.skinBgElevator },
				{InventoryItemType.CounterBg, SkinManager.Instance.skinResource.skinBgCounter},
				{InventoryItemType.ShaftSecondBg, SkinManager.Instance.skinResource.skinSecondBgShaft}

			};
			Dictionary<InventoryItemType, int> curentBg = new()
			{
				{InventoryItemType.ElevatorBg, int.Parse(ElevatorSystem.Instance.elevatorSkin.idBackGround)},
				{InventoryItemType.CounterBg, int.Parse(Counter.Instance.counterSkin.idBackGround)}

			};
			if(this.index != -1)
			{
				curentBg = new()
				{
					{InventoryItemType.ShaftBg, int.Parse(ShaftManager.Instance.Shafts[this.index].shaftSkin.idBackGround)},
					{InventoryItemType.ShaftSecondBg,  int.Parse(ShaftManager.Instance.Shafts[this.index].shaftSkin.idSecondBg)}
				};
			}
			var listBg = data[itemType];
			for(int i = 0; i < listBg.Count; i++)
			{
				items[i].gameObject.SetActive(true);
				Sprite sprite = Resources.Load<Sprite>(listBg[i].path);
				string name = listBg[i].name;
				items[i].ChangItemInfo(sprite, name, i, listBg[i].desc);
				if(i == curentBg[itemType])
				{
					imgSelectedBg.sprite = items[i].img;
					tileSelectedBg.text = items[i].iName;
					descSelectedBg.text = items[i].desc;
				}
			}
		}


		public void SetItemHandle(InventoryItemType itemHandle, int index)
		{
			currentItemHandle = itemHandle;
			this.index = index;
			currentIndexImage = -1;
			LoadBg(itemHandle);
		}
	}
}
