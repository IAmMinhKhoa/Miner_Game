using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SettingChildButton : MonoBehaviour
{
	[Header("Button")]
	[SerializeField] private ButtonBehavior _btnMXH;
	[SerializeField] private ButtonBehavior _btnPhanHoi;
	[SerializeField] private ButtonBehavior _btnTerm;
	[SerializeField] private ButtonBehavior _btnPrivacy;
	[Header("Text")]
	[SerializeField] private TextMeshProUGUI _txtDeviceId;
	[SerializeField] private TextMeshProUGUI _txtVersion;

	private void Awake()
	{
		_btnMXH.onClickEvent.AddListener(OnClickButtonMXH);
		_txtDeviceId.text = $"ID: {SystemInfo.deviceUniqueIdentifier}";
		_txtVersion.text = $"Version: {Application.version}";
	}

	void OnDestroy()
	{
		_btnMXH.onClickEvent.RemoveListener(OnClickButtonMXH);
	}

	void OnClickButtonMXH()
	{
		string facebookUrl = "https://www.facebook.com/profile.php?id=61562808870679";
		Application.OpenURL(facebookUrl);
	}
}
