using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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
                HighlightButton(btn.Key, true);
                ScaleButton(btn.Key, true); // Scale up the selected button
            }
            else
            {
                HighlightButton(btn.Key, false);
                ScaleButton(btn.Key, false); // Scale down the unselected buttons
            }
        }
    }

    private void HighlightButton(Button button, bool highlight)
    {
        var colors = button.colors;
        if (highlight)
        {
            colors.normalColor = Color.white; // Selected button stays normal
            colors.highlightedColor = Color.white; // Highlighted color stays normal
            colors.pressedColor = Color.white; // Pressed color stays normal
        }
        else
        {
            colors.normalColor = Color.gray; // Unselected buttons become gray
            colors.highlightedColor = Color.gray; // Also gray when highlighted
            colors.pressedColor = Color.gray; // Also gray when pressed
        }
        button.colors = colors;
    }

    private void ScaleButton(Button button, bool scaleUp)
    {
        if (scaleUp)
        {
            button.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.2f).SetEase(Ease.OutBack);
        }
        else
        {
            button.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack);
        }
    }
}
