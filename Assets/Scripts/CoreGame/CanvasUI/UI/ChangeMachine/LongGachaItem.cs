using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongGachaItem : MonoBehaviour
{
	[SerializeField]
	SkeletonGraphic skeletonGraphic;

	public void InitialData(GachaItemInfor itemInfor, string SkinName)
	{
		skeletonGraphic.skeletonDataAsset = SkinManager.Instance.SkinGameDataAsset.SkinGameData[itemInfor.type];
		skeletonGraphic.initialSkinName = SkinName;
		skeletonGraphic.Initialize(true);
	}
}
