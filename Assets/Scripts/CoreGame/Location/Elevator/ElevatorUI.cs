using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using Spine.Unity;
using System;
using Spine;
using NOOD.Sound;

public class ElevatorUI : MonoBehaviour
{
    public static Action OnUpgradeRequest;
    [Header("UI Button")]
    [SerializeField] private Button m_upgradeButton;
    [SerializeField] private Button m_managerButton;
    [SerializeField] private Button m_boostButton;
    [SerializeField] private Button m_workerButton;

    [Header("UI Text")]
    [SerializeField] private TextMeshProUGUI m_pawText;
    [SerializeField] private TextMeshProUGUI m_levelText;
    [SerializeField] private TextMeshProUGUI m_costText;

    [Header("Visual object")]
    [SerializeField] private SkeletonAnimation m_refrigeratorAnimation;
    [SerializeField] private SkeletonAnimation m_bgElevator;
    private ElevatorSystem m_elevator;
    private ElevatorUpgrade m_elevatorUpgrade;

    public SkeletonAnimation BgElevator => m_bgElevator;

	// public parameter
	public Button UpgradeButton => m_upgradeButton;
	public Button ManagerButton => m_managerButton;
	void Awake()
    {
        m_elevator = GetComponent<ElevatorSystem>();
        m_elevatorUpgrade = GetComponent<ElevatorUpgrade>();

    }

    void Start()
    {
        m_elevator.OnElevatorControllerArrive += ElevatorSystem_OnElevatorControllerArriveHandler;

        m_levelText.text = m_elevatorUpgrade.CurrentLevel.ToString();
        m_costText.text = Currency.DisplayCurrency(m_elevatorUpgrade.CurrentCost);
        m_pawText.text = Currency.DisplayCurrency(m_elevator.ElevatorDeposit.CurrentPaw);
        //UpdateFrameButtonUpgrade(m_elevatorUpgrade.CurrentLevel);
    }

    void Update()
    {
        m_pawText.text = Currency.DisplayCurrency(m_elevator.ElevatorDeposit.CurrentPaw);
        m_costText.text = Currency.DisplayCurrency(m_elevatorUpgrade.CurrentCost);
        m_levelText.text = "Lv. " + m_elevatorUpgrade.CurrentLevel.ToString();
    }

    void OnEnable()
    {
        m_upgradeButton.onClick.AddListener(UpgradeRequest);
        m_managerButton.onClick.AddListener(OpenManagerPanel);
        m_boostButton.onClick.AddListener(ActiveBoost);
        m_workerButton.onClick.AddListener(AwakeWorker);
        BaseUpgrade.OnUpgrade += UpdateUpgradeButton;
    }

    void OnDisable()
    {
        m_upgradeButton.onClick.RemoveListener(UpgradeRequest);
        m_managerButton.onClick.RemoveListener(OpenManagerPanel);
        m_boostButton.onClick.RemoveListener(ActiveBoost);
        m_workerButton.onClick.RemoveListener(AwakeWorker);
        BaseUpgrade.OnUpgrade -= UpdateUpgradeButton;
        //m_elevator.OnElevatorControllerArrive -= ElevatorSystem_OnElevatorControllerArriveHandler;
    }

	public void AddManagerInteract(bool isShowing) => m_managerButton.gameObject.SetActive(isShowing);
    private async void ElevatorSystem_OnElevatorControllerArriveHandler()
    {
        m_refrigeratorAnimation.AnimationState.SetAnimation(0, "Tu nhan ly nuoc - Active", true);
        await UniTask.WaitForSeconds(4);
        m_refrigeratorAnimation.AnimationState.SetAnimation(0, "Tu nhan ly nuoc - Idle", true);
    }

    public void ShowManagerButton(bool isShow)
    {
        m_managerButton.transform.GetChild(0).gameObject.SetActive(isShow);
        if (isShow)
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
            //UpdateFrameButtonUpgrade(level);
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

    public void UpdateSkeletonData()
    {

        var skinGameData = SkinManager.Instance.SkinGameDataAsset.SkinGameData;
        m_bgElevator.skeletonDataAsset = skinGameData[InventoryItemType.ElevatorBg];
        m_bgElevator.Initialize(true);

        var controllerView = ElevatorSystem.Instance.ElevatorPrefabController.GetComponent<ElevatorControllerView>();
        var fontSkeleton = controllerView.FontElevator;
        var backSkeleton = controllerView.BackElevator;
        fontSkeleton.skeletonDataAsset = skinGameData[InventoryItemType.Elevator];
        backSkeleton.skeletonDataAsset = skinGameData[InventoryItemType.BackElevator];

        fontSkeleton.Initialize(true);
        backSkeleton.Initialize(true);
        var headSkeleton = controllerView.ElevatorHeadStaff;
        var bodySkeleton = controllerView.ElevatorBodyStaff;

        headSkeleton.skeletonDataAsset = skinGameData[InventoryItemType.ElevatorCharacter];
        bodySkeleton.skeletonDataAsset = skinGameData[InventoryItemType.ElevatorCharacter];

        headSkeleton.Initialize(true);
        bodySkeleton.Initialize(true);

    }
    public void ChangeSkin(ElevatorSkin data)
    {

		if (data == null) return;
		int.TryParse(data.idBackGround, out int bgId);
		m_bgElevator.Skeleton.SetSkin($"Skin_{bgId + 1}");


		int.TryParse(data.idFrontElevator, out int elevatorIndex);


		if (ElevatorSystem.Instance.ElevatorController.TryGetComponent<ElevatorControllerView>(out var elevatorControllerView))
		{
			//cap nhat skin thang may
			var fontSkeleton = elevatorControllerView.FontElevator.skeleton;
			var backSkeleton = elevatorControllerView.BackElevator.skeleton;

			fontSkeleton.SetSkin("Skin_" + (elevatorIndex + 1));
			backSkeleton.SetSkin("Skin_" + (elevatorIndex + 1));

			fontSkeleton.SetSlotsToSetupPose();
			backSkeleton.SetSlotsToSetupPose();

			//cap nhat nhan vat thang may
			int headIndex = int.Parse(data.characterSkin.idHead);
			int bodyIndex = int.Parse(data.characterSkin.idBody);
			var headSkeleton = elevatorControllerView.ElevatorHeadStaff.skeleton;
			var bodySkeleton = elevatorControllerView.ElevatorBodyStaff.skeleton;
			headSkeleton.SetSkin("Head/Skin_" + (headIndex + 1));
			bodySkeleton.SetSkin("Body/Skin_" + (bodyIndex + 1));
			headSkeleton.SetSlotsToSetupPose();
			bodySkeleton.SetSlotsToSetupPose();
		}

	}

    private void AwakeWorker()
    {
		//SoundManager.PlaySound(SoundEnum.mobileClickBack);
		m_elevator.AwakeWorker();
    }

    #region DEBUG
    // [Button]
    // private void AddLevel(int valueAdd)
    // {
    //     m_elevatorUpgrade.Upgrade(valueAdd);
    // }
    #endregion
}
