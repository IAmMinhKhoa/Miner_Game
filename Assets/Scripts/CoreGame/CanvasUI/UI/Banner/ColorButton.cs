using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ColorButton : MonoBehaviour
{
	[SerializeField] Image imgColor;
	[SerializeField] Button btn;
	public Action Select;
	private void Start()
	{
		btn.onClick.AddListener(OnSelect);
	}
	public void SetData(Color _color, Action _select)
	{
		Select = _select;
		imgColor.color = _color;

	}
	private void OnDestroy()
	{
		btn.onClick.RemoveAllListeners();
	}


	private void OnSelect()
	{
		Select?.Invoke();
	}

}
