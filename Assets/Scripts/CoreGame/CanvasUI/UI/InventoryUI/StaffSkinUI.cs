using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

namespace UI.Inventory
{
    public class StaffSkinUI : MonoBehaviour
    {
		public List<CharacterSkinUI> headCharacter;
		public List<CharacterSkinUI> bodyCharacter;
		[SerializeField] GameObject priviousHead;
		[SerializeField] GameObject nextHead;
		[SerializeField] GameObject priviousBody;
		[SerializeField] GameObject nextBody;
	
		//so luong skin
		
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
				UpdateListHeadCharacter();
			}
			
		}

		private int MaxHeadSkinAmount;

		private int _rightHeadIndex;
		public int RightHeadIndex
		{
			get => _rightHeadIndex;
			private set
			{
				
				_rightHeadIndex = value;
				if(_rightHeadIndex == MaxHeadSkinAmount)
				{
					nextHead.SetActive(false);
				}
				else
				{
					nextHead.SetActive(true);
				}
				UpdateListHeadCharacter();
			}
		}
		private int leftBodyIndex;
		private int rightBodyIndex;

		private int currentHeadIndex;
		private int currentBodyIndex;
		
		public void SetLeftRightIndex(int left = 0, int right = 4)
		{
			LeftHeadIndex = left;
			MaxHeadSkinAmount = right;
			RightHeadIndex = Math.Min(4, right);
		}
		private void UpdateListHeadCharacter()
		{
			for (int i = LeftHeadIndex; i < RightHeadIndex; i++)
			{
				headCharacter[i].SetItemInfo(null, i.ToString());
				headCharacter[i].OnItemClicked += SetCurHeadIndex;
			}
		}
		private void SetCurHeadIndex(int index)
		{
			currentHeadIndex = index;
			Debug.Log(index);
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
			gameObject.SetActive(false);
		}
    }
}
