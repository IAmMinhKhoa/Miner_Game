using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ManagerElementUI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image _imgNumberIcon;
    [SerializeField] private Image _imgFrame;
    [SerializeField] private Image _icon;
    [SerializeField] GameObject _maskSelect;
    [SerializeField] GameObject _maskRejectMerge;
    [SerializeField] GameObject _maskCanMerge;
    private Manager _data;
    public Manager Data => _data;
    public bool IsSelected { set {
            _maskSelect.SetActive(value);
        } }
    public bool CanMerge
    {
        set
        {
            if(value) _maskCanMerge.SetActive(true);
            else _maskRejectMerge.SetActive(true);
        }
    }

    public void SetData(Manager managerData)
    {
        _data = managerData;
        _imgNumberIcon.sprite = Resources.Load<Sprite>(MainGameData.IconLevelNumber[(int)_data.Level]);
        _imgFrame.sprite = Resources.Load<Sprite>(MainGameData.FrameLevelAvatar[(int)_data.Level]);
        _icon.sprite = (int)_data.Level == 4 ? _data.IconSpecial : _data.Icon;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ManagersController.Instance.OpenManagerDetailPanel(true, _data);
    }
    public void ClearStateCard()
    {
        _maskRejectMerge.SetActive(false);
        _maskCanMerge.SetActive(false);
    }
}
