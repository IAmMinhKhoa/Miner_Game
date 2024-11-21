using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class LevelPanelUI : MonoBehaviour
{
	[SerializeField] private Image imageUpgrade;
	[SerializeField] private BaseUpgrade baseUpgrade;
	[SerializeField] Sprite upgradeableSrpite;
	[SerializeField] Sprite unUpgradeableSprite;
	private float y_pos;
	private Tween tween;

	void Start()
	{
		PawManager.Instance.OnPawChanged += OnPawChanged;
	}

	private void OnPawChanged(double paw)
	{
		bool isActive = paw >= baseUpgrade.CurrentCost;
		//Debug.Log("Current paw:" + paw + " Current cost:" + baseUpgrade.CurrentCost);
		imageUpgrade.sprite = isActive ? upgradeableSrpite : unUpgradeableSprite;

		
	}
}
