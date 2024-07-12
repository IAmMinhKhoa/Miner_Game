using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NOOD;
using System;

public class UpgradeManager : MonoBehaviour
{
    public static Action<int> OnUpdrageRequest;

    [Header("Upgrade Panel Prefab")]
    [SerializeField] private GameObject m_upgradePanel;


    private Shaft _shaft;
    private ShaftUpgrade _shaftUpgrade;

    #region ----Unity Methods----
    private void Start()
    {
        m_upgradePanel = Instantiate(m_upgradePanel, GameUI.Instance.transform);
        m_upgradePanel.SetActive(false);
    }
    private void OnEnable()
    {
        ShaftUI.OnUpdrageRequest += ShowUpgradePanel;
        UpgradeManager.OnUpdrageRequest += OnUpgradeAction;
    }

    private void OnDisable()
    {
        ShaftUI.OnUpdrageRequest -= ShowUpgradePanel;
        UpgradeManager.OnUpdrageRequest -= OnUpgradeAction;
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
                _shaftUpgrade = shaft.GetComponent<ShaftUpgrade>();
                break;
            }
        }
        ControlPannel(true);
    }

    private void ControlPannel(bool open)
    {
        m_upgradePanel.SetActive(open);
    }

    private void OnUpgradeAction(int amount)
    {
        if (_shaft != null)
        {
            if (PawManager.Instance.CurrentPaw >= _shaftUpgrade.CurrentCost)
            {
                PawManager.Instance.RemovePaw(_shaftUpgrade.CurrentCost);
                _shaftUpgrade.Upgrade(amount);
            }
            else
            {
                Debug.Log("Not enough paw");
            }
            ControlPannel(false);
        }
    }
        #endregion
    }
