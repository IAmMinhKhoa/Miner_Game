using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SelecFloorItem : MonoBehaviour
{
	[SerializeField] SkeletonGraphic head;
	[SerializeField] SkeletonGraphic body;
	[SerializeField] TextMeshProUGUI floorText;

	public event Action OnItemSlect;

	public void SetInfoItem(int headIndex, int bodyIndex, int floor)
	{
		var headSkin = head.Skeleton.Data.Skins.Items[headIndex];
		var bodySkin = body.Skeleton.Data.Skins.Items[bodyIndex];

		if(floor != -1)
			floorText.text = "Tầng " + floor;

		head.Skeleton.SetSkin(headSkin);
		body.Skeleton.SetSkin(bodySkin);

		head.Skeleton.SetSlotsToSetupPose();
		body.Skeleton.SetSlotsToSetupPose();
	}
	public void ItemIsSelected()
	{
		OnItemSlect?.Invoke();
	}
}
