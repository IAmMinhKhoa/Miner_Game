using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using NOOD.SerializableDictionary;
using UnityEngine.UI;
using DG.Tweening;

public class ManagerLocationUI : MonoBehaviour
{
    public static Action<ManagerLocation> OnTabChanged;
    [SerializeField] Animator animatorTabScroll;
    [SerializeField] private SerializableDictionary<Button, ManagerLocation> _managerTabFilter = new SerializableDictionary<Button, ManagerLocation>();
    private ManagerLocation _currentManagerLocation;

    void OnEnable()
    {
        OnTabChanged += ChangeTabUI;
        _currentManagerLocation = ManagerLocation.Shaft;
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

        else
        {
            bool currentState= animatorTabScroll.GetBool("active");
            if(currentState!=false) animatorTabScroll.SetBool("active", false);
        }
        
        _currentManagerLocation = managerLocation;
        OnTabChanged?.Invoke(managerLocation);
    }

    private void ChangeTabUI(ManagerLocation locationType)
    {
        if (locationType == ManagerLocation.Shaft)
        {
            bool currentState = animatorTabScroll.GetBool("active");
            if (currentState != true) animatorTabScroll.SetBool("active", true);
        }

        foreach (var btn in _managerTabFilter.Dictionary)
        {
            if (btn.Value == locationType)
            {
                HighlightStateButton(btn.Key, true);  // Highlight and change the state of the selected button
              //  ScaleButton(btn.Key, true);           // Scale up the selected button
            }
            else
            {
                HighlightStateButton(btn.Key, false); // Reset the state of unselected buttons
                //ScaleButton(btn.Key, false);          // Scale down the unselected buttons
            }
        }
    }

    private void HighlightStateButton(Button button, bool highlight)
    {
        var spriteState = button.spriteState;
        if (highlight)
        {
            // Use the pressed sprite as the selected sprite
         

            button.image.sprite = button.spriteState.pressedSprite; // Manually set the pressed sprite as the current sprite
        }
        else
        {
            // Reset to the normal sprite state
        

            button.image.sprite = button.spriteState.highlightedSprite; // Reset to the highlighted or normal sprite
        }
    }
    private void ScaleButton(Button button, bool scaleUp)
    {
        if (scaleUp)
        {
            button.transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.2f).SetEase(Ease.OutBack);
        }
        else
        {
            button.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack);
        }
    }
}
