using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ManagerElementUI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private Image _icon;
    private Manager _data;
    public Manager Data => _data;

    public void SetData(Manager managerData)
    {
        _data = managerData;
        _levelText.text = _data.Level.ToString();
        _timerText.text = _data.BoostTime.ToString();
        _icon.sprite = _data.Icon;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ManagersController.Instance.OpenManagerDetailPanel(true, _data);
    }
}
