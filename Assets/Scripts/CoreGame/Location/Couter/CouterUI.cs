using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CounterUI : MonoBehaviour
{
    [Header("UI Button")]
    [SerializeField] private Button m_upgradeButton;
    [SerializeField] private Button m_managerButton;
    [SerializeField] private Button m_boostButton;

    [Header("UI Text")]
    [SerializeField] private TextMeshProUGUI m_pawText;
    [SerializeField] private TextMeshProUGUI m_levelText;
    [SerializeField] private TextMeshProUGUI m_costText;

    // [Header("Visual object")]
    // [SerializeField] private GameObject m_quayGiaoNuocHolder;

    private Counter m_counter;
    private CounterUpgrade m_counterUpgrade;

    void Awake()
    {
        m_counter = GetComponent<Counter>();
        m_counterUpgrade = GetComponent<CounterUpgrade>();
    }

    void Start()
    {
        m_levelText.text =  m_counterUpgrade.CurrentLevel.ToString();
        m_costText.text = Currency.DisplayCurrency(m_counterUpgrade.CurrentCost);
    }

    void Update()
    {
        m_pawText.text = Currency.DisplayCurrency(PawManager.Instance.CurrentPaw);
        m_costText.text = Currency.DisplayCurrency(m_counterUpgrade.CurrentCost);
        m_levelText.text =  m_counterUpgrade.CurrentLevel.ToString();
    }

    void OnEnable()
    {
        m_upgradeButton.onClick.AddListener(CallUpgrade);
        m_managerButton.onClick.AddListener(OpenManagerPanel);
        m_boostButton.onClick.AddListener(ActiveBoost);
        BaseUpgrade.OnUpgrade += UpdateUpgradeButton;
    }

    void OnDisable()
    {
        m_upgradeButton.onClick.RemoveListener(CallUpgrade);
        m_managerButton.onClick.RemoveListener(OpenManagerPanel);
        m_boostButton.onClick.RemoveListener(ActiveBoost);
        BaseUpgrade.OnUpgrade -= UpdateUpgradeButton;
    }

    void CallUpgrade()
    {
        if (PawManager.Instance.CurrentPaw >= m_counterUpgrade.CurrentCost)
        {
            m_counterUpgrade.Upgrade(1);
        }
    }

    void UpdateUpgradeButton(BaseUpgrade upgrade, int level)
    {
        if (upgrade == m_counterUpgrade)
        {
            m_levelText.text = "Level " + level;
            m_costText.text = Currency.DisplayCurrency(m_counterUpgrade.CurrentCost);
        }
    }

    void ActiveBoost()
    {
        m_counter.RunBoost();
    }

    void OpenManagerPanel()
    {
        ManagersController.Instance.OpenManagerPanel(m_counter.ManagerLocation);
    }
}
