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
		public string cost;
		public string quality;
	    public DataSkinBase(string id, string name, string desc, string cost, string quality)
		{
			this.id = id;
			this.name = name;
			this.desc = desc;
			this.cost = cost;
			this.quality = quality;
		}
	}


	[System.Serializable]
	public class SkinResource
	{
		public List<DataSkinBase> skinElevator;
		public List<DataSkinBase> skinBgShaft;
		public List<DataSkinBase> skinBgCounter;
		public List<DataSkinBase> skinBgElevator;
		public List<DataSkinBase> skinSecondBgShaft;
		public List<DataSkinBase> skinWaitTable;
		public List<DataSkinBase> skinCartShaft;
		public List<DataSkinBase> skinCartCounter;
		public List<DataSkinBase> skinShaftCharacterHead;
		public List<DataSkinBase> skinShaftCharacterBody;
		public List<DataSkinBase> skinElevatorCharacterHead;
		public List<DataSkinBase> skinElevatorCharacterBody;

	/*public List<DataSkinImage> skinMilkCup;

	public List<DataSkinSpine> skinCharacterHead;
	public List<DataSkinSpine> skinCharacterBody;
	public List<DataSkinSpine> skinCharacterCart;*/
}
#endregion







