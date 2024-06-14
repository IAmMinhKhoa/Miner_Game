using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ElevatorUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_pawText;
    [SerializeField] private Button m_upgradeButton;

    private ElevatorSystem m_elevator;
    private ElevatorUpgrade m_elevatorUpgrade;

    void Awake()
    {
        m_elevator = GetComponent<ElevatorSystem>();
        m_elevatorUpgrade = GetComponent<ElevatorUpgrade>();
    }

    void Update()
    {
        m_pawText.text = Currency.DisplayCurrency(m_elevator.ElevatorDeposit.CurrentPaw);
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
        m_elevatorUpgrade.Upgrade(1);
    }

    void UpdateUpgradeButton(BaseUpgrade upgrade, int level)
    {
        if (upgrade == m_elevatorUpgrade)
        {
            m_upgradeButton.GetComponentInChildren<TextMeshProUGUI>().text = "Level " + level;
        }
    }
}
