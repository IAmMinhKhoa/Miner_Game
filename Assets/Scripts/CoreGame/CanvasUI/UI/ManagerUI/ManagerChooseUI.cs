using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ManagerChooseUI : MonoBehaviour
{
    public static Action<BoostType> OnRefreshManagerTab;
    [SerializeField] private ManagerTabUI _managerTabUI;
    [SerializeField] private ManagerSectionList _managerSectionList;
    [SerializeField] private Button _closeButton;

    [SerializeField] private List<Manager> _manager;

    void Start()
    {

    }

    void OnEnable()
    {
        _managerTabUI.onManagerTabChanged += OnManagerTabChanged;
        _closeButton.onClick.AddListener(ClosePanel);
        OnRefreshManagerTab += RefreshData;
    }

    void OnDisable()
    {
        _managerTabUI.onManagerTabChanged -= OnManagerTabChanged;
        _closeButton.onClick.RemoveListener(ClosePanel);
        OnRefreshManagerTab -= RefreshData;
    }

    private void OnManagerTabChanged(BoostType type)
    {
        if (_manager == null)
        {
            return;
        }
        _managerSectionList.ShowManagers(_manager.FindAll(x => x.BoostType == type));
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
    
    }

    public void SetupTab(BoostType type, ManagerLocation managerLocation)
    {
        SetupData(managerLocation);
        _managerTabUI.onManagerTabChanged?.Invoke(type);
    }

    public void RefreshData(BoostType type)
    {
        _manager = ManagersController.Instance.CurrentManagerLocation.LocationType switch
        {
            ManagerLocation.Shaft => ManagersController.Instance.ShaftManagers,
            ManagerLocation.Elevator => ManagersController.Instance.ElevatorManagers,
            ManagerLocation.Counter => ManagersController.Instance.CounterManagers,
        };

        OnManagerTabChanged(type);        
    }

    private void ClosePanel()
    {
        gameObject.SetActive(false);
    }
}
