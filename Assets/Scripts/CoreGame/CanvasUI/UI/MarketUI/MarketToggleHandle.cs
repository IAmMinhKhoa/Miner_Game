using System;
using System.Collections;
using System.Collections.Generic;
using NOOD.Sound;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;

public class MarketToggleHandle : MonoBehaviour
{
	Toggle toggleHandling;
	public InventoryItemType itemType;
	public SkeletonGraphic spine;
	public event Action<InventoryItemType> OnTabulationClick;
	void Start()
    {
        toggleHandling = GetComponent<Toggle>();
		toggleHandling.onValueChanged.AddListener(isOn => OnToggleValueChanged(isOn, itemType));
		if (Common.IsTablet)
		{
			RectTransform rectTransform = GetComponent<RectTransform>();
			Vector2 currentPivot = rectTransform.pivot;

			// Đổi giá trị x thành 0 (y giữ nguyên)
			rectTransform.pivot = new Vector2(0, currentPivot.y);
		}
    }

	private void OnToggleValueChanged(bool isOn, InventoryItemType itemType)
	{
		SoundManager.PlaySound(SoundEnum.mobileClickBack);
		if (isOn)
		{
			if(itemType != InventoryItemType.Null)
			{
				OnTabulationClick?.Invoke(itemType);
			}

			if (spine != null)
			{
				spine.AnimationState.SetAnimation(0, "Active", true);  // Thay đổi animation của track 0 sang "active"
			}

		}
		else
		{
			if (spine != null)
			{
				spine.AnimationState.SetAnimation(0, "Idle", true);
			}

		}
	}
}
