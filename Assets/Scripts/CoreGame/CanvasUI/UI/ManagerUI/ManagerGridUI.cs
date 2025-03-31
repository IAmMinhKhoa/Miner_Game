using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ManagerGridUI : MonoBehaviour
{
    [SerializeField] private ManagerElementUI _managerElementPrefab;
    private List<ManagerElementUI> _managerElementUiList = new List<ManagerElementUI>();
    private RectTransform _rectTransform;

	public List<ManagerElementUI> ManagerElementUiList => _managerElementUiList;


	void Awake()
    {
        _rectTransform = this.GetComponent<RectTransform>();
    }

    void Start()
    {
        _managerElementPrefab.gameObject.SetActive(false);
    }

    public async UniTask ShowMangers(List<Manager> managerDatas)
    {
        // Make sure ui is the same count with data
        AddOrRemoveManagerElementUIs(managerDatas);
        // Set data
        for(int i = 0; i < managerDatas.Count; i++) 
        {
            _managerElementUiList[i].SetData(managerDatas[i]);
        }

        await UniTask.WaitUntil(() => _managerElementUiList.Count == managerDatas.Count);
        await UniTask.WaitForEndOfFrame(this);
        LayoutRebuilder.ForceRebuildLayoutImmediate(_rectTransform);
    }

    private async void AddOrRemoveManagerElementUIs(List<Manager> managerDatas)
    {
        while(_managerElementUiList.Count != managerDatas.Count)
        {
            if(_managerElementUiList.Count < managerDatas.Count)
            {
                var managerElementUI = Instantiate(_managerElementPrefab, transform);
                _managerElementUiList.Add(managerElementUI);
                managerElementUI.gameObject.SetActive(true);
            }
            if(_managerElementUiList.Count > managerDatas.Count)
            {
                Destroy(_managerElementUiList[0].gameObject);
                _managerElementUiList.RemoveAt(0);
            }
        }
        await UniTask.WaitUntil(() => _managerElementUiList.Count == managerDatas.Count);
    }
}
