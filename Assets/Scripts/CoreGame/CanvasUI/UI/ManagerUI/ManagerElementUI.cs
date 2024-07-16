using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ManagerElementUI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private Image _icon;
    private Manager _data;

    public void SetData(Manager managerData)
    {
        _data = managerData;
        _levelText.text = _data.Data.managerLevel.ToString();
        _timerText.text = _data.Data.boostTime.ToString();
        _icon.sprite = _data.Data.icon;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        ManagersController.Instance.OpenManagerDetailPanel(true, _data);
    }
}
