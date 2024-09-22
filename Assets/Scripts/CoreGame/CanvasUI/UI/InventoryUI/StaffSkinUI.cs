using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Inventory
{
    public class StaffSkinUI : MonoBehaviour
    {
		[Header("List skin")]
		public List<CharacterSkinUI> headCharacter;
		public List<CharacterSkinUI> bodyCharacter;

		[Header("Skin review")]
		[SerializeField] List<TypeSpine> headModel;
		[SerializeField] List<TypeSpine> bodyModel;

		public event Action<int, int> OnConfirmButtonClick;
		[Header("Next Preivous button")]
		[SerializeField] GameObject priviousHead;
		[SerializeField] GameObject nextHead;
		[SerializeField] GameObject priviousBody;
		[SerializeField] GameObject nextBody;
		public GameObject selectFloor;

		[Header("Inventory Panel")]
		[SerializeField] GameObject inventoryPanel;
		[SerializeField] SelectFloorHandle selectFloorHandle;

		public SelectFloorHandle SelectFloorHandle => selectFloorHandle;

		public InventoryItemType CurrentItemTypeHandle { set; get; }

		private int _currentFloor;
		public int CurrentFloor
		{
			set
			{
				_currentFloor = value;
			}
			get => _currentFloor;
		}
		//so luong skin

		private int MaxHeadSkinAmount;
		private int MaxBodySkinAmount;
		//int bodySkinAmount = ShaftManager.Instance.Shafts[0].Brewers[0].BodySkeletonAnimation.skeleton.Data.Skins.Count;

		//hien thi cac skin o vi tri tu left den right
		private int _leftHeadIndex;

		public int LeftHeadIndex
		{
			get
			{
				return _leftHeadIndex;
			}
			private set
			{
				_leftHeadIndex = value;
				if (value > 1)
				{
					priviousHead.SetActive(true);
				}
				else
				{
					priviousHead.SetActive(false);
				}
				if(isSetHeadIndex == false)
					UpdateListHeadCharacter(CurrentItemTypeHandle);
				else isSetHeadIndex = false;
			}
			
		}

		

		private int _rightHeadIndex;
		public int RightHeadIndex
		{
			get => _rightHeadIndex;
			private set
			{
				
				_rightHeadIndex = value;
				if(_rightHeadIndex >= MaxHeadSkinAmount)
				{
					nextHead.SetActive(false);
				}
				else
				{
					nextHead.SetActive(true);
				}
				UpdateListHeadCharacter(CurrentItemTypeHandle);
			}
		}
		private int _leftBodyIndex;
		public int LeftBodyIndex
		{
			get
			{
				return _leftBodyIndex;
			}
			private set
			{
				_leftBodyIndex = value;
				if (value > 1)
				{
					priviousBody.SetActive(true);
				}
				else
				{
					priviousBody.SetActive(false);
				}
				if (isSetBodyIndex == false)
					UpdateListBodyCharacter(CurrentItemTypeHandle);
				else isSetBodyIndex = false;
			}

		}
		private int _rightBodyIndex;
		public int RightBodyIndex
		{
			get => _rightBodyIndex;
			private set
			{

				_rightBodyIndex = value;
				if (_rightBodyIndex >= MaxBodySkinAmount)
				{
					nextBody.SetActive(false);
				}
				else
				{
					nextBody.SetActive(true);
				}
				UpdateListBodyCharacter(CurrentItemTypeHandle);
			}
		}

		private int currentHeadIndex;
		private int currentBodyIndex;

		public int CurrentHeadIndex => currentHeadIndex;
		public int CurrentBodyIndex => currentBodyIndex;
		private bool isSetHeadIndex = true;
		private bool isSetBodyIndex = true;
		public void SetCurentHeadBodyIndex(int headIndex, int bodyIndex)
		{
			currentBodyIndex = bodyIndex;
			currentHeadIndex = headIndex;
			UpdateBodyModel();
			UpdateHeadModel();
		}
		public void SetHeadIndex( int skinAmount)
		{
			foreach (var item in headCharacter)
			{
				item.gameObject.SetActive(false);
			}
			LeftHeadIndex = 0;
			MaxHeadSkinAmount = skinAmount;
			RightHeadIndex = Math.Min(4, skinAmount);
		}
		public void SetBodyIndex(int skinAmount)
		{
			foreach (var item in bodyCharacter)
			{
				item.gameObject.SetActive(false);
			}
			LeftBodyIndex = 0;
			MaxBodySkinAmount = skinAmount;
			RightBodyIndex = Math.Min(4, skinAmount);
			

		}
		private void UpdateListHeadCharacter(InventoryItemType type)
		{
			for (int i = 0; i < Math.Min(RightHeadIndex, 4); i++)
			{
				headCharacter[i].gameObject.SetActive(true);
				headCharacter[i].Unselect();
				headCharacter[i].SetItemInfo(LeftHeadIndex + i, type);
				headCharacter[i].OnItemClicked += SetCurHeadIndex;
			}
		}
		private void UpdateListBodyCharacter(InventoryItemType type)
		{
			for (int i = 0; i < Math.Min(RightBodyIndex, 4); i++)
			{
				bodyCharacter[i].gameObject.SetActive(true);
				bodyCharacter[i].Unselect();
				bodyCharacter[i].SetItemInfo(LeftBodyIndex + i, type);
				bodyCharacter[i].OnItemClicked += SetCurBodyIndex;
			}
		}

		private void SetCurBodyIndex(int index)
		{
			currentBodyIndex = index;
			for (int i = 0; i < bodyCharacter.Count; i++)
			{
				if(i != index) bodyCharacter[i].Unselect();
				else bodyCharacter[i].Select();	
			}
			
			UpdateBodyModel();
		}

		private void SetCurHeadIndex(int index)
		{
			currentHeadIndex = index;
			for (int i = 0; i < headCharacter.Count; i++)
			{
				if (i != index) headCharacter[i].Unselect();
				else headCharacter[i].Select();
			}
			UpdateHeadModel();
		}

		private void UpdateHeadModel()
		{
			foreach (var item in headModel)
			{
				if(item.Type == CurrentItemTypeHandle)
				{
					item.gameObject.SetActive(true);
					if(item.TryGetComponent<SkeletonGraphic>(out var skeletonGraphic))
					{
						var skin = skeletonGraphic.Skeleton.Data.Skins.Items[currentHeadIndex];
						skeletonGraphic.Skeleton.SetSkin(skin);
						skeletonGraphic.Skeleton.SetSlotsToSetupPose();
					}

				}
				else
				{
					item.gameObject.SetActive(false);
				}
			}
		}
		private void UpdateBodyModel()
		{
			foreach (var item in bodyModel)
			{
				if (item.Type == CurrentItemTypeHandle)
				{
					item.gameObject.SetActive(true);
					if (item.TryGetComponent<SkeletonGraphic>(out var skeletonGraphic))
					{
						var skin = skeletonGraphic.Skeleton.Data.Skins.Items[currentBodyIndex];
						skeletonGraphic.Skeleton.SetSkin(skin);
						skeletonGraphic.Skeleton.SetSlotsToSetupPose();
					}
				}
				else
				{
					item.gameObject.SetActive(false);
				}
			}
		}
		public void ConfirmButtonCLick()
		{
			OnConfirmButtonClick?.Invoke(currentHeadIndex, currentBodyIndex);
		}
		public void ClearEvent()
		{
			for (int i = LeftHeadIndex; i < RightHeadIndex; i++)
			{
				headCharacter[i].OnItemClicked -= SetCurHeadIndex;
			}
		}
		public void CloseUI()
		{
			isSetHeadIndex = true;
			isSetBodyIndex = true;
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
