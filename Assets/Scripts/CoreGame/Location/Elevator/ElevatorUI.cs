using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

public class ElevatorUI : MonoBehaviour
{
    [Header("UI Button")]
    [SerializeField] private Button m_upgradeButton;
    [SerializeField] private Button m_managerButton;
    [SerializeField] private Button m_boostButton;

    [Header("UI Text")]
    [SerializeField] private TextMeshProUGUI m_pawText;
    [SerializeField] private TextMeshProUGUI m_levelText;
    [SerializeField] private TextMeshProUGUI m_costText;

    [Header("Visual object")]
    [SerializeField] private GameObject m_quayNhanLyNuocHolder;
    [SerializeField] private GameObject m_lyNuocPref;

    private ElevatorSystem m_elevator;
    private ElevatorUpgrade m_elevatorUpgrade;

    void Awake()
    {
        m_elevator = GetComponent<ElevatorSystem>();
        m_elevatorUpgrade = GetComponent<ElevatorUpgrade>();
    }

    void Start()
    {
        m_levelText.text =  m_elevatorUpgrade.CurrentLevel.ToString();
        m_costText.text = Currency.DisplayCurrency(m_elevatorUpgrade.CurrentCost);
        m_pawText.text = Currency.DisplayCurrency(m_elevator.ElevatorDeposit.CurrentPaw);
    }
    void Update()
    {
        m_pawText.text = Currency.DisplayCurrency(m_elevator.ElevatorDeposit.CurrentPaw);
        m_costText.text = Currency.DisplayCurrency(m_elevatorUpgrade.CurrentCost);
        m_levelText.text =  m_elevatorUpgrade.CurrentLevel.ToString();
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

    void ActiveBoost()
    {
        m_elevator.RunBoost();
    }

    void OpenManagerPanel()
    {
        ManagersController.Instance.OpenManagerPanel(m_elevator.ManagerLocation);
    }
}
