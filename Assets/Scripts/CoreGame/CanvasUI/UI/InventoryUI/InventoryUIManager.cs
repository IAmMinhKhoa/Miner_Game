using DG.Tweening;
using NOOD.Sound;
using PlayFab.EconomyModels;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UI.Inventory.PopupOtherItem;
using UnityEngine;
using UnityEngine.UI;


namespace UI.Inventory
{
    public class InventoryUIManager : MonoBehaviour
	{
		private Vector3 scale_tablet = new Vector3(1.2f, 1.2f, 1.2f);
		[Header("UI")]
		[SerializeField] private TextMeshProUGUI activeTextNoiThat;
		[SerializeField] private TextMeshProUGUI activeTextNhanVien;
        [SerializeField] private Toggle tgNoiThat;
        [SerializeField] private Toggle tgNhanVien;
        [SerializeField] private GameObject pnNoiThat;
        [SerializeField] private GameObject pnNhanVien;
        [SerializeField] GameObject inventoryPanel;
		public GameObject InventoryPanel => inventoryPanel;
		[SerializeField] BackGroundItemController bgList;
		public BackGroundItemController BGList => bgList;
		public PopupOtherItemController pOIController;
		[SerializeField] StaffSkinUI staffSkinUI;
		[SerializeField] private ContentFitterRefresh contentFitterRefresh;
		public StaffSkinUI StaffSkin => staffSkinUI;

		[Header("Shaft Item Handle")]
		[SerializeField]
		GameObject shaftContent;
		[SerializeField]
		ShaftUIController shaftUIController;
		[SerializeField] HandleShaftStaffItem shaftStaffSkinItem;
		[SerializeField] GameObject content;
		[SerializeField] private ContentFitterRefresh contentRefreshTabNV;
		List<ShaftUIController> listShaftUI = new();
		List<TabStaffItem> listShaftStaffSkin;
		


		[Header("Counter")]
		[SerializeField] DecoratorItem[] counterItem;
		[SerializeField] TabStaffItem counterStaffSkin;

		[Header("Elevator")]
		[SerializeField] DecoratorItem[] elevatorItem;
		[SerializeField] TabStaffItem elevatorStaffSkin;

		[Header("Prefab")]
		public BackGroundItem bgCounterPrefab;
		public Item popupOtherItemPrefab;

		int shaftCount = 0;

		bool isFirstTimeOpen = true;
		bool isUpdateCounterSkeletonData = false;
		bool isUpdateElevatorSkeletonData = false;
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
			for (int i = 0; i < listShaftUI.Count; i++)
			{
				listShaftUI[i].SetShaftIndex(i);
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
			if (Common.CheckDevice())
			{
				gameObject.transform.localScale = scale_tablet;
			}
			staffSkinUI.OnUpdateInventoryUI += UpdateStabStaffUI;
			if (pnNhanVien.TryGetComponent<TabStaff>(out var tabStaff))
			{
				tabStaff.OnTabStaffEnable += UpdateStabStaffUI;
				tabStaff.OnTabStaffDisable += TabStaff_OnTabStaffDisable;
			}
		
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
				if (item.type == InventoryItemType.CounterBg || item.type == InventoryItemType.CounterSecondBg)
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
				item.OnItemClick += PopupOrtherItemController;
			}
			HandleElevatorIUI();
			HandleCounterIUI();
			isFirstTimeOpen = false;
			contentFitterRefresh.RefreshContentFitters();
			if (pnNhanVien.activeInHierarchy)
			{
				activeTextNoiThat.gameObject.SetActive(false);
			}
			else
			{
				activeTextNhanVien.gameObject.SetActive(false);
			}
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
				listShaftStaffSkin[i].SetInfoItem(int.Parse(shaftStaff.idHead), int.Parse(shaftStaff.idBody), i);
			}
	
		}

		private void HanleUpdateShaftIUI(int index)
		{
			listShaftUI[index].UpdateShaftUI();
		}
		private void HandleElevatorIUI()
		{
			
			foreach (var item in elevatorItem)
			{
				if(isUpdateElevatorSkeletonData == false)
				{
					var data = SkinManager.Instance.SkinGameDataAsset.SkinGameData[item.type];
					item.Spine.skeletonDataAsset = data;
					item.Spine.Initialize(true);
				}
				if(item.type == InventoryItemType.ElevatorBg)
				{
					string skinName = "Icon_" + (int.Parse(ElevatorSystem.Instance.elevatorSkin.idBackGround) + 1);
					item.ChangeSpineSkin(skinName);
					continue;
				}
				if (item.SkinList != null)
				{
					int indexCart = int.Parse(ElevatorSystem.Instance.elevatorSkin.idFrontElevator);
					item.Spine.AnimationState.SetAnimation(0, "Icon", false);
					item.ChangeSpineSkin("Icon_" +(indexCart + 1));
				}
			}
			isUpdateElevatorSkeletonData = true;
		}
		private void HandleCounterIUI()
		{
			
			foreach (var item in counterItem)
			{
				if (isUpdateCounterSkeletonData == false)
				{
					var data = SkinManager.Instance.SkinGameDataAsset.SkinGameData[item.type];
					item.Spine.skeletonDataAsset = data;
					item.Spine.Initialize(true);
				}
				if (item.type == InventoryItemType.CounterBg)
				{
					string skinName = "Icon_" + (int.Parse(Counter.Instance.counterSkin.idBackGround) + 1);
					item.ChangeSpineSkin(skinName);
					continue;
				}
				if (item.type == InventoryItemType.CounterSecondBg)
				{
					string skinName = "Icon_" + (int.Parse(Counter.Instance.counterSkin.idSecondBg) + 1);
					item.ChangeSpineSkin(skinName);
					continue;
				}
				if (item.SkinList != null)
				{
					int indexCart = int.Parse(Counter.Instance.counterSkin.idCart);
					item.ChangeSpineSkin("Skin_" + (indexCart + 1));
				}
			}
			isUpdateCounterSkeletonData = true;
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
				activeTextNhanVien.gameObject.SetActive(true);
				activeTextNoiThat.gameObject.SetActive(false);
			}
			else
			{
				UnLoadListShaft();
				activeTextNhanVien.gameObject.SetActive(false);
				activeTextNoiThat.gameObject.SetActive(true);
			}
			contentRefreshTabNV.RefreshContentFitters();

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

	
		private void OpenListBg(InventoryItemType type, int index = -1)
		{
			bgList.SetIndex(index);
			if (TryGetComponent<InventoryUIStateMachine>(out var stateMachine))
			{
				stateMachine.TransitonToState(type);
				bgList.gameObject.SetActive(true);
				pOIController.FloorIndex = index;
				inventoryPanel.SetActive(false);
			}
			

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
			
			if (TryGetComponent<InventoryUIStateMachine>(out var stateMachine))
			{
				pOIController.gameObject.SetActive(true);
				pOIController.FloorIndex = index;
				stateMachine.TransitonToState(type);
				
			}
		}
		private void CloseInvetoryUI()
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
			gameObject.transform.DOLocalMoveX(0, 0.4f).SetEase(Ease.OutQuart);


		}
		[Button]
		public void FadeOutContainer()
		{
			SoundManager.PlaySound(SoundEnum.mobileTexting2);
			Vector2 posCam = CustomCamera.Instance.GetCurrentTransform().position;
			gameObject.transform.DOLocalMoveX(posCam.x - 2000f, 0.6f).SetEase(Ease.InQuart).OnComplete(() =>
			{
				CloseInvetoryUI();
			});

		}
		#endregion

	}
}
