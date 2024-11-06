using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaffGachaItem : MonoBehaviour
{
	[SerializeField]
	SkeletonGraphic head;
	[SerializeField]
	SkeletonGraphic body;
	[SerializeField]
	Image low;
	[SerializeField]
	Image normal;
	[SerializeField]
	Image super;
	[SerializeField]
	Image ultra;
	public void InitialData(GachaItemInfor itemInfo)
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

		head.skeletonDataAsset = SkinManager.Instance.SkinGameDataAsset.SkinGameData[itemInfo.type];
		head.initialSkinName = "Head/Skin_" + itemInfo.skinGachaInfor.ID;
		head.Initialize(true);
		
		var spineHead = head.GetComponent<RectTransform>();
		spineHead.localScale = itemInfo.skinGachaInfor.Scale;
		spineHead.anchoredPosition = itemInfo.skinGachaInfor.Positon;

		body.skeletonDataAsset = SkinManager.Instance.SkinGameDataAsset.SkinGameData[itemInfo.type];
		body.initialSkinName = "Body/Skin_" + itemInfo.skinGachaInfor.ID;
		body.Initialize(true);

		var spineBody = body.GetComponent<RectTransform>();
		spineBody.localScale = itemInfo.skinGachaInfor.Scale;
		spineBody.anchoredPosition = itemInfo.skinGachaInfor.Positon;

	}
}
