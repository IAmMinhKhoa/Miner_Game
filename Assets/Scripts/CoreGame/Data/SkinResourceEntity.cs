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
		public MultipleLanguageContent name;
		public MultipleLanguageContent desc;
		public MultipleLanguageContent exp;
		public string cost;
		public string quality;
		public float value_1;
		public float value_2;
		public float value_3;
	public DataSkinBase(string id, MultipleLanguageContent name, MultipleLanguageContent desc, MultipleLanguageContent exp, string cost, string quality, float expLevel, float workHardLevel, float funnyLevel)
		{
			this.id = id;
			this.name = name;
			this.desc = desc;
			this.exp = exp;
			this.cost = cost;
			this.quality = quality;
			this.value_1 = expLevel;
			this.value_2 = workHardLevel;
			this.value_3 = funnyLevel;
		}
	}
	[System.Serializable]
	public class MultipleLanguageContent
	{
		public string vi;
		public string en;

	public string GetContent(string languageCode)
	{
		var languageDictionary = new Dictionary<string, string>
		{
			{ "vi",  vi},
			{ "en", en}
		};

		return languageDictionary.ContainsKey(languageCode) ? languageDictionary[languageCode] : "";
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
		public List<DataSkinBase> skinShaftBarCounter;
		public List<DataSkinBase> skinCashierCounter;

	/*public List<DataSkinImage> skinMilkCup;

	public List<DataSkinSpine> skinCharacterHead;
	public List<DataSkinSpine> skinCharacterBody;
	public List<DataSkinSpine> skinCharacterCart;*/
}
#endregion







