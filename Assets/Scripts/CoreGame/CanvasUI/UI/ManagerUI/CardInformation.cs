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
		RenderStart((int)_data.Level);
		_nameText.text = _data.Specie.ToString();
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
				_textValueBuffInfo.text = "chi phí: ";
				_textValueBuff.text = $"-{_data.BoostValue} %";
				break;
			case BoostType.Speed:
				_textValueBuffInfo.text = "tốc độ di chuyển: ";
				_textValueBuff.text = $"+{_data.BoostValue} %";
				break;
			case BoostType.Efficiency:
				_textValueBuffInfo.text = "tốc độ dỡ hàng: ";
				_textValueBuff.text = $"+{_data.BoostValue} %";
				break;
		}

		_textQuoest.text = _data.Quoest;


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
