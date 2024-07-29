using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using NOOD;
using NOOD.Sound;
using UnityEngine;

public class PlayList : MonoBehaviour
{
    [SerializeField] Transform _parentList;
    [SerializeField] GameObject _prefab;
    private List<TitleSong> _titleSongs =new List<TitleSong> ();

    private void Start()
    {
       
    }


    public void InitData(Action<string> selectMusic)
    {
        Dictionary<string, DataMusic> _dataMusicDic = SoundManager.GetAllDataMusicDictionary();
        DataMusic currentData = SoundManager.GetDataMusic(SoundManager.GetCurrentMusic());
        foreach (var kvp in _dataMusicDic)
        {
            string key = kvp.Key;
            DataMusic item = kvp.Value;

            GameObject newTitleObj = Instantiate(_prefab, _parentList);
            TitleSong newTitle = newTitleObj.GetComponent<TitleSong>();

            bool isCurrentItem = (item.name == currentData.name);
            newTitle.SetData(item.icon, item.name, item.author, isCurrentItem);

            newTitle.onClick += (() =>
            {
                Debug.Log("click on:" + item.name);
                reloadStateButton();
                selectMusic?.Invoke(key); // Invoke with the key of the dictionary
            });
            _titleSongs.Add(newTitle);
        }
    }


    #region Support
    private void reloadStateButton()
    {
        foreach (var item in _titleSongs)
        {
            item.resetState();
        }
    }
    private void ClearAllChildren()
    {
        foreach (Transform child in _parentList)
        {
            Destroy(child.gameObject);
        }
    }
    public void CloseModal()
    {
        Vector2 posCam = CustomCamera.Instance.GetCurrentTransform().position;
        transform.DOLocalMoveY(posCam.y + 2000f, 0.5f).SetEase(Ease.InQuart).OnComplete(() =>
        {
            Destroy(this.gameObject);
        });


        
    }
    #endregion
}
