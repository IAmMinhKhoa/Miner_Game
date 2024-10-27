using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
	void OnEnable()
    {
		FadeInContainer();
        _managerTabUI.onManagerTabChanged += OnManagerTabChanged;
        ManagerLocationUI.OnTabChanged += OnLocationTabChanged;
        _closeButton.onClick.AddListener(ClosePanel);
        _hireButton.onClick.AddListener(HireManager);
        _boostButton.onClick.AddListener(Boost);
        OnRefreshManagerTab += RefreshData;
        MergeSuccess += AfterMegerManager;
		PawManager.Instance.OnPawChanged += UpdateUI;

		

	}

    void OnDisable()
    {
        _managerTabUI.onManagerTabChanged -= OnManagerTabChanged;
        ManagerLocationUI.OnTabChanged -= OnLocationTabChanged;
        _closeButton.onClick.RemoveListener(ClosePanel);
        _hireButton.onClick.RemoveListener(HireManager);
        _boostButton.onClick.RemoveListener(Boost);
        OnRefreshManagerTab -= RefreshData;
        MergeSuccess -= AfterMegerManager;
		PawManager.Instance.OnPawChanged -= UpdateUI;
	}
    
    private void OnManagerTabChanged(BoostType type,bool forceAnimation=true)
    {
		Debug.Log("khoa:" + _manager.Count+"/"+ currentLocation);
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
        _managerTabUI.onManagerTabChanged?.Invoke(BoostType.Speed, true);
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
	}
	void UpdateUI(double value=0)
	{
		if (PawManager.Instance.CurrentPaw < ManagersController.Instance.CurrentCost)
		{
			_hireButton.interactable = false;
			return;
		}
		else
		{
			_hireButton.interactable = true;
		}
	}

    void HireManager()
    {
        if (PawManager.Instance.CurrentPaw < ManagersController.Instance.CurrentCost)
        {
			UpdateUI();
			return;
        }
        
        _hireButton.interactable = false;
        Debug.Log("Hire Manager");
        var manager = ManagersController.Instance.CreateManager();
		FxGacha.OpenFxGacha(manager);

		_hireButton.interactable = true;
    }   
    void AfterMegerManager(TypeMerge typeMerge)
    {
		switch (typeMerge)
		{
			case TypeMerge.Success:
				SoundManager.PlaySound(SoundEnum.mergeSuccess);
				break;

			case TypeMerge.FailLevelMax:
				SoundManager.PlaySound(SoundEnum.mergeFail);
				NotiWarning.OpenModal("không thể hợp nhất \n2 nhân vật đã đạt cấp độ cao nhất");
				break;

			case TypeMerge.FailNotSameLevel:
				SoundManager.PlaySound(SoundEnum.mergeFail);
				NotiWarning.OpenModal("không thể hợp nhất \n2 nhân vật có cấp độ khác nhau");
				break;

			default:
				// Handle any other cases if needed
				break;
		}

	}
	private void Boost()
    {
        ManagersController.Instance.BoostAllManager();
    }

    private void ClosePanel()
    {
		FadeOutContainer();
    }

	#region AnimateUI
	[Button]
	public void FadeInContainer()
	{
		gameObject.SetActive(true);
		Vector2 posCam = CustomCamera.Instance.GetCurrentTransform().position;
		gameObject.transform.localPosition = new Vector2(posCam.x - 2000, posCam.y); //Left Screen
		gameObject.transform.DOLocalMoveX(0, 0.6f).SetEase(Ease.OutQuart);


	}
	[Button]
	public void FadeOutContainer()
	{
		Vector2 posCam = CustomCamera.Instance.GetCurrentTransform().position;
		gameObject.transform.DOLocalMoveX(posCam.x - 2000f, 0.5f).SetEase(Ease.InQuart).OnComplete(() =>
		{
			gameObject.SetActive(false);
		});

	}
	#endregion
}
