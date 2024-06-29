using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

public class ElevatorUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_pawText;
    [SerializeField] private Button m_upgradeButton;
    [SerializeField] private TextMeshProUGUI m_levelText;
    [SerializeField] private TextMeshProUGUI m_costText;

    private ElevatorSystem m_elevator;
    private ElevatorUpgrade m_elevatorUpgrade;

    void Awake()
    {
        m_elevator = GetComponent<ElevatorSystem>();
        m_elevatorUpgrade = GetComponent<ElevatorUpgrade>();
    }

    void Start()
    {
        m_levelText.text = "Level " + m_elevatorUpgrade.CurrentLevel;
        m_costText.text = Currency.DisplayCurrency(m_elevatorUpgrade.CurrentCost);
        m_pawText.text = Currency.DisplayCurrency(m_elevator.ElevatorDeposit.CurrentPaw);
    }
    void Update()
    {
        m_pawText.text = Currency.DisplayCurrency(m_elevator.ElevatorDeposit.CurrentPaw);
        m_costText.text = Currency.DisplayCurrency(m_elevatorUpgrade.CurrentCost);
        m_levelText.text = "Level " + m_elevatorUpgrade.CurrentLevel;
    }

    void OnEnable()
    {
        m_upgradeButton.onClick.AddListener(CallUpgrade);
        BaseUpgrade.OnUpgrade += UpdateUpgradeButton;
    }

    void OnDisable()
    {
        m_upgradeButton.onClick.RemoveListener(CallUpgrade);
        BaseUpgrade.OnUpgrade -= UpdateUpgradeButton;
    }

    void CallUpgrade()
    {
        if (PawManager.Instance.CurrentPaw >= m_elevatorUpgrade.CurrentCost)
        {
            m_elevatorUpgrade.Upgrade(1);
        }
    }

    void UpdateUpgradeButton(BaseUpgrade upgrade, int level)
    {
        if (upgrade == m_elevatorUpgrade)
        {
            m_levelText.text = "Level " + level;
            m_costText.text = Currency.DisplayCurrency(m_elevatorUpgrade.CurrentCost);
        }
    }
}
