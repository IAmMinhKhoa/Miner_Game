using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BannerController : MonoBehaviour
{
	/*	public void SetDataTitle(string _content = "Animal Bubble Tea", Color? _color = null, TMP_FontAsset _font = null)
	{
		// Set the text content
		textTitle.text = _content;

		// Set the text color, use Color.black if _color is null
		textTitle.color = _color ?? Color.black;

		// Set the font if one is provided
		if (_font != null)
		{
			textTitle.font = _font;
		}

		//save
	}*/
	[SerializeField] GameObject modalUI;

	#region Support
	public void OpenModal()
	{
		modalUI.SetActive(true);
	}
	#endregion

}




public class BannerEntity
{
	public string title;
	public string color;
	public int indexFont;
	public int indexBackGround;
}

/*
 * fearture
 * I. Text
 *		1. Change text title
 *		2. Change color
 *		3. Change font
 *		4.(Can change stroke)
 * II. Background
 *		1. Change skin
 * III. Creat all to template and select
 */
