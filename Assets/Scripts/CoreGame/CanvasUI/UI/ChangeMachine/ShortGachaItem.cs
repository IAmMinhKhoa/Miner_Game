using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortGachaItem : MonoBehaviour
{
	[SerializeField]
	SkeletonGraphic skeletonGraphic;

	public void InitialData(GachaItemInfor itemInfo, string SkinName)
	{
		skeletonGraphic.skeletonDataAsset = SkinManager.Instance.SkinGameDataAsset.SkinGameData[itemInfo.type];
		skeletonGraphic.initialSkinName = SkinName;

		skeletonGraphic.Initialize(true);
		var iconAnimation = skeletonGraphic.skeletonDataAsset.GetSkeletonData(false).FindAnimation("Icon");
		if (iconAnimation != null)
		{
			skeletonGraphic.AnimationState.SetAnimation(0, "Icon", false);
		}
		else
		{
			skeletonGraphic.AnimationState.ClearTrack(0);
		}

	}
}
