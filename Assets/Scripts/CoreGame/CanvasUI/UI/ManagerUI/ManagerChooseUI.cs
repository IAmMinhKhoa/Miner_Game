using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ManagerChooseUI : MonoBehaviour
{
    public static Action<BoostType> onManagerTabChanged;
    [SerializeField] private ManagerTabUI _managerTabUI;
    [SerializeField] private ManagerSectionList _managerSectionList;
    [SerializeField] private Button _closeButton;

    void Start()
    {
        _managerTabUI.onManagerTabChanged += OnManagerTabChanged;
        SetupTab(BoostType.Costs);
    }

    void OnEnable()
    {
        _closeButton.onClick.AddListener(ClosePanel);
    }

    void OnDisable()
    {
        _closeButton.onClick.RemoveListener(ClosePanel);
    }

    private void OnManagerTabChanged(BoostType type)
    {
        Debug.Log("Type: " + type);
        Debug.Log("Count: " + ManagersController.Instance.managerDataSOs.Where(x => x.boostType == type).ToList());
        _managerSectionList.ShowManagers(ManagersController.Instance.managerDataSOs.Where(x => x.boostType == type).ToList());
    }

    public void SetupTab(BoostType type)
    {
        _managerTabUI.onManagerTabChanged?.Invoke(type);
    }

    private void ClosePanel()
    {
        gameObject.SetActive(false);
    }
}
