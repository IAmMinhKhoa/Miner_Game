using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Spine.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardInformation : MonoBehaviour
{
	[Header("UI Text")]
	[SerializeField] private TextMeshProUGUI _nameText;
	[SerializeField] private TextMeshProUGUI _textTimeSkill;
	[SerializeField] private TextMeshProUGUI _textTimeCD;
	[SerializeField] private TextMeshProUGUI _textValueBuff;
	[SerializeField] private TextMeshProUGUI _textValueBuffInfo;
	[SerializeField] private TextMeshProUGUI _textQuoest;


	[Header("UI Image")]
	[SerializeField] Image _backGroundPanel;
	[SerializeField] Image _bannerName;
	[Header("Skeletion Spine")]
	[SerializeField] SkeletonGraphic _spineBoost;

	[SerializeField] private SkeletonGraphic _spineManager;
	
	[SerializeField] List<Image> _starts = new List<Image>();
	[SerializeField] List<Sprite> _stateStart = new List<Sprite>(); //0 active, 1 unActive

	public void SetData(Manager _data)
	{
		string titleKey = string.Empty;
		string titleQuoest = string.Empty;
		string titleBoost = string.Empty;
		RenderStart((int)_data.Level);
		switch (_data.Specie)
		{
			case ManagerSpecie.Tiger:
				titleKey = LocalizationManager.GetLocalizedString(LanguageKeys.TitleManagerSectionTiger);
				titleQuoest = LocalizationManager.GetLocalizedString(LanguageKeys.QuoestCardInfoTiger);
				break;
			case ManagerSpecie.Dog:
				titleKey = LocalizationManager.GetLocalizedString(LanguageKeys.TitleManagerSectionDog);
				titleQuoest = LocalizationManager.GetLocalizedString(LanguageKeys.QuoestCardInfoDog);
				break;
			case ManagerSpecie.Bear:
				titleKey = LocalizationManager.GetLocalizedString(LanguageKeys.TitleManagerSectionBear);
				titleQuoest = LocalizationManager.GetLocalizedString(LanguageKeys.QuoestCardInfoBear);
				break;
			case ManagerSpecie.Goat:
				titleKey = "Goat";
				titleQuoest = "animal bubble tea bro";
				break;
			case ManagerSpecie.Owl:
				titleKey = "Owl";
				titleQuoest = "animal bubble tea bro";
				break;
			case ManagerSpecie.Hamster:
				titleKey = "Hamster";
				titleQuoest = "animal bubble tea bro";
				break;
			default:
				titleKey = "Animal";
				titleQuoest = "animal bubble tea bro";
				break;
		}
		_nameText.text = titleKey;
		_backGroundPanel.sprite = Resources.Load<Sprite>(MainGameData.PanelFrontCardManager[(int)_data.Level]);
		_bannerName.sprite = Resources.Load<Sprite>(MainGameData.BannerLevels[(int)_data.Level]);
		_spineManager.skeletonDataAsset = _data.SkeletonAsset;
		_spineManager.Initialize(true);
		//set data description
		_textTimeSkill.text = _data.BoostTime.ToString() + " phút";
		_textTimeCD.text = _data.CooldownTime.ToString() + " phút";
		//_textValueBuff.text = _data.BoostValue.ToString() + " %";

		switch (_data.BoostType)
		{
			case BoostType.Costs:
				titleBoost = LocalizationManager.GetLocalizedString(LanguageKeys.BoostCardInFoCost);
				_textValueBuffInfo.text = titleBoost;
				_textValueBuff.text = $"-{_data.BoostValue} %";
				break;
			case BoostType.Speed:
				titleBoost = LocalizationManager.GetLocalizedString(LanguageKeys.BoostCardInFoSpeed);
				_textValueBuffInfo.text = titleBoost;
				_textValueBuff.text = $"+{_data.BoostValue} %";
				break;
			case BoostType.Efficiency:
				titleBoost = LocalizationManager.GetLocalizedString(LanguageKeys.BoostCardInFoEfficiency);
				_textValueBuffInfo.text = titleBoost;
				_textValueBuff.text = $"+{_data.BoostValue} %";
				break;
		}

		_textQuoest.text = titleQuoest;


		if (_data.BoostType == BoostType.Costs)
		{
			_spineBoost.AnimationState.SetAnimation(0, "Giam gia tien", loop: true);
		}
		if (_data.BoostType == BoostType.Efficiency)
		{
			_spineBoost.AnimationState.SetAnimation(0, "Tang tai trong", loop: true);
		}
		if (_data.BoostType == BoostType.Speed)
		{
			_spineBoost.AnimationState.SetAnimation(0, "Toc do di chuyen", loop: true);
		}
	}
	
	private void RenderStart(int Currentlevel)
	{
		foreach (var item in _starts)
		{
			item.sprite = _stateStart[1];
		}
		for (int i = 0; i <= Currentlevel; i++)
		{
			_starts[i].sprite = _stateStart[0];
		}
	}
}
