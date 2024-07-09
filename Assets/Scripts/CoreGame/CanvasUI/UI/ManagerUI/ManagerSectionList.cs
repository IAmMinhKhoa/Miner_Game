using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ManagerSectionList : MonoBehaviour
{
    [SerializeField] private ManagerSectionUI _managerSectionUIPrefab;
    private List<ManagerSectionUI> _managerSectionUIList = new List<ManagerSectionUI>();

    void Start()
    {
        _managerSectionUIPrefab.gameObject.SetActive(false);
    }

    public void ShowManagers(List<ManagerDataSO> managerDataSOs)
    {
        List<ManagerSpecie> managerSpecie = managerDataSOs.Select(x => x.managerSpecie).Distinct().ToList();
        AddOrRemoveManagerSectionUIs(managerSpecie);
        for(int i = 0; i < managerSpecie.Count; i++)
        {
            _managerSectionUIList[i].SetData(managerSpecie[i].ToString(), managerDataSOs.Where(x => x.managerSpecie == managerSpecie[i]).ToList());
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
