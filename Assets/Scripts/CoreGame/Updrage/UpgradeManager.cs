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
    [SerializeField] private UpgradeUI m_upgradePanel;
    private BaseUpgrade _baseUpgrade;
    private ManagerLocation _locationType;
    private BaseWorker _baseWorkerRef;
    private int _number;

    #region ----Unity Methods----
    private void Start()
    {
        m_upgradePanel = Instantiate(m_upgradePanel, GameUI.Instance.transform);
        m_upgradePanel.gameObject.SetActive(false);
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
                _baseWorkerRef = shaft.Brewers.First();
                _number = shaft.Brewers.Count;
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
        _baseWorkerRef = Counter.Instance.Transporters.First();
        _number = Counter.Instance.Transporters.Count;
        ResetPanel();
    }

    private void ControlPanel(bool open)
    {
        m_upgradePanel.gameObject.SetActive(open);
    }

    private void ResetPanel()
    {
        m_upgradePanel.SetUpPanel(CalculateUpgradeAmount());
        switch (_locationType)
        {
            case ManagerLocation.Shaft:
                m_upgradePanel.SetWorkerInfo(_locationType, "Mèo đáng yêu", _baseWorkerRef.ProductPerSecond, _number.ToString(), _baseWorkerRef.ProductPerSecond * _number * _baseWorkerRef.WorkingTime, _baseUpgrade.CurrentLevel);
                break;
            case ManagerLocation.Elevator:
                m_upgradePanel.SetWorkerInfo(_locationType, "Chó đáng yêu", _baseWorkerRef.ProductPerSecond, _baseWorkerRef.MoveTime.ToString("F2"), _baseWorkerRef.ProductPerSecond * _baseWorkerRef.WorkingTime, _baseUpgrade.CurrentLevel);
                break;
            case ManagerLocation.Counter:
                m_upgradePanel.SetWorkerInfo(_locationType, "Mèo đáng yêu", _baseWorkerRef.ProductPerSecond, _number.ToString(), _baseWorkerRef.ProductPerSecond * _number * _baseWorkerRef.WorkingTime, _baseUpgrade.CurrentLevel);
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
    #endregion
    }
