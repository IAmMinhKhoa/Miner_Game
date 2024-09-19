using System.Collections;
using System.Collections.Generic;
using NOOD.SerializableDictionary;
using Spine.Unity;
using UnityEngine;



	#region ENTITY
	[System.Serializable]
	public class DataSkinBase
	{
		public string id;
		public string name;
		public string path; // Dùng chung cho cả Image và Spine
		public string desc;
	}

	[System.Serializable]
	public class DataSkinImage : DataSkinBase
	{
		[System.NonSerialized] public Sprite sprite;  // Thuộc tính riêng để chứa Sprite
	}

	[System.Serializable]
	public class DataSkinSpine : DataSkinBase
	{
		[System.NonSerialized] public SkeletonDataAsset skeletonData;  // Thuộc tính riêng để chứa Spine data
	}

	[System.Serializable]
	public class SkinResource
	{
		public List<DataSkinImage> skinBgShaft;
		public List<DataSkinImage> skinBgCounter;
		public List<DataSkinImage> skinBgElevator;
		public List<DataSkinImage> skinSecondBgShaft;
		public List<DataSkinImage> skinWaitTable;
	/*public List<DataSkinImage> skinMilkCup;

	public List<DataSkinSpine> skinCharacterHead;
	public List<DataSkinSpine> skinCharacterBody;
	public List<DataSkinSpine> skinCharacterCart;*/
}
#endregion







