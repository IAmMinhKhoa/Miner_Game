using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CouterUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_pawText;
    [SerializeField] private Button m_upgradeButton;

    private Couter m_couter;
    private CouterUpdrage m_couterUpdrage;

    void Awake()
    {
        m_couter = GetComponent<Couter>();
        m_couterUpdrage = GetComponent<CouterUpdrage>();
    }

    void Update()
    {
        m_pawText.text = Currency.DisplayCurrency(m_couter.CurrentDeposit.CurrentPaw);
    }

    void OnEnable()
    {
        m_upgradeButton.onClick.AddListener(CallUpgrade);
        BaseUpdrage.OnUpgrade += UpdateUpgradeButton;
    }

    void OnDisable()
    {
        m_upgradeButton.onClick.RemoveListener(CallUpgrade);
        BaseUpdrage.OnUpgrade -= UpdateUpgradeButton;
    }

    void CallUpgrade()
    {
        m_couterUpdrage.Upgrade(1);
    }

    void UpdateUpgradeButton(BaseUpdrage updrage, int level)
    {
        if (updrage == m_couterUpdrage)
        {
            m_upgradeButton.GetComponentInChildren<TextMeshProUGUI>().text = "Level " + level;
        }
    }
}
