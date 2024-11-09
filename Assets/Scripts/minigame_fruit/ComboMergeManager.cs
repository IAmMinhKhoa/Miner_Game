using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ComboMergeManager : Patterns.Singleton<ComboMergeManager>
{
	// Combo
	public int comboCount = 0;
	public float comboTime = 0f;
	public float maxComboTime = 5f;
	private void Update()
	{
		if (comboTime > 0)
		{
			comboTime -= Time.deltaTime;
			if (comboTime <= 0)
			{
				comboTime = 0;
				comboCount = 0;
			}
		}
	}
	
}
