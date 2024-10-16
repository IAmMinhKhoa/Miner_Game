using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Skin Data", menuName = "ScriptableObjects/SkinData")]
public class SkinDataSO : ScriptableObject
{
	[SerializeField]
	List<SkinGameSkeletonDataAsset> skeletonDataAssets;
	

	Dictionary<InventoryItemType, SkeletonDataAsset> _skinGameData;
	public Dictionary<InventoryItemType, SkeletonDataAsset> SkinGameData {
		get {
			
			_skinGameData ??= new();
			foreach (var item in skeletonDataAssets)
			{
				_skinGameData[item.skinType] =  item.data;
			}
			return _skinGameData;
		}
	}
}

[Serializable]
public struct SkinGameSkeletonDataAsset
{
	public InventoryItemType skinType;
	public SkeletonDataAsset data;
}
