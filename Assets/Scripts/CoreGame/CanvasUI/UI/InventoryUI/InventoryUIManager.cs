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

			for (int i = 0; i < listShaftStaffSkin.Count; i++)
			{
				if (listShaftStaffSkin[i].isActiveAndEnabled)
				{
					listShaftStaffSkin[i].gameObject.SetActive(false);
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
			counterStaffSkin.OnItemClick += OpenStaffSkin;
			elevatorStaffSkin.OnItemClick += OpenStaffSkin;

		}
		//public void LoadListShaft()
		//{
		//	Debug.Log("-9-9-9999123123-----");
		//	tgNoiThat.interactable = true;
		//	tgNhanVien.interactable = false;
		//	counterStaffSkin.OnItemClick += OpenStaffSkin;
		//	elevatorStaffSkin.OnItemClick += OpenStaffSkin;
			
		//	for (int i = 0; i < shaftCount; i++)
		//	{
				
		//	}
		//}
		//public void UnLoadListShaft()
		//{
		//	tgNoiThat.interactable = false;
		//	tgNhanVien.interactable = true;
		//	counterStaffSkin.OnItemClick -= OpenStaffSkin;
		//	elevatorStaffSkin.OnItemClick -= OpenStaffSkin;
		//	for (int i = 0; i < shaftCount; i++)
		//	{
		//		listShaftStaffSkin[i].OnItemClick -= OpenStaffSkin;
		//	}
		//}
		private void OpenStaffSkin(InventoryItemType type, int index)
		{
			Debug.Log("-9-9-9999123123-----");
			staffSkinUI.gameObject.SetActive(true);
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
			listShaftStaffSkin[shaftCount].gameObject.SetActive(true);
			listShaftStaffSkin[shaftCount].Index = shaftCount;
			listShaftStaffSkin[shaftCount].OnItemClick += OpenStaffSkin;
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
