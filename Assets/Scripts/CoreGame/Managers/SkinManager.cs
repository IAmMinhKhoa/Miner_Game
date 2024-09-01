using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;
using static ShaftManager;

public class SkinManager : Patterns.Singleton<SkinManager>
{
	public static SkinDataSO SkinDataSO = null;
	public void FindSkinData()
	{
		SkinDataSO[] soundDataSOs = Resources.LoadAll<SkinDataSO>("");
		if (soundDataSOs.Length > 0)
			SkinDataSO = Resources.FindObjectsOfTypeAll<SkinDataSO>()[0];
		if (SkinDataSO == null)
			Debug.LogError("Can't find SkinDataSO, please create one in Resources folder using Create -> SkinDataSO");
		else
			Debug.Log("Load SkinDataSO success");
	}


	public Sprite GetBrShaft(SkinShaftBg enumBr)
	{
		if (SkinDataSO.skinBgShaft.ContainsKey(enumBr))
		{
			DataSkinImage dataSkinImage = SkinDataSO.skinBgShaft.Dictionary[enumBr];
				
			return dataSkinImage.img;
		}
		else
		{
			Debug.LogError($"Key {enumBr} not found in skinBgShaft dictionary.");
			return null; // Or return a default Sprite
		}
	}


	public void Save()
	{
		//create JSON to save data
		Dictionary<string, object> saveData = new Dictionary<string, object>();

		Debug.Log("khoa: " + ShaftManager.Instance.Shafts.Count);
		List<object> shafts = new List<object>();
		foreach (var shaft in ShaftManager.Instance.Shafts)
		{
			Dictionary<string, object> shaftData = new Dictionary<string, object>
			{
				{ "idBackGround", shaft.shaftSkin.idBackGround },

			};
			shafts.Add(shaftData);
		}
		if (shafts.Count == 0)
		{
			return;
		}
		saveData.Add("shaftSkins", shafts);
		Debug.Log("save data: " + saveData.ToString());
		string json = JsonConvert.SerializeObject(saveData, Formatting.Indented);
		PlayerPrefs.SetString("SkinManager", json);
	}

	#region Entity Skin Game
	public class DataSkin
	{
		public List<ShaftSkin> shaftSkins { get; set; }
	
	}

	
	public class ElevatorSkin
	{

	}
	public class CounterSkin
	{

	}
	public class OutSideSkin
	{

	}
	#endregion
}
