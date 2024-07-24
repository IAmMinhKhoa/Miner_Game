using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NOOD;
using System;

public class UpgradeManager : Patterns.Singleton<UpgradeManager>
{
    public Action<int> OnUpdrageRequest;

    [Header("Upgrade Panel Prefab")]
    [SerializeField] private UpgradeUI m_upgradePanel;


    private Shaft _shaft;
    private BaseUpgrade _baseUpgrade;

    #region ----Unity Methods----
    private void Start()
    {
        m_upgradePanel = Instantiate(m_upgradePanel, GameUI.Instance.transform);
        m_upgradePanel.gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        ShaftUI.OnUpgradeRequest += ShowUpgradePanel;
        OnUpdrageRequest += OnUpgradeAction;
        BaseUpgrade.OnUpgradeSuccess += ResertPanel;
    }

    private void OnDisable()
    {
        ShaftUI.OnUpgradeRequest -= ShowUpgradePanel;
        OnUpdrageRequest -= OnUpgradeAction;
        BaseUpgrade.OnUpgradeSuccess -= ResertPanel;
    }
    #endregion

    #region ----Methods----
    private void ShowUpgradePanel(int index)
    {
        List<Shaft> shafts = ShaftManager.Instance.Shafts;
        foreach (var shaft in shafts)
        {
            if (shaft.shaftIndex == index)
            {
                _shaft = shaft;
                _baseUpgrade = shaft.GetComponent<ShaftUpgrade>();
                break;
            }
        }

        ResertPanel();
    }

    private void ControlPannel(bool open)
    {
        m_upgradePanel.gameObject.SetActive(open);
    }

    private void ResertPanel()
    {
        m_upgradePanel.SetUpPanel(CalculateUpgradeAmount());
        ControlPannel(true);
    }

    private void OnUpgradeAction(int amount)
    {
        if (_shaft != null)
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
    #endregion
    }
