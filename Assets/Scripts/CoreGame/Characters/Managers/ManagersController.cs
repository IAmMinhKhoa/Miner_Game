using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

public class ManagersController : Patterns.Singleton<ManagersController>
{
    [SerializeField] private GameObject managerPrefab;
    public List<ManagerDataSO> managerDataSOs => _managerDataSOList;
    private List<ManagerDataSO> _managerDataSOList => MainGameData.managerDataSOList;

    public List<Manager> ShaftManagers = new List<Manager>();
    public List<Manager> ElevatorManagers = new List<Manager>();
    public List<Manager> CounterManagers = new List<Manager>();

    [SerializeField] private GameObject managerPanel;
    [SerializeField] private GameObject managerDetailPanel;

    public BaseManagerLocation CurrentManagerLocation { get; set; }
    private Camera _mainCamera;

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (_mainCamera != null)
            {
                Vector2 rayOrigin = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.zero);

                // Visualize the ray in Scene view
                Debug.DrawLine(rayOrigin, rayOrigin + Vector2.up * 100, Color.red, 2f);

                if (hit.collider != null)
                {
                    var managerLocation = hit.transform.GetComponent<BaseManagerLocation>();
                    if (managerLocation != null)
                    {
                        CurrentManagerLocation = managerLocation;
                        var manager = managerLocation.Manager;
                        OpenManagerPanel(true);
                    }
                }
            }
        }
    }

    public void OpenManagerPanel(bool isOpen)
    {
        managerPanel.SetActive(isOpen);
        
        if (CurrentManagerLocation.Manager != null)
        {
            managerPanel.GetComponent<ManagerChooseUI>().SetupTab(CurrentManagerLocation.Manager.Data.boostType,CurrentManagerLocation.LocationType);
        }
        else
        {
            managerPanel.GetComponent<ManagerChooseUI>().SetupTab(BoostType.Costs,CurrentManagerLocation.LocationType);
        }
    }

    public void OpenManagerDetailPanel(bool isOpen, Manager data)
    {
        managerDetailPanel.GetComponent<ManagerPanelUI>().SetManager(data);
        managerDetailPanel.SetActive(isOpen);
    }
}
