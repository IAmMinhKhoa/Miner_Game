using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.UIElements;

namespace UI.Inventory
{
    public class StaffSkinUI : MonoBehaviour
    {
		
		List<CharacterSkinUI> headCharacter;
		List<CharacterSkinUI> bodyCharacter;

		[Header("Skin review")]
		[SerializeField] GameObject cartModel;
		[SerializeField] TypeSpine elevatorHeadModel;
		[SerializeField] TypeSpine elevatorBodyModel;
		[SerializeField] TypeSpine shaftHeadModel;
		[SerializeField] TypeSpine shaftBodyModel;
		[SerializeField] TypeSpine shaftTailModel;
	
		public TypeSpine ElevatorHeadModel => elevatorHeadModel;
		public TypeSpine ElevatorHeadModelBodyModel => elevatorBodyModel;
		public TypeSpine ShaftHeadModel => shaftHeadModel;
		public TypeSpine ShaftBodyModel => shaftBodyModel;
		public TypeSpine ShaftTailModel => shaftTailModel;

		public event Action<int, int> OnConfirmButtonClick;
		[Header("Next Preivous button")]
		[SerializeField] GameObject priviousHead;
		[SerializeField] GameObject nextHead;
		[SerializeField] GameObject priviousBody;
		[SerializeField] GameObject nextBody;
		public GameObject selectFloor;
		[Header("Content")]
		[SerializeField] GameObject headContent;
		[SerializeField] GameObject bodyContent; 

		[Header("Inventory Panel")]
		[SerializeField] GameObject inventoryPanel;
		[SerializeField] SelectFloorHandle selectFloorHandle;

		[Header("Prefab")]
		[SerializeField] CharacterSkinUI characterSkinUI;


		public event Action OnUpdateInventoryUI;
		public SelectFloorHandle SelectFloorHandle => selectFloorHandle;
		private InventoryItemType _currentItemTypeHandle;

		private int _currentFloor;
		public int CurrentFloor
		{
			set
			{
				_currentFloor = value;
			}
			get => _currentFloor;
		}
		public InventoryItemType CurrentItemTypeHandle {
			set
			{
				_currentItemTypeHandle = value;
				//check if current skin is elevator -> only can change typeAnimal
				if (value == InventoryItemType.ElevatorCharacter)
				{
					shaftBodyModel.gameObject.SetActive(false);
					shaftHeadModel.gameObject.SetActive(false);
					shaftTailModel.gameObject.SetActive(false);
					cartModel.SetActive(false);

					elevatorHeadModel.gameObject.SetActive(true);
					elevatorBodyModel.gameObject.SetActive(true);
				}
				else
				{
					shaftBodyModel.gameObject.SetActive(true);
					shaftHeadModel.gameObject.SetActive(true);
					shaftTailModel.gameObject.SetActive(true);
					cartModel.SetActive(true);

					if(value == InventoryItemType.CounterCharacter)
					{
						var spine = cartModel.GetComponent<SkeletonGraphic>();
						string skinName = "Xe day " + (int.Parse(Counter.Instance.counterSkin.idCart) + 1);
						spine.Skeleton.SetSkin(skinName);
						spine.Skeleton.SetSlotsToSetupPose();
					}
					else
					{
						var spine = cartModel.GetComponent<SkeletonGraphic>();
						string skinName = "Xe day " + (int.Parse(ShaftManager.Instance.Shafts[CurrentFloor].shaftSkin.idCart) + 1);
						spine.Skeleton.SetSkin(skinName);
						spine.Skeleton.SetSlotsToSetupPose();
					}

					elevatorHeadModel.gameObject.SetActive(false);
					elevatorBodyModel.gameObject.SetActive(false);
				}
			}
			get
			{
				return _currentItemTypeHandle;
			}
		}

		//so luong skin

		private int MaxHeadSkinAmount;
		private int MaxBodySkinAmount;
		
		private int currentHeadIndex;
		private int currentBodyIndex;

		public int CurrentHeadIndex => currentHeadIndex;
		public int CurrentBodyIndex => currentBodyIndex;
	
		public void SetCurentHeadBodyIndex(int headIndex, int bodyIndex)
		{
			currentBodyIndex = bodyIndex;
			currentHeadIndex = headIndex;
			UpdateBodyModel();
			UpdateHeadModel();
		}
		public void SetHeadIndex(int skinAmount, SkeletonDataAsset skeletonData, string initialSkinName, Vector3 scale, Vector2 pos)
		{
			if (headCharacter == null) headCharacter = new();
			else headCharacter.Clear();

			var skeletonSkin = characterSkinUI.Spine.GetComponent<SkeletonGraphic>();
			skeletonSkin.skeletonDataAsset = skeletonData;
			skeletonSkin.initialSkinName = initialSkinName + 1;
			skeletonSkin.Initialize(true);

			for (int i = 0; i < skinAmount; i++)
			{
				
				var item = Instantiate(characterSkinUI, headContent.transform);
				headCharacter.Add(item);
				item.SetItemInfo(i);
				item.Spine.Skeleton.SetSkin(initialSkinName + (i + 1));
				item.Spine.Skeleton.SetSlotsToSetupPose();
				item.Spine.transform.localScale = scale;
				item.Spine.GetComponent<RectTransform>().anchoredPosition = pos;
				item.OnItemClicked += SetCurHeadIndex;

			}

		}
		public void SetBodyIndex(int skinAmount, SkeletonDataAsset skeletonData, string initialSkinName, Vector3 scale, Vector2 pos)
		{
			if (bodyCharacter == null) bodyCharacter = new();
			else bodyCharacter.Clear();

			var skeletonSkin = characterSkinUI.Spine.GetComponent<SkeletonGraphic>();
			skeletonSkin.skeletonDataAsset = skeletonData;
			skeletonSkin.initialSkinName = initialSkinName + 1;
			skeletonSkin.Initialize(true);

			for (int i = 0; i < skinAmount; i++)
			{
				var item = Instantiate(characterSkinUI, bodyContent.transform);
				bodyCharacter.Add(item);
				item.SetItemInfo(i);
				item.Spine.Skeleton.SetSkin(initialSkinName + (i + 1));
				item.Spine.Skeleton.SetSlotsToSetupPose();
				item.Spine.transform.localScale = scale;
				item.Spine.GetComponent<RectTransform>().anchoredPosition = pos;
				item.OnItemClicked += SetCurBodyIndex;
			}

		}
	

		private void SetCurBodyIndex(int index)
		{
			currentBodyIndex = index;
			for (int i = 0; i < bodyCharacter.Count; i++)
			{
				
				if(currentBodyIndex != bodyCharacter[i].Index) bodyCharacter[i].Unselect();
				else bodyCharacter[i].Select();	
			}
			UpdateBodyModel();
		}

		private void SetCurHeadIndex(int index)
		{
			currentHeadIndex = index;
			for (int i = 0; i < headCharacter.Count; i++)
			{
				if (currentHeadIndex != headCharacter[i].Index) headCharacter[i].Unselect();
				else headCharacter[i].Select();
			}
			UpdateHeadModel();
		}

		private void UpdateHeadModel()
		{
			if(CurrentItemTypeHandle == InventoryItemType.ElevatorCharacter)
			{
				var spine =	elevatorHeadModel.GetComponent<SkeletonGraphic>();
				spine.Skeleton.SetSkin("Head/Skin_" + (currentHeadIndex + 1));
				spine.Skeleton.SetSlotsToSetupPose();
			}
			else
			{
				var headSpine = shaftHeadModel.GetComponent<SkeletonGraphic>();
				var tailSpine = shaftTailModel.GetComponent<SkeletonGraphic>();

				headSpine.Skeleton.SetSkin("Head/Skin_" + (currentHeadIndex + 1));
				headSpine.Skeleton.SetSlotsToSetupPose();

				tailSpine.gameObject.SetActive(true);
				if(tailSpine.Skeleton.Data.FindSkin("Tail/Skin_"+(currentHeadIndex + 1)) != null)
				{
					tailSpine.Skeleton.SetSkin("Tail/Skin_" + (currentHeadIndex + 1));
					tailSpine.Skeleton.SetSlotsToSetupPose();
					return;
				}
				tailSpine.gameObject.SetActive(false);
			}
		}
		private void UpdateBodyModel()
		{
			if (CurrentItemTypeHandle == InventoryItemType.ElevatorCharacter)
			{
				var spine = elevatorBodyModel.GetComponent<SkeletonGraphic>();
				spine.Skeleton.SetSkin("Body/Skin_" + (currentBodyIndex + 1));
				spine.Skeleton.SetSlotsToSetupPose();
			}
			else
			{
				var headSpine = shaftBodyModel.GetComponent<SkeletonGraphic>();
				headSpine.Skeleton.SetSkin("Body/Skin_" + (currentBodyIndex + 1));
				headSpine.Skeleton.SetSlotsToSetupPose();
			}
		}
		public void ConfirmButtonCLick()
		{
			OnUpdateInventoryUI?.Invoke();
			OnConfirmButtonClick?.Invoke(currentHeadIndex, currentBodyIndex);
			CloseUI();
			
		}
		public void ClearEvent()
		{
			for (int i = 0; i < 3; i++)
			{
				headCharacter[i].OnItemClicked -= SetCurHeadIndex;
				bodyCharacter[i].OnItemClicked -= SetCurBodyIndex;
			}
		}
		public void DestroyObject()
		{
			if(headCharacter != null)
			{
				foreach (var item in headCharacter)
				{
					Destroy(item.gameObject);
				}
			}
			
			if(bodyCharacter != null)
			{
				foreach (var item in bodyCharacter)
				{
					Destroy(item.gameObject);
				}
			}
		}
		public void CloseUI()
		{
		
			inventoryPanel.SetActive(true);	
			gameObject.SetActive(false);
		}
		public void OpenSelecFloorHandle()
		{
			selectFloorHandle.gameObject.SetActive(true);
			gameObject.SetActive(false);
		}
    }
}
