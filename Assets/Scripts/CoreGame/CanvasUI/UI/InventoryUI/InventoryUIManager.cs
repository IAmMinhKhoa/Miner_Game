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
		[SerializeField] HandleShaftStaffItem shaftStaffSkinItem;
		[SerializeField] GameObject content;
		List<ShaftUIController> listShaftUI = new();
		List<StaffSkinItem> listShaftStaffSkin;
		


		[Header("Counter")]
		[SerializeField] DecoratorItem[] counterItem;
		[SerializeField] StaffSkinItem counterStaffSkin;

		[Header("Elevator")]
		[SerializeField] DecoratorItem[] elevatorItem;
		[SerializeField] StaffSkinItem elevatorStaffSkin;
	

		int shaftCount = 0;
		bool isBackgroundItemOpening;
		bool isFirstTimeOpen = true;
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
			if(isFirstTimeOpen == false)
			{
				HandleElevatorIUI();
				HandleCounterIUI();
			}
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
			if(pnNhanVien.TryGetComponent<TabStaff>(out var tabStaff))
			{
				tabStaff.OnTabStaffEnable += UpdateStabStaffUI;
				tabStaff.OnTabStaffDisable += TabStaff_OnTabStaffDisable;
			}
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
			HandleElevatorIUI();
			HandleCounterIUI();
			isFirstTimeOpen = false;
		}

		private void TabStaff_OnTabStaffDisable()
		{
			counterStaffSkin.OnItemClick -= OpenStaffSkin;
			elevatorStaffSkin.OnItemClick -= OpenStaffSkin;
			for (int i = 0; i < shaftCount; i++)
			{
				listShaftStaffSkin[i].OnItemClick -= OpenStaffSkin;
			}
		}

		private void UpdateStabStaffUI()
		{

			counterStaffSkin.OnItemClick += OpenStaffSkin;
			elevatorStaffSkin.OnItemClick += OpenStaffSkin;
			listShaftStaffSkin ??= new();
			while(listShaftStaffSkin.Count < shaftCount)
			{
				var newShaftStaffSkin = Instantiate(shaftStaffSkinItem, content.transform);
				listShaftStaffSkin.Add(newShaftStaffSkin.item);
			}
			var counterStaff = Counter.Instance.counterSkin.character;
			if(counterStaffSkin.TryGetComponent<TabStaffItem>(out var counterItem))
			{
				counterItem.SetInfoItem(int.Parse(counterStaff.idHead), int.Parse(counterStaff.idBody), -1);
			}
			var elevatorStaff = ElevatorSystem.Instance.elevatorSkin.characterSkin;
			if (elevatorStaffSkin.TryGetComponent<TabStaffItem>(out var elevatorItem))
			{
				elevatorItem.SetInfoItem(int.Parse(elevatorStaff.idHead), int.Parse(elevatorStaff.idBody), -1);
			}
		
			for (int i = 0; i < shaftCount; i++)
			{
				listShaftStaffSkin[i].OnItemClick += OpenStaffSkin;
				listShaftStaffSkin[i].Index = i;
				var shaftStaff = ShaftManager.Instance.Shafts[i].shaftSkin.characterSkin;
				Debug.Log(shaftStaff.idHead + "----------------------------" + shaftStaff.idBody);
				if (listShaftStaffSkin[i].TryGetComponent<TabStaffItem>(out var shaftItem))
				{
					shaftItem.SetInfoItem(int.Parse(shaftStaff.idHead), int.Parse(shaftStaff.idBody), i);
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
				if (item.SkinList != null)
				{
					int indexCart = int.Parse(ElevatorSystem.Instance.elevatorSkin.idFrontElevator);
					item.ChangeSpineSkin(item.SkinList.Items[indexCart]);
				}
			}
		}
		private void HandleCounterIUI()
		{
			var counter = Counter.Instance.counterSkin.GetDataSkin();
			foreach (var item in counterItem)
			{
				if (counter.ContainsKey(item.type))
				{
					Sprite bgImage = Resources.Load<Sprite>(counter[item.type].path);
					item.ChangeItem(bgImage);
					continue;
				}
				if(item.SkinList != null)
				{
					int indexCart = int.Parse(Counter.Instance.counterSkin.idCart);
					item.ChangeSpineSkin(item.SkinList.Items[indexCart]);
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
			
		}
		public void UnLoadListShaft()
		{
			tgNhanVien.interactable = true;
			tgNoiThat.interactable = false;
			
		}
		private void OpenStaffSkin(InventoryItemType type, int index)
		{
			if (TryGetComponent<InventoryUIStateMachine>(out var stateMachine))
			{
				staffSkinUI.CurrentFloor = index;
				staffSkinUI.gameObject.SetActive(true);
				inventoryPanel.SetActive(false);
				stateMachine.TransitonToState(type);

			}
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
