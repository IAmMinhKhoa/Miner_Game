using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarketContentBox : MonoBehaviour
{
	
	[SerializeField]
	GameObject lowContent;
	[SerializeField]
	GameObject normalContent;
	[SerializeField]
	GameObject superContent;

	public event Action<MarketContentBox> BoxIsEnable;
	public ContentFitterRefresh refeshUI;
	public GameObject LowContent => lowContent;
	public GameObject NormalContent => normalContent;
	public GameObject SuperContent => superContent;

	private void OnEnable()
	{
		BoxIsEnable?.Invoke(this);
		refeshUI?.RefreshContentFitters();
	}
	
}
