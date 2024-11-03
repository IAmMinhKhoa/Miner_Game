using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;


public class BannerUI : MonoBehaviour
{
	[Header("UI Element")]
	[SerializeField] TMP_Text textTitle;
	[SerializeField] SpriteRenderer backBackground;
	[SerializeField] Image frontBackground;
	[Header("UI Input")]
	[SerializeField] TMP_InputField inputTitle;
	[SerializeField] TMP_Dropdown dropDownFont;
	[SerializeField] TMP_Dropdown dropDownBrBack;
	[SerializeField] TMP_Dropdown dropDownBrFront;
	[Header("Prefab")]
	[SerializeField] ColorButton colorBtnPrefab;
	[SerializeField] Transform parentListColor;
	[Header("Data")]
	[SerializeField] List<Color> dataColorText = new List<Color>();
	[SerializeField] List<TMP_FontAsset> dataFontText = new List<TMP_FontAsset>();
	[SerializeField] List<Sprite> dataSprite_1 = new List<Sprite>();
	[SerializeField] List<Sprite> dataSprite_2 = new List<Sprite>();
	#region Logic

	private void Start()
	{
		RenderColor();
		PopulateDropdownFont();
		PpopulateDropdownBr_1();

	}
	private void OnEnable()
	{
		
		inputTitle.onValueChanged.AddListener(setText);
		dropDownFont.onValueChanged.AddListener(SetTextFont);
		dropDownBrBack.onValueChanged.AddListener(ChangeBr_1);
	}
	private void OnDisable()
	{
		inputTitle.onValueChanged.RemoveAllListeners();
		dropDownFont.onValueChanged.RemoveAllListeners();
		dropDownBrBack.onValueChanged.RemoveAllListeners();
	}






	private void setText(string content)
	{
		textTitle.text = content;
	}
	private void SetTextColor(Color newColor)
	{
		textTitle.color = newColor;
	}
	private void SetTextFont(int index)
	{
		textTitle.font = dataFontText[index];
	}
	private void ChangeBr_1(int index)//Br back
	{
		backBackground.sprite = dataSprite_1[index];
	}
	private void Save(string title, string color, int indexFont, int indexBr)
	{
		//save to playfab
	}
	#endregion
	#region Support
	private void PopulateDropdownFont()
	{
		// Clear any existing options
		dropDownFont.ClearOptions();

		// Create a list of option strings
		List<string> fontNames = new List<string>();
		for (int i = 0; i < dataFontText.Count; i++)
		{
			string fontName = "fontcute " + i; // Create a custom font name using the index
			fontNames.Add(fontName); // Add the custom font name to the list
		}

		dropDownFont.AddOptions(fontNames);
	}
	private void PpopulateDropdownBr_1()
	{
		dropDownBrBack.ClearOptions();
		List<string> namesImage = new List<string>();
		foreach (var item in dataSprite_1)
		{
			namesImage.Add(item.name);
		}
		dropDownBrBack.AddOptions(namesImage);
	}
	private void RenderColor()
	{
		foreach (var item in dataColorText)
		{
			ColorButton _cloneColor = Instantiate(colorBtnPrefab, parentListColor);
			_cloneColor.SetData(item, () =>
			{
				SetTextColor(item);
			});
		}
	}

	public void CloseModal()
	{
		this.gameObject.SetActive(false);
	}
	#endregion

}
