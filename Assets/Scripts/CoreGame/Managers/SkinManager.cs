using System.Collections.Generic;
using Newtonsoft.Json;
using NOOD.SerializableDictionary;
using UnityEngine;
using static ShaftManager;

public class SkinManager : Patterns.Singleton<SkinManager>
{
	public static SkinDataSO SkinDataSO = null;
	public bool isDone = false;
	public void FindSkinDataSO()//INIT find data SO
	{
		SkinDataSO[] soundDataSOs = Resources.LoadAll<SkinDataSO>("");
		if (soundDataSOs.Length > 0)
			SkinDataSO = Resources.FindObjectsOfTypeAll<SkinDataSO>()[0];
		if (SkinDataSO == null)
			Debug.LogError("Can't find SkinDataSO, please create one in Resources folder using Create -> SkinDataSO");
		else
			Debug.Log("Load SkinDataSO success");
	}


	
	public void Save()
	{
		//create JSON to save data
		Dictionary<string, object> saveData = new Dictionary<string, object>();

		List<object> shafts = new List<object>();
		foreach (var shaft in ShaftManager.Instance.Shafts)
		{
			Dictionary<string, object> shaftData = new Dictionary<string, object>
			{
				{ "index", shaft.shaftIndex },
				{ "idBackGround", shaft.shaftSkin.idBackGround },
				{ "idWaitTable", shaft.shaftSkin.idWaitTable },
				{ "idMilkCup", shaft.shaftSkin.idMilkCup },

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
	public bool Load()
	{
		if (PlayerPrefs.HasKey("SkinManager"))
		{
			string json = PlayerPrefs.GetString("SkinManager");
			Debug.Log(json);
			DataSkin saveData = JsonConvert.DeserializeObject<DataSkin>(json);

			int shaftsCount = saveData.shaftSkins.Count;
			
			for (int i = 0; i < saveData.shaftSkins.Count; i++)
			{
				ShaftManager.Instance.Shafts[i].shaftSkin = saveData.shaftSkins[i];
			}
			isDone = true;
			return true;
		}
		return false;
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
