using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using NOOD;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ManagerSectionUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _sectionText;
    [SerializeField] private ManagerGridUI _managerGridUI;
    private RectTransform _rectTransform;

    void Awake()
    {
        _rectTransform = this.GetComponent<RectTransform>();
    }

    public async UniTask SetData(string sectionName, List<Manager> managerDatas)
    {
        _sectionText.text = sectionName;
        await _managerGridUI.ShowMangers(managerDatas);
        await UniTask.WaitForEndOfFrame(this);
        LayoutRebuilder.ForceRebuildLayoutImmediate(_rectTransform);
    }
}
