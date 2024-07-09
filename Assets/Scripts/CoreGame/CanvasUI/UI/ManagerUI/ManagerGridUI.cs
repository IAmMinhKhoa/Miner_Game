using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks.Triggers;
using UnityEngine;

public class ManagerGridUI : MonoBehaviour
{
    [SerializeField] private ManagerElementUI _managerElementPrefab;
    private List<ManagerElementUI> _managerElementUiList = new List<ManagerElementUI>();

    void Start()
    {
        _managerElementPrefab.gameObject.SetActive(false);
    }

    public void ShowMangers(List<ManagerDataSO> managerDataSOs)
    {
        // Make sure ui is the same count with data
        AddOrRemoveManagerElementUIs(managerDataSOs);
        for(int i = 0; i < managerDataSOs.Count; i++) 
        {
            _managerElementUiList[i].SetData(managerDataSOs[i]);
        }
    }

    private void AddOrRemoveManagerElementUIs(List<ManagerDataSO> managerDataSOs)
    {
        while(_managerElementUiList.Count != managerDataSOs.Count)
        {
            if(_managerElementUiList.Count < managerDataSOs.Count)
            {
                var managerElementUI = Instantiate(_managerElementPrefab, transform);
                _managerElementUiList.Add(managerElementUI);
                managerElementUI.gameObject.SetActive(true);
            }
            if(_managerElementUiList.Count > managerDataSOs.Count)
            {
                Destroy(_managerElementUiList[0].gameObject);
                _managerElementUiList.RemoveAt(0);
            }
        }
    }
}
