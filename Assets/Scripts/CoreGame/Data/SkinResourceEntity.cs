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
		public string desc;
		public string quality;
		public string cost;
	}


	[System.Serializable]
	public class SkinResource
	{
		public List<DataSkinBase> skinBgShaft;
		public List<DataSkinBase> skinBgCounter;
		public List<DataSkinBase> skinBgElevator;
		public List<DataSkinBase> skinSecondBgShaft;
		public List<DataSkinBase> skinWaitTable;
		public List<DataSkinBase> skinElevator;
		public List<DataSkinBase> skinCartCounter;
		public List<DataSkinBase> skinCartShaft;

	/*public List<DataSkinImage> skinMilkCup;

	public List<DataSkinSpine> skinCharacterHead;
	public List<DataSkinSpine> skinCharacterBody;
	public List<DataSkinSpine> skinCharacterCart;*/
}
#endregion







