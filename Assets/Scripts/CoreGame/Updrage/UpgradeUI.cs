using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NOOD;
using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour
{
    [Header("Upgrade Panel Elements")]
    [SerializeField] private GameObject m_upgradePanel;
    [SerializeField] private Button closeButton;

    private Shaft _shaft;
    private ShaftUpgrade _shaftUpgrade;

    #region ----Unity Methods----
    private void Start()
    {
        m_upgradePanel.transform.SetParent(GameUI.Instance.transform, false);
        m_upgradePanel.SetActive(false);
    }
    private void OnEnable()
    {
        ShaftUI.OnUpdrageRequest += ShowUpgradePanel;
        closeButton.onClick.AddListener(ClosePanel);
    }

    private void OnDisable()
    {
        ShaftUI.OnUpdrageRequest -= ShowUpgradePanel;
        closeButton.onClick.RemoveListener(ClosePanel);
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

    void ClosePanel()
    {
        ControlPannel(false);
    }
    
    #endregion
}
