using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using DG.Tweening;
using NOOD;
using NOOD.Sound;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



public enum TypeMerge
{
	Success,
	FailLevelMax,
	FailNotSameLevel
}
public class ManagerChooseUI : MonoBehaviour
{
    public static Action<BoostType,bool> OnRefreshManagerTab;
    public static Action<TypeMerge> MergeSuccess;


	private ManagerLocation currentLocation;

    [SerializeField] private ManagerTabUI _managerTabUI;
    [SerializeField] private ManagerSectionList _managerSectionList;
	[SerializeField] public ManagerLocationUI _locationTabUI;
	[SerializeField] public ScrollRect _scrollRect;
    [Header("UI text")]
    [SerializeField] private TextMeshProUGUI _currentCostText;

    [Header("UI Button")]
    [SerializeField] private Button _closeButton;
    [SerializeField] private Button _hireButton;
    [SerializeField] private Button _boostButton;

    [Header("UI Another")]
    [SerializeField] private CanvasGroup _canvasGrList;
    [SerializeField] private List<Manager> _manager;
	[SerializeField] DetailNotification NotiWarning;
	[SerializeField] GachaController FxGacha;
	public ManagerSelectionShaft _managerSelection;
	[Header("UI for Tutorial")]
	[SerializeField] List<GameObject> topUIButton;
	[SerializeField] List<GameObject> botUIButton;
	[SerializeField] Image tutorialLine;

	public Action OnInteractToTutorialManager;
	public Button HireManagerButton => _hireButton;
	public ManagerTabUI ManagerTabUI => _managerTabUI;
	public ManagerSectionList ManagerSectionList => _managerSectionList;

	bool isclosableUI = true;


	void OnEnable()
    {
		FadeInContainer();
		_managerTabUI.onManagerTabChanged += OnManagerTabChanged;
        ManagerLocationUI.OnTabChanged += OnLocationTabChanged;
        _closeButton.onClick.AddListener(ClosePanel);
        _hireButton.onClick.AddListener(OnHireManagerButtonClicked);
        _boostButton.onClick.AddListener(Boost);
        OnRefreshManagerTab += RefreshData;
        MergeSuccess += AfterMegerManager;

		PawManager.Instance.OnPawChanged += UpdateUI;

		if(!TutorialManager.Instance.isTuroialing)
		{
			ShowActiveAllButton();
		}




	}

    void OnDisable()
    {
        _managerTabUI.onManagerTabChanged -= OnManagerTabChanged;
        ManagerLocationUI.OnTabChanged -= OnLocationTabChanged;
        _closeButton.onClick.RemoveListener(ClosePanel);
        _hireButton.onClick.RemoveListener(OnHireManagerButtonClicked);
        _boostButton.onClick.RemoveListener(Boost);
        OnRefreshManagerTab -= RefreshData;
        MergeSuccess -= AfterMegerManager;

		PawManager.Instance.OnPawChanged -= UpdateUI;
	}

	public void AddListenerFromFXGacha()
	{
		FxGacha.OnUIClose += HandleCloseEventFromFxGacha;
	}
	private void RemoveListenerFromFxGacha()
	{
		FxGacha.OnUIClose -= HandleCloseEventFromFxGacha;
	}

	private void HandleCloseEventFromFxGacha()
	{

		OnInteractToTutorialManager?.Invoke();
		RemoveListenerFromFxGacha();
	}
	public void OpenTutorialLine()
	{
		isclosableUI = false;
		tutorialLine.gameObject.SetActive(true);

	}
	public void CloseTutorialLine()
	{
		isclosableUI = true;
		tutorialLine.gameObject.SetActive(false);
	}

    private void OnManagerTabChanged(BoostType type,bool forceAnimation=true)
    {


		if (_manager == null)
        {
            return;
        }
		_managerSectionList.ShowManagers(
	   _manager.FindAll(x => x.BoostType == type
		   && ((x.LocationType == ManagerLocation.Shaft && !x.IsAssigned)
		   || x.LocationType != ManagerLocation.Shaft)),
	   forceAnimation);

	}

	private void OnLocationTabChanged(ManagerLocation location)
    {
		Debug.Log("khoa OnLocationTabChanged:" + location);
		currentLocation = location;
		UpdateUI(PawManager.Instance.CurrentPaw);
		SetupData(location);
        _managerTabUI.onManagerTabChanged?.Invoke(CheckListManager(), true);
    }
	private BoostType CheckListManager()
	{
		if(!TutorialManager.Instance.isTuroialing)
		{
			for (int i = 0; i < botUIButton.Count; i++)
			{
				ShowHideBotButton(true, i);
			}
			return BoostType.Speed;
		}
		BoostType curTypeManager = BoostType.None;

		for (int i = 0; i < botUIButton.Count; i++)
		{
			ShowHideBotButton(false, i);
		}

		foreach (var manager in _manager)
		{
			switch (manager.BoostType)
			{
				case BoostType.Speed:
					ShowHideBotButton(true, 0);
					OnManagerTabChanged(BoostType.Speed);
					break;
				case BoostType.Costs:
					ShowHideBotButton(true, 1);
					OnManagerTabChanged(BoostType.Costs);
					break;
				case BoostType.Efficiency:
					ShowHideBotButton(true, 2);
					OnManagerTabChanged(BoostType.Efficiency);
					break;
			}

			if (curTypeManager == BoostType.None)
			{
				curTypeManager = manager.BoostType;
			}
		}

		return curTypeManager == BoostType.None ? BoostType.Speed : curTypeManager;
	}
	public void SetupData(ManagerLocation location)
    {
        _manager = location switch
        {
            ManagerLocation.Shaft => ManagersController.Instance.ShaftManagers,
            ManagerLocation.Elevator => ManagersController.Instance.ElevatorManagers,
            ManagerLocation.Counter => ManagersController.Instance.CounterManagers,
            _ => throw new ArgumentOutOfRangeException(nameof(location), location, null)
        };
        Debug.Log("SetupData");

        _currentCostText.text = Currency.DisplayCurrency(ManagersController.Instance.CurrentCost);

    }


    public void SetupTab(BoostType type, ManagerLocation managerLocation,bool foceAnimation=true)
    {
        ManagerLocationUI.OnTabChanged?.Invoke(managerLocation);
        _managerTabUI.onManagerTabChanged?.Invoke(type, foceAnimation);
    }

    public void RefreshData(BoostType type, bool foceAnimation = true)
    {
		Debug.Log("refesh :" + type);
        SetupData(ManagersController.Instance.CurrentManagerLocation.LocationType);

        _managerTabUI.onManagerTabChanged?.Invoke(type, foceAnimation);
		UpdateUI(); //update current paw -> disable button gacha
		CheckListManager();
	}
	void UpdateUI(double value=0)
	{
		/*if (PawManager.Instance.CurrentPaw < ManagersController.Instance.CurrentCost)
		{
			_hireButton.interactable = false;
			return;
		}
		else
		{
			_hireButton.interactable = true;
		}*/
	}
	private async void OnHireManagerButtonClicked()
	{

		await HireManager();
	}

	async Task HireManager()
    {
        if (PawManager.Instance.CurrentPaw < ManagersController.Instance.CurrentCost)
        {
	        SoundManager.PlaySound(SoundEnum.mergeFail);
			UpdateUI();
			return;
        }
        SoundManager.PlaySound(SoundEnum.click);
        StartCoroutine(Common.IeDoSomeThing(0.5f, () =>
        {
	        SoundManager.PlaySound(SoundEnum.gacha);
        }));
        _hireButton.interactable = false;
        Debug.Log("Hire Manager");
        var manager = ManagersController.Instance.CreateManager();

		FxGacha.OpenFxGacha(manager);

		_hireButton.interactable = true;

		await Task.Delay(1500);
		OnRefreshManagerTab?.Invoke(manager.BoostType, false);
	}
    void AfterMegerManager(TypeMerge typeMerge)
    {
		switch (typeMerge)
		{
			case TypeMerge.Success:
				SoundManager.PlaySound(SoundEnum.appsuccess1);
				break;

			case TypeMerge.FailLevelMax:
				SoundManager.PlaySound(SoundEnum.accessdenied);
				NotiWarning.OpenModal("không thể hợp nhất \n2 nhân vật đã đạt cấp độ cao nhất");
				break;

			case TypeMerge.FailNotSameLevel:
				SoundManager.PlaySound(SoundEnum.accessdenied);
				NotiWarning.OpenModal("không thể hợp nhất \n2 nhân vật có cấp độ khác nhau");
				break;

			default:
				// Handle any other cases if needed
				break;
		}

	}
	private void Boost()
    {
		if (ManagersController.Instance.HasAnyManager())
		{
			SoundManager.PlaySound(SoundEnum.cartoonButton1);
		}
		else
		{
			SoundManager.PlaySound(SoundEnum.clickdecline);
		}

		ManagersController.Instance.BoostAllManager();
	}

    public void ClosePanel()
    {
		if (!isclosableUI) return;
		SoundManager.PlaySound(SoundEnum.mobileTexting2);
		FadeOutContainer();
    }

	//Hien thi cac UI trong tabs manager
	public void ShowActiveAllButton()
	{
		SetCanvasGroup(_boostButton.gameObject, true);
	}
	private void SetCanvasGroup(GameObject obj, bool isShow)
	{
		if (obj == null) return;
		var canvasGroup = obj.GetComponent<CanvasGroup>();
		if (canvasGroup != null)
		{
			canvasGroup.alpha = isShow ? 1 : 0;
			canvasGroup.interactable = isShow;
		}
	}

	private void SetCanvasGroupList(List<GameObject> buttons, bool isShow)
	{
		foreach (var button in buttons)
		{
			SetCanvasGroup(button, isShow);
		}
	}

	public void HideShowAllUITabManagerUI(bool isShow)
	{
		SetCanvasGroupList(topUIButton, isShow);
		SetCanvasGroupList(botUIButton, isShow);
		SetCanvasGroup(_boostButton.gameObject, isShow);
	}

	public void ShowHideTopButton(bool isShow, int index)
	{
		if (index >= 0 && index < topUIButton.Count)
		{
			SetCanvasGroup(topUIButton[index], isShow);
		}
	}

	public void ShowHideBotButton(bool isShow, int index)
	{
		if (index >= 0 && index < botUIButton.Count)
		{
			SetCanvasGroup(botUIButton[index], isShow);
		}
	}

	#region AnimateUI
	[Button]
	public void FadeInContainer()
	{

		//gameObject.SetActive(true);
		Vector2 posCam = CustomCamera.Instance.GetCurrentTransform().position;
		gameObject.transform.localPosition = new Vector2(posCam.x - 2000, posCam.y); //Left Screen
		gameObject.transform.DOLocalMoveX(0, 0.5f).SetEase(Ease.OutQuart).OnComplete(() =>
		{
			if (TutorialManager.Instance.isTuroialing)
			{
				OnInteractToTutorialManager?.Invoke();
			}
		}
		);
	}
	[Button]
	public void FadeOutContainer()
	{
		Vector2 posCam = CustomCamera.Instance.GetCurrentTransform().position;
		gameObject.transform.DOLocalMoveX(posCam.x - 2000f, 0.5f).SetEase(Ease.InQuart).OnComplete(() =>
		{
			if (TutorialManager.Instance.isTuroialing)
			{
				OnInteractToTutorialManager?.Invoke();
			}
			gameObject.SetActive(false);
		});

	}
	#endregion
}
