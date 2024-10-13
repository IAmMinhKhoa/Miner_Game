using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Linq;
using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using PlayFabManager.Data;

public class ShaftManager : Patterns.Singleton<ShaftManager>
{
	public Action OnNewShaftCreated;
	public event Action OnShaftUpdated;
	public Action<int> OnUpdateShaftInventoryUI;
	[Header("Prefab")]
	[SerializeField] private Shaft shaftPrefab;
	[SerializeField] private float shaftYOffset = 1.716f;
	[SerializeField] private float roofOffset = 2f;
	[SerializeField] private float roofOffsetBuilding = 2f;
	[SerializeField] private double currentCost = 0;
	[SerializeField] private GameObject _roof;
	[SerializeField] private GameObject _roof_Building;

	[Header("Basement")]
	[SerializeField] public List<Shaft> Shafts = new();

	[Header("Shaft")]
	[SerializeField] private Vector3 firstShaftPosition = new(0.656000018f, -0.0390000008f, 0);
	[SerializeField] int maxShaftCount = 30;
	[SerializeField] private double initCost = 10;

	public double CurrentCost => currentCost;

	public Shaft Shaft => shaftPrefab;
	private bool isDone = false;
	public bool IsDone => isDone;
	[Header("Building Shaft")]
	public bool isBuilding=false;
	public double TimeBuild = 5f;
	public double TimeCurrentBuild;
	private void Start()
	{
		//InitializeShafts();
	}
	protected override void Awake()
	{
		isPersistent = false;
		base.Awake();
	}
	public void AddShaft()
	{
		OnNewShaftCreated?.Invoke();

		Transform lastShaft = Shafts[^1].transform;
		Shaft newShaft = Instantiate(shaftPrefab, lastShaft.position, Quaternion.identity);
		newShaft.transform.localPosition += new Vector3(0, shaftYOffset, 0);
		newShaft.IndexBoost = BaseShaftIndexScale();

		ShaftUpgrade shaftUpgrade = newShaft.GetComponent<ShaftUpgrade>();
		shaftUpgrade.SetInitialValue(Shafts.Count, CalculateNextShaftCost(), 1);

		Shafts.Add(newShaft);
		newShaft.shaftSkin = new ShaftSkin(Shafts.Count);

		newShaft.gameObject.GetComponent<ShaftUI>().NewShaftCostText.text = Currency.DisplayCurrency(CalculateNextShaftCost());
		float newY = newShaft.transform.position.y;
		newY += roofOffset;
		_roof.transform.position = new Vector3(_roof.transform.position.x, newY, 0);

		CustomCamera.Instance.SetMaxY(Shafts[^1].transform.position.y);
		OnShaftUpdated?.Invoke();
	}

	public void InitializeShafts()
	{
		if (!Load())
		{
			Shaft firstShaft = Instantiate(shaftPrefab, firstShaftPosition, Quaternion.identity);
			Shafts.Add(firstShaft);

			ShaftUpgrade shaftUpgrade = firstShaft.GetComponent<ShaftUpgrade>();
			shaftUpgrade.SetInitialValue(0, initCost, 1);
			firstShaft.shaftSkin = new ShaftSkin(Shafts.Count);

			firstShaft.gameObject.GetComponent<ShaftUI>().NewShaftCostText.text = Currency.DisplayCurrency(CalculateNextShaftCost());
			float newY = firstShaft.transform.position.y;
			newY += roofOffset;
			_roof.transform.position = new Vector3(_roof.transform.position.x, newY, 0);
			OnShaftUpdated?.Invoke();
		}

		isDone = true;

		ValidateTimeOffline();
		

		CustomCamera.Instance.SetMaxY(Shafts[^1].transform.position.y);
	}

	private double CalculateNextShaftCost()
	{
		int shaftCount = Shafts.Count;
		currentCost = shaftCount switch
		{
			0 => 10,
			1 => 2600,
			2 => 78000,
			_ => Mathf.Pow(20, shaftCount - 2) * 78000,
		};

		return currentCost;
	}

	private double BaseShaftIndexScale()
	{
		double scale = 1;

		int shaftCount = Shafts.Count;

		scale = shaftCount switch
		{
			0 => 1,
			1 => 50,
			_ => Mathf.Pow(17, shaftCount - 1) * 50,
		};
		return scale;
	}

	public double GetTotalNS()
	{
		double totalNS = 0;
		foreach (Shaft shaft in Shafts)
		{
			totalNS += shaft.GetShaftNS();
		}
		return totalNS;
	}

	public void Save()
	{
		//create JSON to save data
		Dictionary<string, object> saveData = new Dictionary<string, object>();
		saveData.Add("Shafts", Shafts.Count);
		saveData.Add("CurrentCost", currentCost);
		saveData.Add("isBuilding", isBuilding);
		saveData.Add("TimeCurrentBuild", TimeCurrentBuild);

		List<object> shafts = new List<object>();
		foreach (Shaft shaft in Shafts)
		{
			Dictionary<string, object> shaftData = new Dictionary<string, object>
			{
				{ "Index", shaft.shaftIndex },
				{ "LevelBoost", shaft.LevelBoost },
				{ "IndexBoost", shaft.IndexBoost },
				{ "Brewers", shaft.Brewers.Count },
				{ "CurrentDeposit", shaft.CurrentDeposit.CurrentPaw },
				{"Level", shaft.gameObject.GetComponent<ShaftUpgrade>().CurrentLevel},
				{"InitCost", shaft.gameObject.GetComponent<ShaftUpgrade>().GetInitialCost()},
				{"ManagerIndex", shaft.ManagerLocation.Manager != null ? shaft.ManagerLocation.Manager.Index : -1},
			};
			shafts.Add(shaftData);
		}
		if (shafts.Count == 0)
		{
			return;
		}
		saveData.Add("ShaftsData", shafts);
		Debug.Log("save data: " + saveData.ToString());
		string json = JsonConvert.SerializeObject(saveData, Formatting.Indented);
		PlayFabManager.Data.PlayFabDataManager.Instance.SaveData("ShaftManager", json);
	}

	private bool Load()
	{
		if (PlayFabManager.Data.PlayFabDataManager.Instance.ContainsKey("ShaftManager"))
		{
			string json = PlayFabManager.Data.PlayFabDataManager.Instance.GetData("ShaftManager");

			Data saveData = JsonConvert.DeserializeObject<Data>(json);

			int shaftsCount = saveData.Shafts;
			currentCost = saveData.CurrentCost;
			isBuilding = saveData.IsBuilding;
			TimeCurrentBuild = saveData.TimeCurrentBuild;
			foreach (ShaftData shaftData in saveData.ShaftsData)
			{
				int index = shaftData.Index;
				double levelBoost = shaftData.LevelBoost;
				double indexBoost = shaftData.IndexBoost;
				int brewers = shaftData.Brewers;
				double currentDeposit = shaftData.CurrentDeposit;
				int level = shaftData.Level;
				double initCost = shaftData.InitCost;

				Vector3 position = Shafts.Count switch
				{
					0 => firstShaftPosition,
					_ => Shafts[^1].transform.position + new Vector3(0, shaftYOffset, 0),
				};

				Shaft shaft = Instantiate(shaftPrefab, position, Quaternion.identity);
				shaft.LevelBoost = levelBoost;
				shaft.IndexBoost = indexBoost;
				shaft.numberBrewer = brewers;
				shaft.gameObject.GetComponent<ShaftUpgrade>().SetInitialValue(index, initCost, level);
				shaft.SetDepositValue(currentDeposit);
				shaft.gameObject.GetComponent<ShaftUI>().UpdateSkeletonData();
				shaft.gameObject.GetComponent<ShaftUI>().m_buyNewShaftButton.gameObject.SetActive(false);
				Shafts.Add(shaft);
				float newY = shaft.transform.position.y;
				newY += roofOffset;
				_roof.transform.position = new Vector3(_roof.transform.position.x, newY, 0);

				if (shaftData.ManagerIndex != -1)
				{
					ManagersController.Instance.ShaftManagers[index].SetupLocation(shaft.ManagerLocation);
				}
				OnShaftUpdated?.Invoke();
			}
			Shafts.Last().gameObject.GetComponent<ShaftUI>().m_buyNewShaftButton.gameObject.SetActive(true);
			Shafts.Last().gameObject.GetComponent<ShaftUI>().NewShaftCostText.text = Currency.DisplayCurrency(CalculateNextShaftCost());
			//Debug.Log(json);
			return true;
		}
		return false;
	}
	public IEnumerator AddShaftAfterCooldown()
	{
		isBuilding = true;
		TimeCurrentBuild =TimeBuild;
		_roof_Building.SetActive(true);
		_roof.transform.position = new Vector3(_roof.transform.position.x, _roof.transform.position.y+ roofOffsetBuilding,0);
		while (TimeCurrentBuild > 0)
		{
			TimeCurrentBuild -= Time.deltaTime;
			yield return null;  // Wait for the next frame
		}

		_roof_Building.SetActive(false);
		isBuilding = false;
		AddShaft();  // Call AddShaft after cooldown
	}
	private void ValidateTimeOffline()
	{
		string lastTimeQuit = PlayFabDataManager.Instance.GetData("LastTimeQuit");
		System.DateTime lastTime = System.DateTime.Parse(lastTimeQuit);
		System.TimeSpan timeSpan = System.DateTime.Now - lastTime;
		double seconds = timeSpan.TotalSeconds;
		if (isBuilding)
		{
			TimeCurrentBuild -= seconds;
			if (TimeCurrentBuild <= 0)
			{
				isBuilding = false;
				TimeCurrentBuild = 0;
				AddShaft();
			}
			else
			{
				StartCoroutine(AddShaftAfterCooldown());
			}
		}
		
		
	}
	public class Data
	{
		public int Shafts { get; set; }
		public double CurrentCost { get; set; }
		public bool IsBuilding { get; set; }
		public double TimeCurrentBuild { get; set; }
		public List<ShaftData> ShaftsData { get; set; }
	}

	public class ShaftData
	{
		public int Index { get; set; }
		public double LevelBoost { get; set; }
		public double IndexBoost { get; set; }
		public int Brewers { get; set; }
		public double CurrentDeposit { get; set; }
		public int Level { get; set; }
		public double InitCost { get; set; }
		public int ManagerIndex { get; set; }
		public int Br { get; set; }

	}
}


