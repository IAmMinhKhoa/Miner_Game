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
	public class CharacterSkinUI : MonoBehaviour, IPointerClickHandler
	{
		public event Action<int> OnItemClicked;
		public List<TypeSpine> listSpine;
		private int _index;
		public int Index => _index;
		
		[SerializeField] Image BG;
		[SerializeField] Image border;
		[SerializeField] TextMeshProUGUI itemName;
		public void SetItemInfo(int indexSkin, InventoryItemType type, bool isHeadSkin)
		{
			_index = indexSkin;
			foreach (var spine in listSpine)
			{
				if(spine.Type == type)
				{
					spine.gameObject.SetActive(true);
					if (spine.TryGetComponent<SkeletonGraphic>(out SkeletonGraphic skeletonGraphic))
					{
						if(type == InventoryItemType.ElevatorCharacter)
						{
							
							if (skeletonGraphic.SkeletonDataAsset == null)
							{
								if(isHeadSkin)
								{
									SkeletonDataAsset dataAsset = ElevatorSystem
									.Instance
									.ElevatorController
									.GetComponent<ElevatorControllerView>()
									.ElevatorHeadStaff
									.SkeletonDataAsset;
									var skeleton = skeletonGraphic.GetComponent<SkeletonGraphic>();
									skeleton.initialSkinName = "Head/Skin_" + (indexSkin + 1);
									skeleton.startingAnimation = "";
									skeleton.Initialize(true);
									skeleton.skeletonDataAsset = dataAsset;
									skeleton.Initialize(true);
								}
								else
								{
									SkeletonDataAsset dataAsset = ElevatorSystem
										.Instance
										.ElevatorController
										.GetComponent<ElevatorControllerView>()
										.ElevatorBodyStaff
										.SkeletonDataAsset;
									var skeleton = skeletonGraphic.GetComponent<SkeletonGraphic>();
									skeleton.initialSkinName = "Body/Skin_" + (indexSkin + 1);
									skeleton.startingAnimation = "Idle_Corgi";
									skeleton.Initialize(true);
									skeleton.skeletonDataAsset = dataAsset;
									skeleton.Initialize(true);
								}
								
							}
						}
						else 
						{
							if(isHeadSkin)
							{
								skeletonGraphic.Skeleton.SetSkin("Head/Skin_" + (indexSkin + 1));
								skeletonGraphic.Skeleton.SetSlotsToSetupPose();
							}
							else
							{
								skeletonGraphic.Skeleton.SetSkin("Body/Skin_" + (indexSkin + 1));
								skeletonGraphic.Skeleton.SetSlotsToSetupPose();
							}
						}
	
						
					}
				}
				else
				{
					spine.gameObject.SetActive(false);
				}
			}
			itemName.text = indexSkin.ToString();
		}
		public void OnPointerClick(PointerEventData eventData)
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
