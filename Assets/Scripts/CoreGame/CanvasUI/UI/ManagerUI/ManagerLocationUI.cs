using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using NOOD.SerializableDictionary;
using UnityEngine.UI;

public class ManagerLocationUI : MonoBehaviour
{
    public static Action<ManagerLocation> OnTabChanged;

    [SerializeField] private SerializableDictionary<Button, ManagerLocation> _managerTabFilter = new SerializableDictionary<Button, ManagerLocation>();
    private ManagerLocation _currentManagerLocation;

    void Start()
    {
        foreach (var btn in _managerTabFilter.Dictionary)
        {
            btn.Key.onClick.RemoveAllListeners();
            btn.Key.onClick.AddListener(() => ChangeTab(btn.Value));
        }
    }

    private void ChangeTab(ManagerLocation managerLocation)
    {
        if (_currentManagerLocation == managerLocation)
        {
            return;
        }
        _currentManagerLocation = managerLocation;
        OnTabChanged?.Invoke(managerLocation);
    }
}
