using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ManagerChooseUI : MonoBehaviour
{
    [SerializeField] private ManagerTabUI _managerTabUI;
    [SerializeField] private ManagerSectionList _managerSectionList;

    void Start()
    {
        _managerTabUI.onManagerTabChanged += OnManagerTabChanged;
    }

    private void OnManagerTabChanged(BoostType type)
    {
        Debug.Log("Type: " + type);
        Debug.Log("Count: " + ManagersController.Instance.managerDataSOs.Where(x => x.boostType == type).ToList());
        _managerSectionList.ShowManagers(ManagersController.Instance.managerDataSOs.Where(x => x.boostType == type).ToList());
    }
}
