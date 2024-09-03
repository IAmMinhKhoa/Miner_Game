using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class LevelPanelUI : MonoBehaviour
{
	[SerializeField] private Image imageUpgrade;
	[SerializeField] private BaseUpgrade baseUpgrade;
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
		imageUpgrade.gameObject.SetActive(isActive);

		if (isActive)
		{
			int amount = UpgradeManager.Instance.CalculateUpgradeAmount(paw, baseUpgrade);

			if (amount < 51 && amount > 0)
			{
				imageUpgrade.sprite = Resources.Load<Sprite>(MainGameData.CanUpgradeButton[0]);
			}
			else if (amount < 101)
			{
				imageUpgrade.sprite = Resources.Load<Sprite>(MainGameData.CanUpgradeButton[1]);
			}
			else
			{
				imageUpgrade.sprite = Resources.Load<Sprite>(MainGameData.CanUpgradeButton[2]);
			}
			var rect = imageUpgrade.gameObject.GetComponent<RectTransform>();
			if (tween == null || !tween.IsActive())
			{
				y_pos = imageUpgrade.gameObject.GetComponent<RectTransform>().position.y;
				tween = rect.DOMoveY(y_pos + 0.05f, 0.5f).SetLoops(-1, LoopType.Yoyo);
			}
		}
		else
		{
			if (tween != null)
			{
				tween.Kill();
			}
		}
	}
}
