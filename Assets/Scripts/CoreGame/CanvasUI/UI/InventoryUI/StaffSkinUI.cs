using System;
using System.Collections;
using System.Collections.Generic;
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

		

		private int _rightHeadIndex;
		public int RightHeadIndex
		{
			get => _rightHeadIndex;
			private set
			{
				int headSkinAmount = ShaftManager.Instance.Shafts[0].Brewers[0].HeadSkeletonAnimation.skeleton.Data.Skins.Count;
				_rightHeadIndex = value;
				if(value == headSkinAmount - 1)
				{
					nextBody.SetActive(false);
				}
				else
				{
					nextBody.SetActive(true);
				}
			}
		}
		private int leftBodyIndex;
		private int rightBodyIndex;

		private int currentHeadIndex;
		private int currentBodyIndex;
		private void OnEnable()
		{
			
			LeftHeadIndex = 0;
			leftBodyIndex = 0;
			//int headSkinAmount = ShaftManager.Instance.Shafts[0].Brewers[0].HeadSkeletonAnimation.skeleton.Data.Skins.Count;
			//RightHeadIndex = Math.Min(4, headSkinAmount);
			//rightBodyIndex = Math.Min(4, bodySkinAmount);
			


		}
		private void UpdateListHeadCharacter()
		{
			for (int i = LeftHeadIndex; i < RightHeadIndex; i++)
			{
				headCharacter[i].SetItemInfo(null, i.ToString());
			}
		}
		public void CloseUI()
		{
			gameObject.SetActive(false);
		}
    }
}
