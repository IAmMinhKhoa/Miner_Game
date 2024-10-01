using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SelecFloorItem : MonoBehaviour
{
	[SerializeField] protected SkeletonGraphic head;
	[SerializeField] protected SkeletonGraphic body;
	[SerializeField] protected TextMeshProUGUI floorText;

	public event Action OnItemSlect;

	public virtual void SetInfoItem(int headIndex, int bodyIndex, int floor)
	{
		

		if(floor != -1)
			floorText.text = "Tầng " + floor;

		head.Skeleton.SetSkin("Head/Skin_"+(headIndex + 1));
		body.Skeleton.SetSkin("Body/Skin_" + (bodyIndex + 1));

		head.Skeleton.SetSlotsToSetupPose();
		body.Skeleton.SetSlotsToSetupPose();
	}
	public void ItemIsSelected()
	{
		OnItemSlect?.Invoke();
	}
}
