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
    private ElevatorUpdrage m_elevatorUpdrage;

    void Awake()
    {
        m_elevator = GetComponent<ElevatorSystem>();
        m_elevatorUpdrage = GetComponent<ElevatorUpdrage>();
    }

    void Update()
    {
        m_pawText.text = Currency.DisplayCurrency(m_elevator.ElevatorDeposit.CurrentPaw);
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
        m_elevatorUpdrage.Upgrade(1);
    }

    void UpdateUpgradeButton(BaseUpdrage updrage, int level)
    {
        if (updrage == m_elevatorUpdrage)
        {
            m_upgradeButton.GetComponentInChildren<TextMeshProUGUI>().text = "Level " + level;
        }
    }
}
