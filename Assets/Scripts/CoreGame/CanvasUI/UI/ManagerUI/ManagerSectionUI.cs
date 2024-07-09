using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ManagerSectionUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _sectionText;
    [SerializeField] private ManagerGridUI _managerGridUI;

    public void SetData(string sectionName, List<ManagerDataSO> managerDataSOs)
    {
        _sectionText.text = sectionName;
        _managerGridUI.ShowMangers(managerDataSOs);
    }
}
