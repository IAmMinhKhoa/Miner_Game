using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingChildButton : MonoBehaviour
{
	[Header("Button")]
	[SerializeField] private ButtonBehavior _btnMXH;
	[SerializeField] private ButtonBehavior _btnPhanHoi;
	[SerializeField] private ButtonBehavior _btnTerm;
	[SerializeField] private ButtonBehavior _btnPrivacy;

	private void Awake()
	{
		_btnMXH.onClickEvent.AddListener(OnClickButtonMXH);
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
