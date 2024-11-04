using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Inventory
{
	public class CharacterSkinUI : MonoBehaviour
	{
		public event Action<int> OnItemClicked;
		[SerializeField]
		SkeletonGraphic spine;
		[SerializeField]
		Image hideImg;
		[SerializeField]
		Button clickButton;
		[SerializeField]
		SkeletonGraphic staticHead;
		[SerializeField]
		SkeletonGraphic staticBody;
		public SkeletonGraphic Spine => spine;
		private int _index;
		public int Index => _index;

		[SerializeField] Image border;
		[SerializeField] TextMeshProUGUI itemName;
		public void SetItemInfo(int indexSkin, InventoryItemType itType, bool isHeadSkin)
		{
			
			_index = indexSkin;

			
			staticHead.skeletonDataAsset = spine.skeletonDataAsset;
			staticHead.initialSkinName = "Head/Skin_" + 1;
			staticHead.Initialize(true);
			
			staticBody.skeletonDataAsset = spine.skeletonDataAsset;
			staticBody.initialSkinName = "Body/Skin_" + 1;
			staticBody.Initialize(true);
			Debug.Log(indexSkin + " -------------------------- " + itType);
			itemName.text = SkinManager.Instance.InfoSkinGame[itType][indexSkin].name;
			int idInInfo = SkinManager.Instance.ItemBought[itType].IndexOf((indexSkin+1).ToString());

			if(idInInfo == -1 && indexSkin != 0)
			{
				clickButton.interactable = false;
				hideImg.gameObject.SetActive(true);
			}
			
			staticHead.gameObject.SetActive(!isHeadSkin);
			staticBody.gameObject.SetActive(isHeadSkin);
			
		}
		public void SetScaleAndPos(CharScaleAndPos it)
		{
			var transform = spine.GetComponent<RectTransform>();
			transform.localScale = it.scale - new Vector3(0.02f, 0.02f, 0);
			transform.anchoredPosition = it.pos + new Vector3(0f, 4f, 0);
			var transformStaticHeadSpine = staticHead.GetComponent<RectTransform>();
			transformStaticHeadSpine.localScale = it.scale - new Vector3(0.02f, 0.02f, 0); 
			transformStaticHeadSpine.anchoredPosition =it.pos + new Vector3(0f, 4f, 0);
			var transformBodySpine = staticBody.GetComponent<RectTransform>();
			transformBodySpine.localScale = it.scale - new Vector3(0.02f, 0.02f, 0); 
			transformBodySpine.anchoredPosition = it.pos + new Vector3(0f, 4f, 0);
		}
		public void OnPointerClick()
		{
			OnItemClicked?.Invoke(_index);
		}
		public void Select()
		{
			border.gameObject.SetActive(true);
		}
		public void Unselect()
		{
			border.gameObject.SetActive(false);
		}

	}

	
}
