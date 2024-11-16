using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using NOOD;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;

public class ManagerSectionUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _sectionText;
    [SerializeField] private ManagerGridUI _managerGridUI;
    private RectTransform _rectTransform;
	private ManagerSpecie managerSpecieLocation;

	private void Awake()
	{
		_rectTransform = this.GetComponent<RectTransform>();
	}
	public async UniTask SetData(ManagerSpecie managerSpecie, List<Manager> managerDatas)
    {
		this.managerSpecieLocation = managerSpecie;
		string titleKey = string.Empty;

		switch (managerSpecieLocation)
		{
			case ManagerSpecie.Tiger:
				titleKey = LocalizationManager.GetLocalizedString(LanguageKeys.TitleManagerSectionTiger);

				break;
			case ManagerSpecie.Dog:
				titleKey = LocalizationManager.GetLocalizedString(LanguageKeys.TitleManagerSectionDog);
				break;
			case ManagerSpecie.Bear:
				titleKey = LocalizationManager.GetLocalizedString(LanguageKeys.TitleManagerSectionBear);
				break;
			case ManagerSpecie.Owl:
				titleKey = LocalizationManager.GetLocalizedString(LanguageKeys.TitleManagerSectionOwl);
				break;
			case ManagerSpecie.Goat:
				titleKey = LocalizationManager.GetLocalizedString(LanguageKeys.TitleManagerSectionGoat);
				break;
			case ManagerSpecie.Hamster:
				titleKey = LocalizationManager.GetLocalizedString(LanguageKeys.TitleManagerSectionHamster);
				break;
		}
		_sectionText.text = titleKey;
		await _managerGridUI.ShowMangers(managerDatas);
        await UniTask.WaitForEndOfFrame(this);
        LayoutRebuilder.ForceRebuildLayoutImmediate(_rectTransform);
    }
}
