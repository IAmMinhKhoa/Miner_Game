using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LongGachaItem : MonoBehaviour
{
	[SerializeField]
	SkeletonGraphic skeletonGraphic;
	[SerializeField]
	Image low;
	[SerializeField]
	Image normal;
	[SerializeField]
	Image super;
	[SerializeField]
	Image ultra;

	public bool InitialData(GachaItemInfor itemInfo, string SkinName)
	{
		switch (itemInfo.skinGachaInfor.quality)
		{
			case MarketPlayItemQuality.low:
				low.gameObject.SetActive(true);
				break;
			case MarketPlayItemQuality.normal:
				normal.gameObject.SetActive(true);
				break;
			case MarketPlayItemQuality.super:
				super.gameObject.SetActive(true);
				break;
			case MarketPlayItemQuality.ultra:
				ultra.gameObject.SetActive(true);
				break;
		}
		skeletonGraphic.skeletonDataAsset = SkinManager.Instance.SkinGameDataAsset.SkinGameData[itemInfo.type];

		if (skeletonGraphic.Skeleton.Data.FindSkin(SkinName) == null) return false;

		skeletonGraphic.initialSkinName = SkinName;
		skeletonGraphic.Initialize(true);

		return true;
	}
}
