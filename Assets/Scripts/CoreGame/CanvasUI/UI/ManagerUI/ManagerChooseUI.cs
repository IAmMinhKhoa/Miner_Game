using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NOOD;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ManagerChooseUI : MonoBehaviour    
{
    public static Action<BoostType> OnRefreshManagerTab;
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

    void OnEnable()
    {
        _managerTabUI.onManagerTabChanged += OnManagerTabChanged;
        ManagerLocationUI.OnTabChanged += OnLocationTabChanged;
        _closeButton.onClick.AddListener(ClosePanel);
        _hireButton.onClick.AddListener(HireManager);
        _boostButton.onClick.AddListener(Boost);
        OnRefreshManagerTab += RefreshData;
    }

    void OnDisable()
    {
        _managerTabUI.onManagerTabChanged -= OnManagerTabChanged;
        ManagerLocationUI.OnTabChanged -= OnLocationTabChanged;
        _closeButton.onClick.RemoveListener(ClosePanel);
        _hireButton.onClick.RemoveListener(HireManager);
        _boostButton.onClick.RemoveListener(Boost);
        OnRefreshManagerTab -= RefreshData;
    }
    
    private void OnManagerTabChanged(BoostType type)
    {
        if (_manager == null)
        {
            return;
        }
        DoFaceList(() =>
        {
            _managerSectionList.ShowManagers(_manager.FindAll(x => x.BoostType == type && !x.IsAssigned));
        });
        
    }

    private void OnLocationTabChanged(ManagerLocation location)
    {
        DoFaceList(() =>
        {
            SetupData(location);
            _managerTabUI.onManagerTabChanged?.Invoke(BoostType.Speed);
        });
        
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
    private void DoFaceList(Action renderUI)
    {

        StartCoroutine(IeDoFadeList(_canvasGrList,
            renderUI
            ,0.3f,0.2f));
    }
    private IEnumerator IeDoFadeList(CanvasGroup group, Action renderUI, float timeIN,float timeOut)
    {
        yield return Common.FadeOut(group, timeOut);
        renderUI?.Invoke();
        yield return new WaitForSeconds(0.1f);
        yield return Common.FadeIn(group, timeIN);
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
    }

    void HireManager()
    {
        if (PawManager.Instance.CurrentPaw < ManagersController.Instance.CurrentCost)
        {
            return;
        }
        
        _hireButton.interactable = false;
        Debug.Log("Hire Manager");
        var manager = ManagersController.Instance.CreateManager();
        //OnRefreshManagerTab?.Invoke(manager.BoostType);
        _hireButton.interactable = true;
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
