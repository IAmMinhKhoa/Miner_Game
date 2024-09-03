using System.Collections;
using System.Collections.Generic;
using NOOD.SerializableDictionary;
using UnityEngine;
[CreateAssetMenu(fileName = "SkinData", menuName = "SkinData")]
public class SkinDataSO: ScriptableObject
{
	#region Shaft
	[Header("SHAFT")]
	[SerializeField] public SerializableDictionary<SkinShaftBg, DataSkinImage> skinBgShaft = new SerializableDictionary<SkinShaftBg, DataSkinImage>();
	[SerializeField] public SerializableDictionary<SkinShaftWaitTable, DataSkinImage> skinWaitTable = new SerializableDictionary<SkinShaftWaitTable, DataSkinImage>();
	[SerializeField] public SerializableDictionary<SkinShaftMilkCup, DataSkinImage> skinMilkCup = new SerializableDictionary<SkinShaftMilkCup, DataSkinImage>();

	[SerializeField] public SerializableDictionary<string, DataSkinSpine> skinCharacterHead = new SerializableDictionary<string, DataSkinSpine>();
	[SerializeField] public SerializableDictionary<string, DataSkinSpine> skinCharacterBody = new SerializableDictionary<string, DataSkinSpine>();
	[SerializeField] public SerializableDictionary<string, DataSkinSpine> skinCharacterCart= new SerializableDictionary<string, DataSkinSpine>();
	#endregion
	// Generic method to get the Sprite from a dictionary
	private Sprite GetSpriteFromDictionary<T>(SerializableDictionary<T, DataSkinImage> dictionary, T key, string dictionaryName)
	{
		if (dictionary.ContainsKey(key))
		{
			return dictionary.Dictionary[key].img;
		}
		else
		{
			Debug.LogError($"Key {key} not found in {dictionaryName} dictionary.");
			return null;
		}
	}

	#region SHAFT
	// Specific methods for each type of shaft
	public Sprite GetBrShaft(SkinShaftBg value)
	{
		return GetSpriteFromDictionary(skinBgShaft, value, "skinBgShaft");
	}

	public Sprite GetWaitTableShaft(SkinShaftWaitTable value)
	{
		return GetSpriteFromDictionary(skinWaitTable, value, "skinWaitTable");
	}

	public Sprite GetMilkCupShaft(SkinShaftMilkCup value)
	{
		return GetSpriteFromDictionary(skinMilkCup, value, "skinMilkCup");
	}
	#endregion

}



[System.Serializable]
public class DataSkinImage
{
	public string name;
	public Sprite img;
}
[System.Serializable]
public class DataSkinSpine
{
	public string name;
	public string idSkin;
}
