using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    [Header("Upgrade Panel Prefab")]
    [SerializeField] private GameObject upgradePanelPrefab;
    [SerializeField] private GameObject m_upgradePanel;

    private Shaft _shaft;
    private ShaftUpgrade _shaftUpgrade;

    #region ----Unity Methods----
    private void Start()
    {
        m_upgradePanel = Instantiate(upgradePanelPrefab, GameUI.Instance.transform);
        m_upgradePanel.SetActive(false);
    }
    private void OnEnable()
    {
        ShaftUI.OnUpdrageRequest += ShowUpgradePanel;
    }

    private void OnDisable()
    {
        ShaftUI.OnUpdrageRequest -= ShowUpgradePanel;
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
        m_upgradePanel.SetActive(true);
    }
    
    #endregion
}
