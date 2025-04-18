using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NOOD;
using System;
using System.Linq;

public class UpgradeManager : Patterns.Singleton<UpgradeManager>
{
	public Action<int> OnUpgradeRequest;

	[Header("Upgrade Panel Prefab")]
	[SerializeField] private Transform _upgradePanelPref;
	private UpgradeUI m_upgradePanel;
	private Transform m_upgradePanelTrans;
	private BaseUpgrade _baseUpgrade;
	private ManagerLocation _locationType;
	private BaseWorker _baseWorkerRef;
	private TransportMachine _transportMachine;
	private int _number;
	private List<Brewer> _brewers;
	private List<Transporter> _transporters;

	#region ----Unity Methods----
	protected override void Awake()
	{
		isPersistent = false;
		base.Awake();
	}
	private void Start()
	{
		m_upgradePanelTrans = Instantiate(_upgradePanelPref, GameUI.Instance.transform);
		m_upgradePanel = m_upgradePanelTrans.GetComponentInChildren<UpgradeUI>();
		ControlPanel(false);
		if (Common.IsTablet)
		{
			_upgradePanelPref.GetComponent<RectTransform>().localScale = new Vector3(0.8f, 0.8f, 0.8f);
			Debug.Log("Scale has been set to 0.8");
		}
	}
	private void OnEnable()
	{
		ShaftUI.OnUpgradeRequest += ShowShaftUpgradePanel;
		ElevatorUI.OnUpgradeRequest += ShowElevatorUpgradePanel;
		CounterUI.OnUpgradeRequest += ShowCounterUpgradePanel;
		OnUpgradeRequest += OnUpgradeAction;
		BaseUpgrade.OnUpgradeSuccess += ResetPanel;
	}

	private void OnDisable()
	{
		ShaftUI.OnUpgradeRequest -= ShowShaftUpgradePanel;
		ElevatorUI.OnUpgradeRequest -= ShowElevatorUpgradePanel;
		CounterUI.OnUpgradeRequest -= ShowCounterUpgradePanel;
		OnUpgradeRequest -= OnUpgradeAction;
		BaseUpgrade.OnUpgradeSuccess -= ResetPanel;
	}
	#endregion

	#region ----Methods----
	private void ShowShaftUpgradePanel(int index)
	{
		List<Shaft> shafts = ShaftManager.Instance.Shafts;
		foreach (var shaft in shafts)
		{
			if (shaft.shaftIndex == index)
			{
				_baseUpgrade = shaft.GetComponent<ShaftUpgrade>();
				_locationType = ManagerLocation.Shaft;
				_transportMachine = shaft.transportMachine;
				/*_brewers = shaft.Brewers;
				_baseWorkerRef = _brewers.First();
				_number = shaft.Brewers.Count;*/

				var shaftUI = shaft.GetComponent<ShaftUI>();
				if (shaftUI != null)
				{
					string skinName = shaftUI.GetCurrentTableSkinName();
					var dataAsset = shaftUI.GetTableDataAsset();
					m_upgradePanel.SetBarCounterData(dataAsset, skinName);
				}

				break;
			}
		}

		ResetPanel();
	}

	private void ShowElevatorUpgradePanel()
	{
		_baseUpgrade = ElevatorSystem.Instance.gameObject.GetComponent<ElevatorUpgrade>();
		_locationType = ManagerLocation.Elevator;
		_baseWorkerRef = ElevatorSystem.Instance.ElevatorController;
		ResetPanel();
	}

	private void ShowCounterUpgradePanel()
	{
		_baseUpgrade = Counter.Instance.gameObject.GetComponent<CounterUpgrade>();
		_locationType = ManagerLocation.Counter;
		_transporters = Counter.Instance.Transporters;
		_baseWorkerRef = _transporters.First();
		_number = Counter.Instance.Transporters.Count;

		var counterUI = Counter.Instance.GetComponent<CounterUI>();
		if (counterUI != null)
		{
			string skinName = counterUI.GetCurrentCashierCounterSkinName();
			var dataAsset = counterUI.GetCashierCounterDataAsset();
			m_upgradePanel.SetCashierCounterData(dataAsset, skinName);
		}

		ResetPanel();
	}


	private void ControlPanel(bool open)
	{
		m_upgradePanelTrans.gameObject.SetActive(open);
		if (open)
		{
			m_upgradePanel.OpenPanel();
		}
	}

	private void ResetPanel()
	{
		m_upgradePanel.SetUpPanel(CalculateUpgradeAmount());
		switch (_locationType)
		{
			case ManagerLocation.Shaft:
				Debug.Log("khoa check:"+_transportMachine.ValueProduct.ToString("F0"));
				m_upgradePanel.SetWorkerInfo(_locationType, "!!! Upgrade !!!", _transportMachine.ProductPerSecond, _transportMachine.ValueProduct.ToString("F0"), 111, _baseUpgrade.CurrentLevel);
				break;
			case ManagerLocation.Elevator:
				m_upgradePanel.SetWorkerInfo(_locationType, "!!! Upgrade !!!", _baseWorkerRef.ProductPerSecond, _baseWorkerRef.MoveTime.ToString("F2"), GetTotalProduction(), _baseUpgrade.CurrentLevel);
				break;
			case ManagerLocation.Counter:
				m_upgradePanel.SetWorkerInfo(_locationType, "!!! Upgrade !!!", _baseWorkerRef.ProductPerSecond, _transporters.Count.ToString(), GetTotalProduction(), _baseUpgrade.CurrentLevel);
				break;
		}
		ControlPanel(true);
	}

	private void OnUpgradeAction(int amount)
	{
		if (_baseUpgrade != null)
		{
			if (PawManager.Instance.CurrentPaw >= GetUpgradeCost(amount))
			{
				_baseUpgrade.Upgrade(amount);
			}
			else
			{
				Debug.Log("Not enough paw");
			}
		}
	}

	private int CalculateUpgradeAmount()
	{
		int amount = 0;
		double paw = PawManager.Instance.CurrentPaw;
		double cost = _baseUpgrade.CurrentCost;
		int level = _baseUpgrade.CurrentLevel;
		while (paw >= cost)
		{
			amount++;
			paw -= cost;
			level++;
			cost *= 1 + _baseUpgrade.GetNextUpgradeCostScale(level);
		}

		return amount;
	}

	public double GetUpgradeCost(int amount)
	{
		double total = 0;
		double cost = _baseUpgrade.CurrentCost;
		int level = _baseUpgrade.CurrentLevel;
		for (int i = 1; i <= amount; i++)
		{
			total += cost;
			level++;
			cost *= 1 + _baseUpgrade.GetNextUpgradeCostScale(level);
		}

		return total;
	}

	public double GetInitCost()
	{
		return _baseUpgrade.GetInitialCost();
	}

	public int CalculateUpgradeAmount(double paw, BaseUpgrade baseUpgrade)
	{
		int amount = 0;
		double cost = baseUpgrade.CurrentCost;
		int level = baseUpgrade.CurrentLevel;
		while (paw >= cost)
		{
			amount++;
			paw -= cost;
			level++;
			cost *= 1 + baseUpgrade.GetNextUpgradeCostScale(level);
		}

		return amount;
	}

	public double GetTotalCakeValue(int amount)
	{
		return _transportMachine.ValueProduct*(_baseUpgrade.GetProductionCakeScale(amount)-1d);
	}

	public double GetTotalBakingTime(int amount)
	{
		//Debug.Log("khoa check:"+_transportMachine.ProductPerSecond+"/"+_baseUpgrade.GetProductionBakingTime(amount));
		return _transportMachine.ProductPerSecond*(_baseUpgrade.GetProductionBakingTime(amount)-1d);
	}
	public double GetProductIncrement(int amount)
	{

		return _baseWorkerRef.ProductPerSecond * (_baseUpgrade.GetProductionCakeScale(amount) - 1d);
	}

	public int GetWorkerIncrement(int amount, ManagerLocation location)
	{
		int currentWorker = location switch
		{
			ManagerLocation.Shaft => _brewers.Count,
			ManagerLocation.Counter => _transporters.Count,
			_ => 1
		};
		return _baseUpgrade.GetNumberWorkerAtLevel(_baseUpgrade.CurrentLevel + amount) - currentWorker;
	}

	public double GetIncrementTotal(int amount, ManagerLocation location)
	{
		var current = _baseWorkerRef.ProductPerSecond * _baseWorkerRef.WorkingTime;
		var next = current * (_baseUpgrade.GetProductionCakeScale(amount) - 1d);
		var incrementWorker = GetWorkerIncrement(amount, location);
		var currentNumberWorker = location switch
		{
			ManagerLocation.Shaft => _brewers.Count,
			ManagerLocation.Counter => _transporters.Count,
			_ => 1
		};
		if (location == ManagerLocation.Shaft || location == ManagerLocation.Counter)
		{
			return current * incrementWorker + next * currentNumberWorker + next * incrementWorker;
		}
		else
		{
			return next;
		}
	}

	public double GetTotalProduction()
	{
		return _locationType switch
		{
			ManagerLocation.Shaft => _baseWorkerRef.ProductPerSecond * _baseWorkerRef.WorkingTime * _brewers.Count,
			ManagerLocation.Counter => _baseWorkerRef.ProductPerSecond * _baseWorkerRef.WorkingTime * _transporters.Count,
			_ => _baseWorkerRef.ProductPerSecond * _baseWorkerRef.WorkingTime
		};
	}

	public double GetDecreaseSpeed(int amount)
	{
		return _baseWorkerRef.MoveTime * (1d - _baseUpgrade.GetSpeedScale(amount));
	}

	#endregion
}
