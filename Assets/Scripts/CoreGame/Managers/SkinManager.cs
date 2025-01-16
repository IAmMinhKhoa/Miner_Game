using System.Collections.Generic;
using Newtonsoft.Json;
using NOOD.SerializableDictionary;
using UnityEngine;
using PlayFabManager.Data;
using PlayFab.EconomyModels;
using UI.Inventory;
using System;
using System.Linq;
public class SkinManager : Patterns.Singleton<SkinManager>
{
	public SkinResource skinResource;
	public string jsonFilePath = "Assets/Resources/Json/SkinResources.json";
	public bool isDone = false;
	[SerializeField]
	SkinDataSO skinGameDataAsset;
	public Dictionary<InventoryItemType, List<DataSkinBase>> InfoSkinGame { private set; get;}
	public Dictionary<InventoryItemType, List<string>> ItemBought { private set; get; } = new();

	public SkinDataSO SkinGameDataAsset => skinGameDataAsset;
	public List<SkinChangeMachineSO> skinEvent;

	protected override void Awake()
	{
		isPersistent = false;
		base.Awake();
	}
	public void InitData()//INIT find data SO
	{
		LoadSkinData(); //fectch data from json
		LoadAssets(); //get resource by path to list

		
		if (!Load())
		{
			ShaftManager.Instance.Shafts[0].shaftSkin = new ShaftSkin(0);
			ElevatorSystem.Instance.elevatorSkin = new();
			Counter.Instance.counterSkin = new();
		
			foreach (InventoryItemType itemType in Enum.GetValues(typeof(InventoryItemType)))
			{
				// Thêm từng phần tử của enum vào dictionary
				ItemBought[itemType] = new();
			}

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

		//Skin da mua
		Dictionary<string, List<string>> itemBoughtData = new();

		foreach (var entry in ItemBought)
		{
			itemBoughtData.Add(entry.Key.ToString(), entry.Value);
		}

		// Thêm dictionary vào saveData
		saveData.Add("itemBought", itemBoughtData);

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

			foreach (var entry in saveData.itemBought)
			{
				// Chuyển đổi key từ chuỗi về enum
				if (Enum.TryParse(entry.Key, out InventoryItemType itemType))
				{
					// Thêm vào dictionary ItemBought
					ItemBought.Add(itemType, entry.Value);
				}
			}

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
			InfoSkinGame = new()
			{
				[InventoryItemType.Elevator] = skinResource.skinElevator,
				[InventoryItemType.ShaftBg] = skinResource.skinBgShaft,
				[InventoryItemType.CounterBg] = skinResource.skinBgCounter,
				[InventoryItemType.ElevatorBg] = skinResource.skinBgElevator,
				[InventoryItemType.ShaftSecondBg] = skinResource.skinSecondBgShaft,
				[InventoryItemType.ShaftWaitTable] = skinResource.skinWaitTable,
				[InventoryItemType.ShaftCart] = skinResource.skinCartShaft,
				[InventoryItemType.CounterCart] = skinResource.skinCartCounter,
				[InventoryItemType.ShaftCharacter] = skinResource.skinShaftCharacterHead,
				[InventoryItemType.ShaftCharacterBody] = skinResource.skinShaftCharacterBody,
				[InventoryItemType.ElevatorCharacter] = skinResource.skinElevatorCharacterHead,
				[InventoryItemType.ElevatorCharacterBody] = skinResource.skinElevatorCharacterBody
			};
			Debug.Log("Data loaded successfully!");
		}
		else
		{
			Debug.LogError("JSON file not found in Resources folder!");
		}
	}

	public List<(string ID, string Name)> GetListPopupOtherItem(InventoryItemType type)
	{
		var skeletonData = SkinGameDataAsset.SkinGameData[type];
		var listBaseSkin = skinResource.skinWaitTable;
		List<(string ID, string Name)> listSkin = new();
		//Lay skin tu file json
		foreach (var skin in listBaseSkin)
		{
			if (skeletonData.GetSkeletonData(false).FindSkin("Icon_" + skin.id) != null)
			{
				listSkin.Add((skin.id, skin.name.GetContent(ManagersController.Instance.localSelected)));
			}
		}
		//lay skin tu SO
		//var listEventSkin = skinEvent.Where(item => item.type == type).First().listSkinGacha;
		//foreach (var skin in listEventSkin)
		//{
		//	if(skeletonData.GetSkeletonData(false).FindSkin("Icon_" + skin.ID) != null)
		//	{
		//		listSkin.Add((skin.ID, skin.Name));
		//	}
		//}
		return listSkin;
	}

	public List<DataSkinBase> GetListDataSkinBases(InventoryItemType type)
	{
		var skeletonData = SkinGameDataAsset.SkinGameData[type];
		List<DataSkinBase> listBaseSkin = InfoSkinGame[type];
		List<DataSkinBase> listSkin = new();
		//Lay skin tu file json
		foreach (var skin in listBaseSkin)
		{
			if (skeletonData.GetSkeletonData(false).FindSkin("Icon_" + skin.id) != null)
			{

				listSkin.Add(skin);
			}
		}
		////lay skin tu SO
		//var listEventSkin = skinEvent.Where(item => item.type == type).First().listSkinGacha;
		//foreach (var skin in listEventSkin)
		//{
		//	if (skeletonData.GetSkeletonData(false).FindSkin("Icon_" + skin.ID) != null)
		//	{
		//		var skinData = new DataSkinBase(skin.ID, skin.Name, skin.Description, "", "");
		//		listSkin.Add(skinData);
		//	}
		//}
		return listSkin;
	}

	public void BuyNewSkin(InventoryItemType item, string id)
	{
		ItemBought[item].Add(id);
	}

	private void LoadAssets()
	{
		
	}
	#endregion
}
#region Entity Skin Game
public class DataSkin
{
	public List<ShaftSkin> shaftSkins { get; set; }
	public ElevatorSkin elevatorSkin { get; set; }
	public CounterSkin counterSkin { get; set; }
	public Dictionary<string, List<string>> itemBought { set; get; }

}
public class SkinBase
{
	public string idBackGround { get; set; }
	public string idMilkCup { get; set; }

	public SkinBase(string idBackGround = "0", string idMilkCup = "0")
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

	public ShaftSkin(int index, string idBackGround = "0", string idWaitTable = "0", string idMilkCup = "0", string idCart = "0", string idSecondBg = "0", CharacterSkin characterSkin = null)
		: base(idBackGround, idMilkCup)
	{
		this.index = index;
		this.idWaitTable = idWaitTable;
		this.idCart = idCart;
		this.idSecondBg = idSecondBg;
		this.characterSkin = characterSkin ?? new CharacterSkin();
	}

	
}

public class ElevatorSkin : SkinBase
{
	public string idFrontElevator { get; set; }
	public string idBackElevator { get; set; }
	public CharacterSkin characterSkin { get; set; }

	public ElevatorSkin(string idBackGround = "0", string idMilkCup = "0", string idFrontElevator = "0", string idBackElevator = "0", CharacterSkin characterSkin = null)
		: base(idBackGround, idMilkCup)
	{
		this.idFrontElevator = idFrontElevator;
		this.idBackElevator = idBackElevator;
		this.characterSkin = characterSkin ?? new();
	}
	
}

public class CounterSkin : SkinBase
{
	public string idCart { get; set; }
	public string idSecondBg { get; set; }
	public CharacterSkin character { get; set; }

	public CounterSkin(string idBackGround = "0", string idMilkCup = "0", string idCart = "0", string idSecondBg = "0", CharacterSkin characterSkin = null)
		: base(idBackGround, idMilkCup)
	{
		this.idCart = idCart;
		this.character = characterSkin ?? new CharacterSkin();
		this.idSecondBg = idSecondBg;
	}

}

public class CharacterSkin
{
	public string idHead { get; set; }
	public string idBody { get; set; }

	public CharacterSkin(string idHead = "0", string idBody = "0")
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
	CounterCharacter,
	CounterSecondBg,
	BackElevator,
	ShaftCharacterBody,
	ElevatorCharacterBody,
	Null
}
#endregion

