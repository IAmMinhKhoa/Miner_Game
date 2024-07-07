using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CouterUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_pawText;
    [SerializeField] private Button m_updrageButton;
    [SerializeField] private TextMeshProUGUI m_levelText;
    [SerializeField] private TextMeshProUGUI m_costText;

    private Counter m_couter;
    private CouterUpdrage m_couterUpdrage;

    void Awake()
    {
        m_couter = GetComponent<Counter>();
        m_couterUpdrage = GetComponent<CouterUpdrage>();
    }

    void Start()
    {
        m_levelText.text = "Level " + m_couterUpdrage.CurrentLevel;
        m_costText.text = Currency.DisplayCurrency(m_couterUpdrage.CurrentCost);
    }

    void Update()
    {
        m_pawText.text = Currency.DisplayCurrency(PawManager.Instance.CurrentPaw);
        m_costText.text = Currency.DisplayCurrency(m_couterUpdrage.CurrentCost);
        m_levelText.text = "Level " + m_couterUpdrage.CurrentLevel;
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
        if (PawManager.Instance.CurrentPaw >= m_couterUpdrage.CurrentCost)
        {
            m_couterUpdrage.Upgrade(1);
        }
    }

    void UpdateUpgradeButton(BaseUpgrade upgrade, int level)
    {
        if (upgrade == m_couterUpdrage)
        {
            m_levelText.text = "Level " + level;
            m_costText.text = Currency.DisplayCurrency(m_couterUpdrage.CurrentCost);
        }
    }
}
