using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BannerUI : MonoBehaviour
{
	[Header("UI Element")]
	
	[SerializeField] TMP_Text _demoTextTitle;
	[SerializeField] Image _demobackBackground;
	[SerializeField] Image _demofrontBackground;

	[Header("UI Input")]
	[SerializeField] TMP_InputField inputTitle;
	[SerializeField] TMP_Dropdown dropDownFont;
	[SerializeField] TMP_Dropdown dropDownBrBack;
	[SerializeField] TMP_Dropdown dropDownBrFront;

	[Header("Prefab")]
	[SerializeField] ColorButton colorBtnPrefab;
	[SerializeField] Transform parentListColor;

	BannerSO bannerData;
	List<ColorButton> listColorButton = new();

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
		}

	}
	private void Init()
	{
		string titleKeyBodyFontCute = LocalizationManager.GetLocalizedString(LanguageKeys.TitleBodyFontCute);
		string titleKeyTemplateNumber = LocalizationManager.GetLocalizedString(LanguageKeys.TitleBodyTemplateNumber);
		bannerData = BannerController.Instance.bannerData;
		PopulateDropdown(dropDownFont, bannerData.dataFontText.Count, titleKeyBodyFontCute + " ");
		PopulateDropdown(dropDownBrBack, bannerData.dataSprite_1.Count, titleKeyTemplateNumber + " ");
		PopulateDropdown(dropDownBrFront, bannerData.dataSprite_2.Count, titleKeyTemplateNumber + " ");
	}	

	private void OnEnable()
	{
		Init();
		inputTitle.onValueChanged.AddListener(SetDemoText);
		dropDownFont.onValueChanged.AddListener(SetTextFont);
		//dropDownBrBack.onValueChanged.AddListener(index => SetBackgroundImage(_demobackBackground, bannerData.dataSprite_1, index));
		//dropDownBrFront.onValueChanged.AddListener(index => SetBackgroundImage(_demofrontBackground, bannerData.dataSprite_2, index));
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

	private void SetBackgroundImage(Image image, List<Sprite> sprites, int index)
	{
		image.sprite = sprites[index];
	}

	public void SaveCurrentData()
	{
		// Collect the current data from the UI elements
		var currentBanner = new BannerEntity(_demoTextTitle.text, _demoTextTitle.color, dropDownFont.value, dropDownBrBack.value, dropDownBrFront.value);
		string json = JsonUtility.ToJson(currentBanner);
		PlayerPrefs.SetString("BannerData", json);
		PlayerPrefs.Save();

		// Apply to world UI
		BannerController.Instance.ApplyToWorldUI(currentBanner);
		CloseModal();
	}


	private void ApplyToWorldUIDemo(BannerEntity banner)
	{
		_demoTextTitle.text = banner.title;
		_demoTextTitle.color = banner.color;
		_demoTextTitle.font = bannerData.dataFontText[banner.indexFont];
		//_demobackBackground.sprite = bannerData.dataSprite_1[banner.indexBrBack];
		//_demofrontBackground.sprite = bannerData.dataSprite_2[banner.indexBrFront];
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
	public int indexBrFront;

	public BannerEntity(string title, Color color, int indexFont, int indexBrBack, int indexBrFront)
	{
		this.title = title;
		this.color = color;
		this.indexFont = indexFont;
		this.indexBrBack = indexBrBack;
		this.indexBrFront = indexBrFront;
	}
}
