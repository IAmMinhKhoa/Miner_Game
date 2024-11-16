using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWorldUI : Patterns.Singleton<GameWorldUI>
{
	[SerializeField] GameObject banner;

	private void Start()
	{
		//banner.transform.SetParent(GameWorldUI.Instance.transform, true);
	}
}
