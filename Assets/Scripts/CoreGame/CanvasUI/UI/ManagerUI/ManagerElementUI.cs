using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ManagerElementUI : Selectable, ISelectHandler
{
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private Image _icon;
    private ManagerDataSO _data;

    public void SetData(ManagerDataSO managerDataSO)
    {
        _data = managerDataSO;
        _levelText.text = managerDataSO.managerLevel.ToString();
        _timerText.text = managerDataSO.boostTime.ToString();
        _icon.sprite = managerDataSO.icon;
    }

    public void OnSelect(BaseEventData _)
    {
        Debug.Log("Selected " + _data.managerName);
        ManagersController.Instance.OpenManagerDetailPanel(true, _data);
    }
}
