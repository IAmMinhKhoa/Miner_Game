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
	[SerializeField] public SerializableDictionary<string, DataSkinImage> skinWaitTable = new SerializableDictionary<string, DataSkinImage>();
	[SerializeField] public SerializableDictionary<string, DataSkinImage> skinCupMilk = new SerializableDictionary<string, DataSkinImage>();

	[SerializeField] public SerializableDictionary<string, DataSkinSpine> skinCharacterHead = new SerializableDictionary<string, DataSkinSpine>();
	[SerializeField] public SerializableDictionary<string, DataSkinSpine> skinCharacterBody = new SerializableDictionary<string, DataSkinSpine>();
	[SerializeField] public SerializableDictionary<string, DataSkinSpine> skinCharacterCart= new SerializableDictionary<string, DataSkinSpine>();
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
