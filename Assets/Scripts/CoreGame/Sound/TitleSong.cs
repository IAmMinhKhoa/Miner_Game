using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TitleSong : MonoBehaviour
{
    [SerializeField] Image _avatar;
    [SerializeField] TMP_Text _nameSong;
    [SerializeField] TMP_Text _author;
    [SerializeField] List<Sprite> _imgStateButton=new List<Sprite>(); //0 -> play, 1 -> pause
    [SerializeField] Image _imgBtnSelect;
    private bool _isPlay = false;
    public bool IsPlay
    {
        get => _isPlay;
        set
        {
             _isPlay=value;
            if (!value) 
            {
                _imgBtnSelect.sprite = _imgStateButton[0];
            }
            else
            {
                _imgBtnSelect.sprite = _imgStateButton[1];
            }
        }

    }


    public Action onClick;

    public void SetData(Sprite icon, string namesong, string nameAuthor,bool isPlay=false)
    {
        _avatar.sprite = icon;
        _nameSong.text = namesong;
        _author.text = "Tac Gia : "+ nameAuthor;
        IsPlay=isPlay;

    }
    public void resetState()
    {
        IsPlay = false;
    }

    public void OnSelect()
    {
        onClick?.Invoke();
        IsPlay = !IsPlay;
    }
}
