using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ManagerSectionList : MonoBehaviour
{
    [SerializeField] private float _animationSpeed = 1.2f;
    [SerializeField] private ManagerSectionUI _managerSectionUIPrefab;
    private List<ManagerSectionUI> _managerSectionUIList = new List<ManagerSectionUI>();
    private RectTransform _rectTransform;
    private CanvasGroup _canvasGroup;
    private bool _isPlayingAnimation;
    private List<ManagerSpecie> managerSpecies = new List<ManagerSpecie>();
    private List<Manager> managerDatas;

    void Awake()
    {
        _rectTransform = this.GetComponent<RectTransform>();
        _canvasGroup = this.GetComponent<CanvasGroup>();
    }

    void Start()
    {
        _managerSectionUIPrefab.gameObject.SetActive(false);
    }

    async void OnEnable()
    {
        await SwitchAnimation();
    }

    public async void ShowManagers(List<Manager> managerDatas, bool forceAnimation=true)
    {
        if (this.managerDatas != null && IsEqual(managerDatas, this.managerDatas)) return;
        List<ManagerSpecie> managerSpecie = managerDatas.Select(x => x.Specie).Distinct().OrderBy(specie => specie).ToList();
        this.managerDatas = managerDatas;
        this.managerSpecies = managerSpecie;
        if(_isPlayingAnimation == false)
        {
            await SwitchAnimation(forceAnimation);
        }
    }

    private bool IsEqual(List<Manager> managerDatas, List<Manager> managerDatas2)
    {
        if (managerDatas.Count != managerDatas2.Count) return false;

        for(int i = 0; i < managerDatas.Count; i++)
        {
            if (managerDatas[i] != managerDatas2[i]) return false;
        }
        return true;
    }

    private async UniTask SwitchAnimation(bool forceAnimation=true)
    {
        _isPlayingAnimation = true;



		if (forceAnimation)
		{
			while (_canvasGroup.alpha > 0)
			{
				_canvasGroup.alpha -= Time.deltaTime * _animationSpeed;
				await UniTask.Yield();
			}
		}
        AddOrRemoveManagerSectionUIs(managerSpecies);
        await SetDatas(managerSpecies, managerDatas);
        LayoutRebuilder.ForceRebuildLayoutImmediate(_rectTransform);
        List<Manager> tempManagerList = managerDatas;
        while(_canvasGroup.alpha < 1)
        {
            // Handle switch data when animating
            if(tempManagerList != this.managerDatas)
            {
                AddOrRemoveManagerSectionUIs(managerSpecies);
                await SetDatas(managerSpecies, managerDatas);
                LayoutRebuilder.ForceRebuildLayoutImmediate(_rectTransform);
                tempManagerList = managerDatas;
            }
            _canvasGroup.alpha += Time.deltaTime * _animationSpeed;
            await UniTask.Yield();           
        }
        _isPlayingAnimation = false;
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
