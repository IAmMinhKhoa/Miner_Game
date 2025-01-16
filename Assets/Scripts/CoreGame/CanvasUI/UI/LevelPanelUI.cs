using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Spine.Unity;
public class LevelPanelUI : MonoBehaviour
{
	[SerializeField] private Image imageUpgrade;
	[SerializeField] private BaseUpgrade baseUpgrade;
	//[SerializeField] Sprite upgradeableSrpite;
	//[SerializeField] Sprite unUpgradeableSprite;
	[SerializeField] SkeletonGraphic skeletonGraphic;
	private float y_pos;
	private Tween tween;

	void Start()
	{
		PawManager.Instance.OnPawChanged += OnPawChanged;
	}

	private void OnPawChanged(double paw)
	{
		bool isActive = paw >= baseUpgrade.CurrentCost;
		
		skeletonGraphic.gameObject.SetActive(isActive);
		

		
	}
}
