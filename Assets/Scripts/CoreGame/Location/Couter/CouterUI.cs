using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CouterUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_pawText;
    [SerializeField] private Button m_updrageButton;

    private Couter m_couter;
    private CouterUpdrage m_couterUpdrage;

    void Awake()
    {
        m_couter = GetComponent<Couter>();
        m_couterUpdrage = GetComponent<CouterUpdrage>();
    }

    void Update()
    {
        m_pawText.text = Currency.DisplayCurrency(m_couter.CouterDeposit.CurrentPaw);
    }

    void OnEnable()
    {
        m_updrageButton.onClick.AddListener(CallUpgrade);
        BaseUpgrade.OnUpgrade += UpdateUpgradeButton;
    }

    void OnDisable()
    {
        m_updrageButton.onClick.RemoveListener(CallUpgrade);
        BaseUpgrade.OnUpgrade -= UpdateUpgradeButton;
    }

    void CallUpgrade()
    {
        m_couterUpdrage.Upgrade(1);
    }

    void UpdateUpgradeButton(BaseUpgrade upgrade, int level)
    {
        if (upgrade == m_couterUpdrage)
        {
            m_updrageButton.GetComponentInChildren<TextMeshProUGUI>().text = "Level " + level;
        }
    }
}
