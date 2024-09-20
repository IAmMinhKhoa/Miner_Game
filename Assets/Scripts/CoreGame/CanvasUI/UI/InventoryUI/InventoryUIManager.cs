using DG.Tweening;
using PlayFab.EconomyModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;


namespace UI.Inventory
{
    public class InventoryUIManager : MonoBehaviour
	{
		[Header("UI")]
        [SerializeField] private Toggle tgNoiThat;
        [SerializeField] private Toggle tgNhanVien;
        [SerializeField] private GameObject pnNoiThat;
        [SerializeField] private GameObject pnNhanVien;
        [SerializeField] GameObject inventoryPanel;
		[SerializeField] BackGroundItemController bgList;
		public PopupOtherItemController pOIController;
		[SerializeField] StaffSkinUI staffSkinUI;
		public StaffSkinUI StaffSkin => staffSkinUI;

		[Header("Shaft Item Handle")]
		[SerializeField]
		GameObject shaftContent;
		[SerializeField]
		ShaftUIController shaftUIController;
		List<ShaftUIController> listShaftUI = new();
		[SerializeField] List<StaffSkinItem> listShaftStaffSkin;


		[Header("Counter")]
		[SerializeField] DecoratorItem[] counterItem;
		[SerializeField] StaffSkinItem counterStaffSkin;

		[Header("Elevator")]
		[SerializeField] DecoratorItem[] elevatorItem;
		[SerializeField] StaffSkinItem elevatorStaffSkin;
	

		int shaftCount = 0;
		bool isBackgroundItemOpening;
		
		private void OnEnable()
		{
			
			ShaftManager.Instance.OnUpdateShaftInventoryUI += HanleUpdateShaftIUI;
			ElevatorSystem.Instance.OnUpdateElevatorInventoryUI += HandleElevatorIUI;
			Counter.Instance.OnUpdateCounterInventoryUI += HandleCounterIUI;
			int totalShaft = ShaftManager.Instance.Shafts.Count;
			if (shaftCount < totalShaft)
			{
				for (int i = shaftCount; i < totalShaft; i++)
					HandleShaftUI();
			}

			for (int i = 0; i < ShaftManager.Instance.Shafts.Count; i++)
			{
				ShaftManager.Instance.OnUpdateShaftInventoryUI?.Invoke(i);
			}
			HandleElevatorIUI();
			HandleCounterIUI();
		}
		private void OnDisable()
		{
			ShaftManager.Instance.OnUpdateShaftInventoryUI -= HanleUpdateShaftIUI;
			ElevatorSystem.Instance.OnUpdateElevatorInventoryUI -= HandleElevatorIUI;
			Counter.Instance.OnUpdateCounterInventoryUI -= HandleCounterIUI;
		}
		private void OnApplicationQuit()
		{
			gameObject.SetActive(false);
		}

		void Start()
        {
			isBackgroundItemOpening = false;
            tgNhanVien.onValueChanged.AddListener(delegate
            {
                SlideInContainer(pnNhanVien, tgNhanVien);
            });
            tgNoiThat.onValueChanged.AddListener(delegate
            {
                SlideInContainer(pnNoiThat, tgNoiThat);
            });

			foreach (var item in counterItem)
			{
				if (item.type == InventoryItemType.CounterBg)
				{
					
					item.OnItemClick += OpenListBg;
				}
				else
				{
					item.OnItemClick += PopupOrtherItemController;
				}
			}
			foreach (var item in elevatorItem)
			{
				if (item.type == InventoryItemType.ElevatorBg)
				{
					item.OnItemClick += OpenListBg;
				}
				else
				{
					item.OnItemClick += PopupOrtherItemController;
				}
			}

			
		}

		private void HanleUpdateShaftIUI(int index)
		{
			listShaftUI[index].UpdateShaftUI();
		}
		private void HandleElevatorIUI()
		{
			var elevatorSkinData = ElevatorSystem.Instance.elevatorSkin.GetDataSkin();
			foreach (var item in elevatorItem)
			{
				if(elevatorSkinData.ContainsKey(item.type))
				{
					Sprite bgImage = Resources.Load<Sprite>(elevatorSkinData[item.type].path);
					item.ChangeItem(bgImage);
				} 
			}
		}
		private void HandleCounterIUI()
		{
			var elevatorSkinData = Counter.Instance.counterSkin.GetDataSkin();
			foreach (var item in counterItem)
			{
				if (elevatorSkinData.ContainsKey(item.type))
				{
					Sprite bgImage = Resources.Load<Sprite>(elevatorSkinData[item.type].path);
					item.ChangeItem(bgImage);
				}
			}
		}
		public void SlideInContainer(GameObject panel, Toggle tg)
        {
			if (tg.isOn) tg.gameObject.GetComponent<ToggleBehaviour>().DoAnimate();
            panel.SetActive(tg.isOn);
            Vector2 posCam = CustomCamera.Instance.GetCurrentTransform().position;
            panel.transform.localPosition = new Vector2(posCam.x - 2000, panel.transform.localPosition.y);
            panel.transform.DOLocalMoveX(0, 0.6f).SetEase(Ease.OutElastic, 1, 1f);
			if(pnNhanVien.activeInHierarchy)
			{
				LoadListShaft();
			}
			else
			{
				UnLoadListShaft();
			}

		}
		public void LoadListShaft()
		{
			tgNhanVien.interactable = false;
			tgNoiThat.interactable = true;
			foreach (var item in listShaftStaffSkin)
			{
				item.gameObject.SetActive(false);
			}

			counterStaffSkin.OnItemClick += OpenStaffSkin;
			elevatorStaffSkin.OnItemClick += OpenStaffSkin;

			for (int i = 0; i < shaftCount; i++)
			{
				listShaftStaffSkin[i].gameObject.SetActive(true);
				listShaftStaffSkin[i].OnItemClick += OpenStaffSkin;
				listShaftStaffSkin[i].Index = i;
			}
		}
		public void UnLoadListShaft()
		{
			tgNhanVien.interactable = true;
			tgNoiThat.interactable = false;
			counterStaffSkin.OnItemClick -= OpenStaffSkin;
			elevatorStaffSkin.OnItemClick -= OpenStaffSkin;
			for (int i = 0; i < shaftCount; i++)
			{
				listShaftStaffSkin[i].OnItemClick -= OpenStaffSkin;
			}
		}
		private void OpenStaffSkin(InventoryItemType type, int index)
		{
			staffSkinUI.gameObject.SetActive(true);
			staffSkinUI.SetLeftRightIndex(0, 4);
		}

		public void CloseInvetoryUI()
        {
            if (isBackgroundItemOpening == false)
                gameObject.SetActive(false);
			if(isBackgroundItemOpening)
			{
				isBackgroundItemOpening = false;
				bgList.gameObject.SetActive(false);
				inventoryPanel.SetActive(true);
			}
        }
		private void OpenListBg(InventoryItemType type, int index = -1)
		{
			isBackgroundItemOpening = true;
			bgList.gameObject.SetActive(true);
			bgList.SetItemHandle(type, index);
			inventoryPanel.SetActive(false);

		}

		private void HandleShaftUI()
		{
			var shaft = Instantiate(shaftUIController, Vector3.zero, Quaternion.identity);
			shaft.transform.SetParent(shaftContent.transform, false);
			shaft.SetShaftIndex(shaftCount);
			foreach(var item in shaft.items)
			{
				if(item.type == InventoryItemType.ShaftBg || item.type == InventoryItemType.ShaftSecondBg)
				{
					item.OnItemClick += OpenListBg;
				}
				else
				{
					item.OnItemClick += PopupOrtherItemController;
				}
			}
			shaftCount++;
			listShaftUI.Add(shaft);
			
		}

		private void PopupOrtherItemController(InventoryItemType type, int index = -1)
		{
			if(TryGetComponent<InventoryUIStateMachine>(out var stateMachine))
			{
				stateMachine.TransitonToState(type);
				pOIController.gameObject.SetActive(true);
				pOIController.FloorIndex = index;
			}
		}
	}
}
