using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NOOD.SerializableDictionary;
using UnityEngine;
using UnityEngine.UI;

public class ManagerTabUI : MonoBehaviour
{
    public Action<BoostType,bool> onManagerTabChanged;

    [SerializeField] private SerializableDictionary<Button, BoostType> _managerTabFilter = new SerializableDictionary<Button, BoostType>();

	public SerializableDictionary<Button, BoostType> ManagerTabFilter => _managerTabFilter;

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

    private void ChangeTab(BoostType boostType,bool forceAnimation=true)
    {
        onManagerTabChanged?.Invoke(boostType, forceAnimation);
		Debug.Log("tab change:"+boostType);
    }

    private void ChangeTabUI(BoostType boostType,bool forceAnimation)
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
        ButtonBehavior buttonBehavior;
        if (button.gameObject.TryGetComponent<ButtonBehavior>(out buttonBehavior))
        {
            if (highlight)
            {
                buttonBehavior.SetState(ButtonState.Click);
            }
            else
            {
                buttonBehavior.SetState(ButtonState.Default);
            }
        }

        

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
