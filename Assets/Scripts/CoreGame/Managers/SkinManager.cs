using System.Collections.Generic;
using Newtonsoft.Json;
using NOOD.SerializableDictionary;
using UnityEngine;
using static ShaftManager;
using PlayFabManager.Data;
using PlayFab.EconomyModels;
using UI.Inventory;
public class SkinManager : Patterns.Singleton<SkinManager>
{
	public SkinResource skinResource = new();
	public string jsonFilePath = "Assets/Resources/Json/SkinResources.json";
	public bool isDone = false;
	public void InitData()//INIT find data SO
	{
		LoadSkinData(); //fectch data from json
		LoadAssets(); //get resource by path to list
		if (!Load())
		{
			ShaftManager.Instance.Shafts[0].shaftSkin = new ShaftSkin(0);
			ElevatorSystem.Instance.elevatorSkin = new();
			Counter.Instance.counterSkin = new();

		}
		for (int i = 0; i < ShaftManager.Instance.Shafts.Count; i++)
		{
			ShaftManager.Instance.Shafts[i].UpdateUI();
		}
		ElevatorSystem.Instance.UpdateUI();
		Counter.Instance.UpdateUI();
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
				{ "index", shaft.shaftIndex},
				{ "idBackGround", shaft.shaftSkin.idBackGround },
				{ "idWaitTable", shaft.shaftSkin.idWaitTable },
				{ "idMilkCup", shaft.shaftSkin.idMilkCup },
				{ "idSecondBg", shaft.shaftSkin.idSecondBg },
				{"idCart", shaft.shaftSkin.idCart },
				{"characterSkin", shaft.shaftSkin.characterSkin}

			};
			shafts.Add(shaftData);
		}
		var elvator = ElevatorSystem.Instance.elevatorSkin;
		var counter = Counter.Instance.counterSkin;
		if (shafts.Count == 0 || elvator == null || counter == null)
		{
			return;
		}

		saveData.Add("shaftSkins", shafts);
		saveData.Add("elevatorSkin", elvator);
		saveData.Add("counterSkin", counter);
		string json = JsonConvert.SerializeObject(saveData, Formatting.Indented);
		PlayFabDataManager.Instance.SaveData("SkinManager", json);
	}
	public bool Load()
	{
		if (PlayFabDataManager.Instance.ContainsKey("SkinManager"))
		{
			string json = PlayFabDataManager.Instance.GetData("SkinManager");
			DataSkin saveData = JsonConvert.DeserializeObject<DataSkin>(json);

			//int shaftsCount = saveData.shaftSkins.Count;

			for (int i = 0; i < saveData.shaftSkins.Count; i++)
			{
				ShaftManager.Instance.Shafts[i].shaftSkin = saveData.shaftSkins[i];
			}

			ElevatorSystem.Instance.elevatorSkin = saveData.elevatorSkin;
			Counter.Instance.counterSkin = saveData.counterSkin;
			isDone = true;
			return true;
		}
		return false;
	}



	#region Get resource by path
	private void LoadSkinData()
	{
		// Load the JSON file from Resources
		TextAsset jsonFile = Resources.Load<TextAsset>("Json/SkinResources"); // Bỏ phần mở rộng ".json"

		if (jsonFile != null)
		{
			string jsonContent = jsonFile.text;
			Debug.Log(jsonContent);
			skinResource = JsonUtility.FromJson<SkinResource>(jsonContent);
			Debug.Log("Data loaded successfully!");
		}
		else
		{
			Debug.LogError("JSON file not found in Resources folder!");
		}
	}


	private void LoadAssets()
	{
		// Load Sprites
		foreach (var imageData in skinResource.skinBgShaft)
		{
			imageData.sprite = Common.LoadSprite(imageData.path);
		}
		foreach (var imageData in skinResource.skinSecondBgShaft)
		{
			imageData.sprite = Common.LoadSprite(imageData.path);
		}
		foreach (var imageData in skinResource.skinBgElevator)
		{
			imageData.sprite = Common.LoadSprite(imageData.path);
		}
		foreach (var imageData in skinResource.skinBgCounter)
		{
			imageData.sprite = Common.LoadSprite(imageData.path);
		}
		foreach (var imageData in skinResource.skinWaitTable)
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
	public ElevatorSkin elevatorSkin { get; set; }
	public CounterSkin counterSkin { get; set; }

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
	public string idSecondBg { get; set; }
	public CharacterSkin characterSkin { get; set; }

	public ShaftSkin(int index, string idBackGround = "1", string idWaitTable = "1", string idMilkCup = "1", string idCart = "1", string idSecondBg = "1", CharacterSkin characterSkin = null)
		: base(idBackGround, idMilkCup)
	{
		this.index = index;
		this.idWaitTable = idWaitTable;
		this.idCart = idCart;
		this.idSecondBg = idSecondBg;
		this.characterSkin = characterSkin ?? new CharacterSkin();
	}

	public Dictionary<InventoryItemType, DataSkinImage> GetDataSkin()
	{
		int idBg = int.Parse(idBackGround);
		int idSBg = int.Parse(idSecondBg);
		int idWt = int.Parse(idWaitTable);
		//int idMc = int.Parse(idMilkCup);
		//int idC = int.Parse(idCart);
		return new Dictionary<InventoryItemType, DataSkinImage>()
		{
			{InventoryItemType.ShaftBg, SkinManager.Instance.skinResource.skinBgShaft[idBg]},
			{InventoryItemType.ShaftSecondBg, SkinManager.Instance.skinResource.skinSecondBgShaft[idSBg]},
			{InventoryItemType.ShaftWaitTable, SkinManager.Instance.skinResource.skinWaitTable[idWt]},
			//{"skinMcShaft", SkinManager.skinResource.skinMilkCup[idMc]},
			//{"skinCShaft", SkinManager.skinResource.skinCharacterCart[idC]},
		};
	}
}

public class ElevatorSkin : SkinBase
{
	public string idFrontElevator { get; set; }
	public string idBackElevator { get; set; }
	public CharacterSkin characterSkin { get; set; }

	public ElevatorSkin(string idBackGround = "1", string idMilkCup = "1", string idFrontElevator = "1", string idBackElevator = "1", CharacterSkin characterSkin = null)
		: base(idBackGround, idMilkCup)
	{
		this.idFrontElevator = idFrontElevator;
		this.idBackElevator = idBackElevator;
		this.characterSkin = characterSkin ?? new();
	}
	public Dictionary<InventoryItemType, DataSkinImage> GetDataSkin()
	{
		int idBg = int.Parse(idBackGround);
		//int idWt = int.Parse(idWaitTable);
		//int idMc = int.Parse(idMilkCup);
		//int idC = int.Parse(idCart);
		return new Dictionary<InventoryItemType, DataSkinImage>()
		{
			{InventoryItemType.ElevatorBg, SkinManager.Instance.skinResource.skinBgElevator[idBg]},
			//{"skinWtShaft", SkinManager.skinResource.skinWaitTable[idWt]},
			//{"skinMcShaft", SkinManager.skinResource.skinMilkCup[idMc]},
			//{"skinCShaft", SkinManager.skinResource.skinCharacterCart[idC]},
		};
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
	public Dictionary<InventoryItemType, DataSkinImage> GetDataSkin()
	{
		int idBg = int.Parse(idBackGround);
		//int idWt = int.Parse(idWaitTable);
		//int idMc = int.Parse(idMilkCup);
		//int idC = int.Parse(idCart);
		return new Dictionary<InventoryItemType, DataSkinImage>()
		{
			{InventoryItemType.CounterBg, SkinManager.Instance.skinResource.skinBgCounter[idBg]},
			//{"skinWtShaft", SkinManager.skinResource.skinWaitTable[idWt]},
			//{"skinMcShaft", SkinManager.skinResource.skinMilkCup[idMc]},
			//{"skinCShaft", SkinManager.skinResource.skinCharacterCart[idC]},
		};
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
public enum InventoryItemType
{
	ShaftBg,
	CounterBg,
	ElevatorBg,
	CounterCart,
	Elevator,
	ShaftSecondBg,
	ShaftCart,
	ShaftWaitTable,
	ShaftCharacter,
	ElevatorCharacter,
	CounterCharacter
}
#endregion

