using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using NOOD;
using NOOD.Sound;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ManagerChooseUI : MonoBehaviour    
{
    public static Action<BoostType> OnRefreshManagerTab;
    public static Action<bool> MergeSuccess;
    

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
    [SerializeField] private GameObject _ContainerWarning;
    [SerializeField] private RectTransform _imgContent;

    [SerializeField] private List<Manager> _manager;

    void OnEnable()
    {
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
    
    private void OnManagerTabChanged(BoostType type)
    {
        if (_manager == null)
        {
            return;
        }
        _managerSectionList.ShowManagers(_manager.FindAll(x => x.BoostType == type && !x.IsAssigned));
    }

    private void OnLocationTabChanged(ManagerLocation location)
    {
        SetupData(location);
        _managerTabUI.onManagerTabChanged?.Invoke(BoostType.Speed);
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
  
 
    public void SetupTab(BoostType type, ManagerLocation managerLocation)
    {
        ManagerLocationUI.OnTabChanged?.Invoke(managerLocation);
        _managerTabUI.onManagerTabChanged?.Invoke(type);
    }

    public void RefreshData(BoostType type)
    {
        SetupData(ManagersController.Instance.CurrentManagerLocation.LocationType);
       
        _managerTabUI.onManagerTabChanged.Invoke(type);
		UpdateUI();
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
        //OnRefreshManagerTab?.Invoke(manager.BoostType);
        _hireButton.interactable = true;
    }   
    void AfterMegerManager(bool success)
    {
        if (success)
        {
			SoundManager.PlaySound(SoundEnum.mergeSuccess);
        }
        else
        {
			SoundManager.PlaySound(SoundEnum.mergeFail);
			_ContainerWarning.SetActive(true);
            _imgContent.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack);

        }
    }
    public void CloseoWarning()
    {
        _imgContent.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack).OnComplete(() =>
        {
            _ContainerWarning.SetActive(false);
        });

    }
    private void Boost()
    {
        ManagersController.Instance.BoostAllManager();
    }

    private void ClosePanel()
    {
        gameObject.SetActive(false);
    }
}
