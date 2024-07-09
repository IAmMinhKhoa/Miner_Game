using System;
using System.Collections;
using System.Collections.Generic;
using NOOD.SerializableDictionary;
using UnityEngine;
using UnityEngine.UI;

public class ManagerTabUI : MonoBehaviour
{
    public Action <BoostType> onManagerTabChanged;

    [SerializeField] private SerializableDictionary<Button, BoostType> _managerTabFilter = new SerializableDictionary<Button, BoostType>();

    void Start()
    {
        foreach(var btn in _managerTabFilter.Dictionary)
        {
            btn.Key.onClick.RemoveAllListeners();
            btn.Key.onClick.AddListener(() => ChangeTab(btn.Value));
        }
    }

    private void ChangeTab(BoostType boostType)
    {
        onManagerTabChanged?.Invoke(boostType);
    }
}
