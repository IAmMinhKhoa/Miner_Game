using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

public class ManagersController : Patterns.Singleton<ManagersController>
{
    public List<ManagerDataSO> managerDataSOs => _managerDataSOList;
    private List<ManagerDataSO> _managerDataSOList => MainGameData.managerDataSOList;

    private List<Manager> _ShaftManagers = new List<Manager>();
    private List<Manager> _ElevatorManagers = new List<Manager>();
    private List<Manager> _CouterManagers = new List<Manager>();

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
                    Debug.Log("Target:" + hit.collider.gameObject.name);
                    var managerLocation = hit.transform.GetComponent<BaseManagerLocation>();
                    if (managerLocation != null)
                    {
                        CurrentManagerLocation = managerLocation;
                        var manager = managerLocation.Manager;
                        OpenManagerPanel(true);
                        if (manager != null)
                        {
                            ManagerChooseUI.onManagerTabChanged?.Invoke(manager.Data.boostType);
                        }
                        else
                        {
                            ManagerChooseUI.onManagerTabChanged?.Invoke(BoostType.Costs);
                        }
                    }
                }
            }
        }
    }

    public void OpenManagerPanel(bool isOpen)
    {
        managerPanel.SetActive(isOpen);
    }

    public void OpenManagerDetailPanel(bool isOpen, ManagerDataSO data)
    {
        
        managerDetailPanel.SetActive(isOpen);
    }
}
