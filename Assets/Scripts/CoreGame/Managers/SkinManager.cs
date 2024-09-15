using System.Collections.Generic;
using Newtonsoft.Json;
using NOOD.SerializableDictionary;
using UnityEngine;
using static ShaftManager;
using PlayFabManager.Data;

public class SkinManager : Patterns.Singleton<SkinManager>
{
	public static SkinResource skinResource = new();
	public string jsonFilePath = "Assets/Resources/Json/SkinResources.json";
	public bool isDone = false;
	public void InitData()//INIT find data SO
	{
		LoadSkinData(); //fectch data from json
		LoadAssets(); //get resource by path to list
		if(!Load())
		{
			ShaftManager.Instance.Shafts[0].shaftSkin =  new ShaftSkin(0);
			Debug.Log(JsonConvert.SerializeObject(ShaftManager.Instance.Shafts[0].shaftSkin));
		}
		isDone = true;
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
		Debug.Log(json);
		PlayerPrefs.SetString("SkinManager", json);
	}
	public bool Load()
	{
		if (PlayFabDataManager.Instance.ContainsKey("SkinManager"))
		{
			string json = PlayFabDataManager.Instance.GetData("SkinManager");
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



	#region Get resource by path
	private void LoadSkinData()
	{
		if (System.IO.File.Exists(jsonFilePath))
		{
			string jsonContent = System.IO.File.ReadAllText(jsonFilePath);
			skinResource = JsonUtility.FromJson<SkinResource>(jsonContent);
			Debug.Log("Data loaded successfully!");
		}
		else
		{
			Debug.LogError("JSON file not found at: " + jsonFilePath);
		}
	}

	private void LoadAssets()
	{
		// Load Sprites
		foreach (var imageData in skinResource.skinBgShaft)
		{
			imageData.sprite = Common.LoadSprite(imageData.path);
		}

		// Load Spine data
		/*foreach (var spineData in skinResource.skinCharacterHead)
		{
			spineData.skeletonData = Common.LoadSpine(spineData.path);
		}*/
	}
	#endregion
}
#region Entity Skin Game
public class DataSkin
{
	public List<ShaftSkin> shaftSkins { get; set; }

}
public class SkinBase
{
	public string idBackGround { get; set; }
	public string idMilkCup { get; set; }

	public SkinBase(string idBackGround = "1", string idMilkCup = "1")
	{
		this.idBackGround = idBackGround;
		this.idMilkCup = idMilkCup;
	}
}

public class ShaftSkin : SkinBase
{
	public int index { get; set; }
	public string idWaitTable { get; set; }
	public string idCart { get; set; }
	public CharacterSkin character { get; set; }

	public ShaftSkin(int index, string idBackGround = "1", string idWaitTable = "1", string idMilkCup = "1", string idCart = "1", CharacterSkin characterSkin = null)
		: base(idBackGround, idMilkCup)
	{
		this.index = index;
		this.idWaitTable = idWaitTable;
		this.idCart = idCart;
		this.character = characterSkin ?? new CharacterSkin();
	}

	public Dictionary<string, DataSkinImage> GetDataSkin()
	{
		int idBg = int.Parse(idBackGround);
		//int idWt = int.Parse(idWaitTable);
		//int idMc = int.Parse(idMilkCup);
		//int idC = int.Parse(idCart);
		return new Dictionary<string, DataSkinImage>()
		{
			{"skinBgShaft", SkinManager.skinResource.skinBgShaft[idBg]},
			//{"skinWtShaft", SkinManager.skinResource.skinWaitTable[idWt]},
			//{"skinMcShaft", SkinManager.skinResource.skinMilkCup[idMc]},
			//{"skinCShaft", SkinManager.skinResource.skinCharacterCart[idC]},
		};
	}
}

public class ElevatorSkin : SkinBase
{
	public string idFrontElevator { get; set; }
	public string idBackElevator { get; set; }

	public ElevatorSkin(string idBackGround = "1", string idMilkCup = "1", string idFrontElevator = "1", string idBackElevator = "1")
		: base(idBackGround, idMilkCup)
	{
		this.idFrontElevator = idFrontElevator;
		this.idBackElevator = idBackElevator;
	}
}

public class CounterSkin : SkinBase
{
	public string idCart { get; set; }
	public CharacterSkin character { get; set; }

	public CounterSkin(string idBackGround = "1", string idMilkCup = "1", string idCart = "1", CharacterSkin characterSkin = null)
		: base(idBackGround, idMilkCup)
	{
		this.idCart = idCart;
		this.character = characterSkin ?? new CharacterSkin();
	}
}

public class CharacterSkin
{
	public string idHead { get; set; }
	public string idBody { get; set; }

	public CharacterSkin(string idHead = "1", string idBody = "1")
	{
		this.idHead = idHead;
		this.idBody = idBody;
	}
}
#endregion

