using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using Spine.Unity;
using System;

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
    private ElevatorController m_elevatorController;
    private ElevatorUpgrade m_elevatorUpgrade;
    private SkeletonAnimation _frontElevator, _backElevator, _elevatorStaff;
    private Vector3 _target;

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
        m_levelText.text = m_elevatorUpgrade.CurrentLevel.ToString();
        if (m_elevatorController != null && m_elevatorController.IsArrive)
        {
            _frontElevator.AnimationState.SetAnimation(0, "Thangmay - Idle", true);
            _backElevator.AnimationState.SetAnimation(0, "Thangmay - Idle", true);
        }
    }

    void OnEnable()
    {
        m_upgradeButton.onClick.AddListener(CallUpgrade);
        m_managerButton.onClick.AddListener(OpenManagerPanel);
        m_boostButton.onClick.AddListener(ActiveBoost);
        m_elevator.OnCreateElevatorController += OnCreateElevatorControllerHandler;
        BaseUpgrade.OnUpgrade += UpdateUpgradeButton;
    }

    void OnDisable()
    {
        m_upgradeButton.onClick.RemoveListener(CallUpgrade);
        m_managerButton.onClick.RemoveListener(OpenManagerPanel);
        m_boostButton.onClick.RemoveListener(ActiveBoost);
        m_elevator.OnCreateElevatorController -= OnCreateElevatorControllerHandler;
        BaseUpgrade.OnUpgrade -= UpdateUpgradeButton;
    }

    private void OnCreateElevatorControllerHandler(ElevatorController controller)
    {
        m_elevatorController = controller;
        m_elevatorController.OnMoveToTarget += OnMoveToTargetHandler;
        _frontElevator = m_elevatorController.FrontElevator;
        _backElevator = m_elevatorController.BackElevator;
        _elevatorStaff = m_elevatorController.ElevatorStaff;
    }

    private void OnMoveToTargetHandler(Vector3 vector)
    {
        _target = vector;
        if(this.transform.position.y > vector.y)
        {
            // Move down
            _frontElevator.AnimationState.SetAnimation(0, "Thangmay - Down", true);
            _backElevator.AnimationState.SetAnimation(0, "Thangmay - Down", true);
        }
        else
        {
            // Move up
            _frontElevator.AnimationState.SetAnimation(0, "Thangmay - Up", true);
            _backElevator.AnimationState.SetAnimation(0, "Thangmay - Up", true);
        }
        _elevatorStaff.AnimationState.SetAnimation(0, "Idle", true);
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
