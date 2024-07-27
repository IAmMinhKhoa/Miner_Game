using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomScrollbar : MonoBehaviour
{
    //small value check against to see if floats match
    private const float LABDA = 0.0001f;

    [SerializeField] private Scrollbar _scrollbar;
    [SerializeField] private Slider _slider;
    private float _currentProgress = 0;

    private void Awake()
    {
        _scrollbar.onValueChanged.AddListener(OnScrollbarChanged);
        _slider.onValueChanged.AddListener(OnSliderChanged);

        UpdateProgress(1);
    }
    public void UpdateProgress(float value)
    {
        _currentProgress = Mathf.Clamp(value, 0f, 1f);
        _scrollbar.value = _currentProgress;
        _slider.value = _currentProgress;

    }

    private void OnSliderChanged(float value)
    {
        if (value == _currentProgress) return;

        //normalize value to range 0 - 1
        // value = (value - _slider.minValue) / (_slider.maxValue - _slider.minValue);
        UpdateProgress(value);
        // Debug.Log($"OnSliderChanged: {value.ToString()}");
        // if (Mathf.Abs(_scrollbar.value - value) > LABDA)
        // {
        //     _scrollbar.value = value;
        // }
    }

    private void OnScrollbarChanged(float value)
    {
        if (_scrollbar.size >= 0.9) return;
        if (value == _currentProgress) return;

        UpdateProgress(value);
        // value = (value * (_slider.maxValue - _slider.minValue) + _slider.minValue);
        //  Debug.Log($"OnScrollbarChanged: {value.ToString()}");
        // if (Mathf.Abs(_slider.value - value) > LABDA)
        // {
        //     _slider.value = value;
        // }
    }
}
