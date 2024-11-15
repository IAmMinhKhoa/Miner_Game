using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class BannerUI : MonoBehaviour
{
	[Header("UI Element")]
	
	[SerializeField] TMP_Text _demoTextTitle;
	[SerializeField] Image _demobackBackground;
	[SerializeField] Image _demofrontBackground;
	[SerializeField] Image _demoFrontColor;
	[SerializeField] Image _demoBackColor;

	[Header("UI Input")]
	[SerializeField] TMP_InputField inputTitle;
	[SerializeField] TMP_Dropdown dropDownFont;
	[SerializeField] TMP_Dropdown dropDownBrBack;
	[SerializeField] TMP_Dropdown dropDownBrFront;
	[SerializeField] TMP_Dropdown dropDownDetailBrFront;
	[SerializeField] TMP_Dropdown dropDownDetailBrBack;


	[Header("Prefab")]
	[SerializeField] ColorButton colorBtnPrefab;
	[SerializeField] Transform parentListColor;

	BannerSO bannerData;
	List<ColorButton> listColorButton = new();
	
	int curIndexFront = 0;
	int curIndexBack = 0;
	#region Unity Callbacks

	private void Start()
	{
		Init();
		RenderColor();
		if (PlayerPrefs.HasKey("BannerData"))
		{
			string json = PlayerPrefs.GetString("BannerData");
			BannerEntity banner = JsonUtility.FromJson<BannerEntity>(json);
			ApplyToWorldUIDemo(banner);
			Debug.Log("Data loaded from PlayerPrefs.");
			//return;
		}
		UpdateDemoImg();
		//banner = new();
	}
	private void UpdateDemoImg()
	{
		var bn = BannerController.Instance.curBannerEntity;
		Debug.Log(bn.IDFront + "-" + bn.indexBrFront + "-" + bn.IDBack + "-" + bn.indexBrBack);
		_demobackBackground.sprite = bannerData.dataSprite_2[bn.IDBack].templateDetails[bn.indexBrBack].sprite;
		_demofrontBackground.sprite = bannerData.dataSprite_1[bn.IDFront].templateDetails[bn.indexBrFront].sprite;
		dropDownBrBack.value = bn.IDBack;
		dropDownBrFront.value = bn.IDFront;
		dropDownDetailBrBack.value = bn.indexBrBack;
		dropDownDetailBrFront.value = bn.indexBrFront;
		curIndexBack = bn.indexBrBack;
		curIndexFront = bn.indexBrFront;
	}
	private void Init()
	{
		string titleKeyBodyFontCute = LocalizationManager.GetLocalizedString(LanguageKeys.TitleBodyFontCute);
		string titleKeyTemplateNumber = LocalizationManager.GetLocalizedString(LanguageKeys.TitleBodyTemplateNumber);
		bannerData = BannerController.Instance.bannerData;
		PopulateDropdown(dropDownFont, bannerData.dataFontText.Count, titleKeyBodyFontCute + " ");
		PopulateDropdown(dropDownBrBack, bannerData.dataSprite_2.Count, titleKeyTemplateNumber + " ");
		PopulateDropdown(dropDownBrFront, bannerData.dataSprite_1.Count, titleKeyTemplateNumber + " ");
		InitDetailDropDown(bannerData.dataSprite_2, 0, dropDownDetailBrBack, false);
		InitDetailDropDown(bannerData.dataSprite_1, 0, dropDownDetailBrFront, true);
	}	

	private void OnEnable()
	{
		Init();
		inputTitle.onValueChanged.AddListener(SetDemoText);
		dropDownFont.onValueChanged.AddListener(SetTextFont);
		dropDownBrBack.onValueChanged.AddListener(ID => InitDetailDropDown(bannerData.dataSprite_2, ID, dropDownDetailBrBack, false));
		dropDownBrFront.onValueChanged.AddListener(ID => InitDetailDropDown(bannerData.dataSprite_1, ID, dropDownDetailBrFront, true));
		dropDownDetailBrBack.onValueChanged.AddListener(index => SetBackgroundImage(_demobackBackground, bannerData.dataSprite_2, dropDownBrBack.value, index, false));
		dropDownDetailBrFront.onValueChanged.AddListener(index => SetBackgroundImage(_demofrontBackground, bannerData.dataSprite_1, dropDownBrFront.value, index, true));
		UpdateDemoImg();
	}

	private void OnDisable()
	{
		inputTitle.onValueChanged.RemoveAllListeners();
		dropDownFont.onValueChanged.RemoveAllListeners();
		dropDownBrBack.onValueChanged.RemoveAllListeners();
		dropDownBrFront.onValueChanged.RemoveAllListeners();
	}

	#endregion

	#region UI Methods

	private void SetDemoText(string content) => _demoTextTitle.text = content;

	private void SetTextColor(Color newColor) {
		_demoTextTitle.color = newColor;
		foreach(var item in listColorButton)
		{
			item.HideBoder();
		}
	}

	private void SetTextFont(int index)
	{
		_demoTextTitle.font = bannerData.dataFontText[index];
	}
	void InitDetailDropDown(List<DesignTemplateInfo> templateInfos, int ID, TMP_Dropdown dropdownDetail, bool isFront)
	{
		string titleKeyTemplateNumber = LocalizationManager.GetLocalizedString(LanguageKeys.TitleBodyTemplateNumber);
		PopulateDropdown(dropdownDetail, templateInfos[ID].templateDetails.Count, titleKeyTemplateNumber + " ");

		if (isFront)
		{
			_demoFrontColor.color = templateInfos[ID].templateDetails[0].color;
			_demofrontBackground.sprite = templateInfos[ID].templateDetails[0].sprite;
			curIndexFront = 0;
		}
		else
		{
			_demoBackColor.color = templateInfos[ID].templateDetails[0].color;
			_demobackBackground.sprite = templateInfos[ID].templateDetails[0].sprite;
			curIndexBack = 0;
		}
	}
	private void SetBackgroundImage(Image image, List<DesignTemplateInfo> templateInfos,int ID ,int index, bool isFront)
	{
		image.sprite = templateInfos[ID].templateDetails[index].sprite;
		if(isFront)
		{
			curIndexFront = index;
			_demoFrontColor.color = templateInfos[ID].templateDetails[index].color;
		}
		else
		{
			curIndexBack = index;
			_demoBackColor.color = templateInfos[ID].templateDetails[index].color;
		}
	}

	public void SaveCurrentData()
	{
		// Collect the current data from the UI elements
		//BannerEntity(string title, Color color, int IDBack, int indexFont, int IDFront, int indexBrBack, int indexBrFront)
		var currentBanner = new BannerEntity(_demoTextTitle.text, _demoTextTitle.color, dropDownFont.value, dropDownBrBack.value, dropDownBrFront.value, curIndexBack, curIndexFront);
		string json = JsonUtility.ToJson(currentBanner);
		PlayerPrefs.SetString("BannerData", json);
		PlayerPrefs.Save();
		BannerController.Instance.curBannerEntity = currentBanner;
		// Apply to world UI
		
		BannerController.Instance.ApplyToWorldUI(currentBanner);
		CloseModal();
	}


	private void ApplyToWorldUIDemo(BannerEntity banner)
	{
		_demoTextTitle.text = banner.title;
		_demoTextTitle.color = banner.color;
		_demoTextTitle.font = bannerData.dataFontText[banner.indexFont];
		_demobackBackground.sprite = bannerData.dataSprite_1[banner.IDBack].templateDetails[banner.indexBrFront].sprite;
		_demofrontBackground.sprite = bannerData.dataSprite_2[banner.IDBack].templateDetails[banner.indexBrBack].sprite;
	}
	#endregion



	#region Helper Methods

	private void PopulateDropdown(TMP_Dropdown dropdown, int itemCount, string prefix)
	{
		dropdown.ClearOptions();
		List<string> options = new List<string>();
		for (int i = 0; i < itemCount; i++)
		{
			options.Add(prefix + i);
		}
		dropdown.AddOptions(options);
	}

	private void RenderColor()
	{
		foreach (var color in bannerData.dataColorText)
		{
			ColorButton colorButton = Instantiate(colorBtnPrefab, parentListColor);
			colorButton.SetData(color, () => SetTextColor(color));
			listColorButton.Add(colorButton);
		}
	}

	private void CloseModal() => BannerController.Instance.CloseModal();

	#endregion
}

public class BannerEntity
{
	public string title;
	public Color color;
	public int indexFont;
	public int indexBrBack;
	public int IDBack;
	public int indexBrFront;
	public int IDFront;

	public BannerEntity(string title, Color color, int indexFont, int IDBack, int IDFront, int indexBrBack, int indexBrFront)
	{
		this.title = title;
		this.color = color;
		this.indexFont = indexFont;
		this.IDFront = IDFront;
		this.indexBrBack = indexBrBack;
		this.IDBack = IDBack;
		this.indexBrFront = indexBrFront;
	}
}
