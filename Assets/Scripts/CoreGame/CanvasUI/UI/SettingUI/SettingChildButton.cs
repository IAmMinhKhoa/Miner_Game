using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class SettingChildButton : MonoBehaviour
{
	[Header("Button")]
	[SerializeField] private ButtonBehavior _btnMXH;
	[SerializeField] private ButtonBehavior _btnPhanHoi;
	[SerializeField] private ButtonBehavior _btnTerm;
	[SerializeField] private ButtonBehavior _btnPrivacy;
	[SerializeField] private TMP_Dropdown _Dropdown;
	[Header("Text")]
	[SerializeField] private TextMeshProUGUI _txtDeviceId;
	[SerializeField] private TextMeshProUGUI _txtVersion;

	private void Awake()
	{
		_btnMXH.onClickEvent.AddListener(OnClickButtonMXH);
		_Dropdown.onValueChanged.AddListener(OnLanguageSelected);
		_txtDeviceId.text = $"ID: {SystemInfo.deviceUniqueIdentifier}";
		_txtVersion.text = $"Version: {Application.version}";
	}

	void OnDestroy()
	{
		_btnMXH.onClickEvent.RemoveListener(OnClickButtonMXH);
		_Dropdown.onValueChanged.RemoveListener(OnLanguageSelected);
	}

	void OnClickButtonMXH()
	{
		string facebookUrl = "https://www.facebook.com/profile.php?id=61562808870679";
		Application.OpenURL(facebookUrl);
	}

	private void OnLanguageSelected(int index)
	{
		// Thay đổi ngôn ngữ khi chọn một mục mới
		Debug.Log("Change Language :" + index);
		var selectedLocale = index == 0
			? LocalizationSettings.AvailableLocales.GetLocale("vi")
			: LocalizationSettings.AvailableLocales.GetLocale("en");

		LocalizationSettings.SelectedLocale = selectedLocale;
	}
}
