using System;
using System.Collections;
using System.Collections.Generic;
using NOOD.SerializableDictionary;
using UnityEngine;
using UnityEngine.UI;

public class ManagerTabUI : MonoBehaviour
{
    public Action<BoostType> onManagerTabChanged;

    [SerializeField] private SerializableDictionary<Button, BoostType> _managerTabFilter = new SerializableDictionary<Button, BoostType>();

    void OnEnable()
    {
        onManagerTabChanged += ChangeTabUI;
    }

    void OnDisable()
    {
        onManagerTabChanged -= ChangeTabUI;
    }
    void Start()
    {
        foreach (var btn in _managerTabFilter.Dictionary)
        {
            btn.Key.onClick.RemoveAllListeners();
            btn.Key.onClick.AddListener(() => ChangeTab(btn.Value));
        }
    }

    private void ChangeTab(BoostType boostType)
    {
        onManagerTabChanged?.Invoke(boostType);
    }

    private void ChangeTabUI(BoostType boostType)
    {
        foreach (var btn in _managerTabFilter.Dictionary)
        {
            if (btn.Value == boostType)
            {
                btn.Key.interactable = false;
                HighlightButton(btn.Key, true);
            }
            else
            {
                btn.Key.interactable = true;
                HighlightButton(btn.Key, false);
            }
        }
    }

    private void HighlightButton(Button button, bool highlight)
    {
        var colors = button.colors;
        colors.normalColor = highlight ? Color.green : Color.white;
        button.colors = colors;
    }
}
