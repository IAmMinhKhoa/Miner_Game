using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ManagerSectionList : MonoBehaviour
{
    [SerializeField] private ManagerSectionUI _managerSectionUIPrefab;
    private List<ManagerSectionUI> _managerSectionUIList = new List<ManagerSectionUI>();
    private RectTransform _rectTransform;

    void Awake()
    {
        _rectTransform = this.GetComponent<RectTransform>();
    }

    void Start()
    {
        _managerSectionUIPrefab.gameObject.SetActive(false);
    }

    public async void ShowManagers(List<Manager> managerDatas)
    {
        List<ManagerSpecie> managerSpecie = managerDatas.Select(x => x.Specie).Distinct().OrderBy(specie => specie).ToList();
        AddOrRemoveManagerSectionUIs(managerSpecie);
        await SetDatas(managerSpecie, managerDatas);
        await UniTask.WaitForEndOfFrame(this);
        LayoutRebuilder.ForceRebuildLayoutImmediate(_rectTransform);
    }

    private async UniTask SetDatas(List<ManagerSpecie> managerSpecie, List<Manager> managerDatas)
    {
        for(int i = 0; i < managerSpecie.Count; i++)
        {
            await _managerSectionUIList[i].SetData(managerSpecie[i].ToString(), managerDatas.Where(x => x.Specie == managerSpecie[i] && !x.IsAssigned).ToList());
        }
    }

    private void AddOrRemoveManagerSectionUIs(List<ManagerSpecie> managerSpecie)
    {
        while(_managerSectionUIList.Count != managerSpecie.Count)
        {
            if(_managerSectionUIList.Count < managerSpecie.Count)
            {
                var managerSectionUI = Instantiate(_managerSectionUIPrefab, transform);
                _managerSectionUIList.Add(managerSectionUI);
                managerSectionUI.gameObject.SetActive(true);
            }
            if(_managerSectionUIList.Count > managerSpecie.Count)
            {
                if(_managerSectionUIList.Count > 0)
                {
                    Destroy(_managerSectionUIList[0].gameObject);
                    _managerSectionUIList.RemoveAt(0);
                }
            }
        }
    }
}
