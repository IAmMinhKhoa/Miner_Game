using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using NOOD.SerializableDictionary;
using UnityEngine.UI;

public class ManagerLocationUI : MonoBehaviour
{
    public static Action<ManagerLocation> OnTabChanged;
    [SerializeField] GameObject panelShaftManager;
    [SerializeField] private SerializableDictionary<Button, ManagerLocation> _managerTabFilter = new SerializableDictionary<Button, ManagerLocation>();
    private ManagerLocation _currentManagerLocation;

    void OnEnable()
    {
        OnTabChanged += ChangeTabUI;
    }

    void OnDisable()
    {
        OnTabChanged -= ChangeTabUI;
    }

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
        if (managerLocation == ManagerLocation.Shaft) panelShaftManager.SetActive(true);
        else panelShaftManager.SetActive(false);
        _currentManagerLocation = managerLocation;
        OnTabChanged?.Invoke(managerLocation);
    }

    private void ChangeTabUI(ManagerLocation locationType)
    {
        foreach (var btn in _managerTabFilter.Dictionary)
        {
            if (btn.Value == locationType)
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
