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
    public static Action OnUpgradeRequest;
    [Header("UI Button")]
    [SerializeField] private Button m_upgradeButton;
    [SerializeField] private Button m_managerButton;
    [SerializeField] private Button m_boostButton;

    [Header("UI Text")]
    [SerializeField] private TextMeshProUGUI m_pawText;
    [SerializeField] private TextMeshProUGUI m_levelText;
    [SerializeField] private TextMeshProUGUI m_costText;

    [Header("Visual object")]
    [SerializeField] private SkeletonAnimation m_refrigeratorAnimation;
	[SerializeField] private SpriteRenderer m_bgElevator;
    private ElevatorSystem m_elevator;
    private ElevatorUpgrade m_elevatorUpgrade;

    void Awake()
    {
        m_elevator = GetComponent<ElevatorSystem>();
        m_elevatorUpgrade = GetComponent<ElevatorUpgrade>();
    }

    void Start()
    {
        m_elevator.OnElevatorControllerArrive += ElevatorSystem_OnElevatorControllerArriveHandler;

        m_levelText.text =  m_elevatorUpgrade.CurrentLevel.ToString();
        m_costText.text = Currency.DisplayCurrency(m_elevatorUpgrade.CurrentCost);
        m_pawText.text = Currency.DisplayCurrency(m_elevator.ElevatorDeposit.CurrentPaw);
        UpdateFrameButtonUpgrade(m_elevatorUpgrade.CurrentLevel);
    }

    void Update()
    {
        m_pawText.text = Currency.DisplayCurrency(m_elevator.ElevatorDeposit.CurrentPaw);
        m_costText.text = Currency.DisplayCurrency(m_elevatorUpgrade.CurrentCost);
        m_levelText.text = m_elevatorUpgrade.CurrentLevel.ToString();
    }

    void OnEnable()
    {
        m_upgradeButton.onClick.AddListener(UpgradeRequest);
        m_managerButton.onClick.AddListener(OpenManagerPanel);
        m_boostButton.onClick.AddListener(ActiveBoost);
        BaseUpgrade.OnUpgrade += UpdateUpgradeButton;
    }

    void OnDisable()
    {
        m_upgradeButton.onClick.RemoveListener(UpgradeRequest);
        m_managerButton.onClick.RemoveListener(OpenManagerPanel);
        m_boostButton.onClick.RemoveListener(ActiveBoost);
        BaseUpgrade.OnUpgrade -= UpdateUpgradeButton;
        //m_elevator.OnElevatorControllerArrive -= ElevatorSystem_OnElevatorControllerArriveHandler;
    }

    private async void ElevatorSystem_OnElevatorControllerArriveHandler()
    {
        m_refrigeratorAnimation.AnimationState.SetAnimation(0, "Tu nhan ly nuoc - Active", true);
        await UniTask.WaitForSeconds(4);
        m_refrigeratorAnimation.AnimationState.SetAnimation(0, "Tu nhan ly nuoc - Idle", true);
    }

    public void ShowManagerButton(bool isShow)
    {
        m_managerButton.transform.GetChild(0).gameObject.SetActive(isShow);
        if(isShow)
        {
            m_managerButton.image.color = new Color(1, 1, 1, 1);
        }
        else
        {
            m_managerButton.image.color = new Color(1, 1, 1, 0);
        }
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
            UpdateFrameButtonUpgrade(level);
        }
    }
    void UpdateFrameButtonUpgrade(int currentLevel)
    {

        Image imgButtonUpgrade = m_upgradeButton.GetComponent<Image>();
        if (currentLevel <= 600)
        {
            imgButtonUpgrade.sprite = Resources.Load<Sprite>(MainGameData.FrameLevelButton[ManagerLocation.Elevator][0]);
        }
        else if (currentLevel > 600 && currentLevel <= 1200)
        {
            imgButtonUpgrade.sprite = Resources.Load<Sprite>(MainGameData.FrameLevelButton[ManagerLocation.Elevator][1]);
        }
        else if (currentLevel > 1200 && currentLevel <= 1800)
        {
            imgButtonUpgrade.sprite = Resources.Load<Sprite>(MainGameData.FrameLevelButton[ManagerLocation.Elevator][2]);
        }
        else if (currentLevel > 1800 && currentLevel <= 2400)
        {
            imgButtonUpgrade.sprite = Resources.Load<Sprite>(MainGameData.FrameLevelButton[ManagerLocation.Elevator][3]);
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

    public void UpgradeRequest()
    {
        OnUpgradeRequest?.Invoke();
    }

	public void ChangeSkin(ElevatorSkin data)
	{
		//set init Data Skin shaft
		//Debug.Log(ElevatorSystem.Instance.elevatorSkin);
		m_bgElevator.sprite = SkinManager.Instance.skinResource.skinBgElevator[int.Parse(data.idBackGround)].sprite; ; 
		/*  m_waitTable.sprite = SkinManager.skinResource.skinWaitTable[int.Parse(data.idBackGround)].sprite;*/
		
	}

	#region DEBUG
	// [Button]
	// private void AddLevel(int valueAdd)
	// {
	//     m_elevatorUpgrade.Upgrade(valueAdd);
	// }
	#endregion
}
