using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarketUIHandler : MonoBehaviour
{
	[SerializeField] private Toggle tgNoiThat, tgNhanVien, tgSideTabHotNV, tgSideTabHotNT;
	[SerializeField] private GameObject pnNoiThat, pnNhanVien, pnHotContentNT, pnContentNT, pnHotContentNV, pnContentNV;
	[SerializeField] private Button btExit;

	private void Start()
	{
		tgNhanVien.onValueChanged.AddListener(delegate
		{
			OnChoosingPanel(pnNhanVien, tgNhanVien);
			tgSideTabHotNV.isOn = true;
		});
		tgNoiThat.onValueChanged.AddListener(delegate
		{
			OnChoosingPanel(pnNoiThat, tgNoiThat);
			tgSideTabHotNT.isOn = true;
		});
		tgSideTabHotNT.onValueChanged.AddListener(delegate
		{
			OnChoosingSideTab(pnHotContentNT, pnContentNT, tgSideTabHotNT);
		});
		tgSideTabHotNV.onValueChanged.AddListener(delegate
		{
			OnChoosingSideTab(pnHotContentNV, pnContentNV, tgSideTabHotNV);
		});
		tgNoiThat.isOn = true;
		tgSideTabHotNT.isOn = true;

		btExit.onClick.AddListener(() => {
			FadeOutContainer();
		});
		
	}

	public void OnChoosingPanel(GameObject panel, Toggle tg)
	{
		if (tg.isOn) tg.gameObject.GetComponent<ToggleBehaviour>().DoAnimate();
		panel.SetActive(tg.isOn);
	}

	public void SlideInContainer(GameObject panel, Toggle tg)
	{
		if (tg.isOn) tg.gameObject.GetComponent<ToggleBehaviour>().DoAnimate();
		panel.SetActive(tg.isOn);
		Vector2 posCam = CustomCamera.Instance.GetCurrentTransform().position;
		panel.transform.localPosition = new Vector2(posCam.x - 2000, panel.transform.localPosition.y);
		panel.transform.DOLocalMoveX(0, 0.6f).SetEase(Ease.OutElastic, 1, 1f);

	}

	public void OnChoosingSideTab(GameObject panelOn, GameObject panelOff, Toggle tg)
	{
		panelOn.SetActive(tg.isOn);
		panelOff.SetActive(!tg.isOn);
	}


	public void FadeInContainer()
	{
		gameObject.SetActive(true);
		Vector2 posCam = CustomCamera.Instance.GetCurrentTransform().position;
		gameObject.transform.localPosition = new Vector2(posCam.x - 2000, posCam.y); //Left Screen
		gameObject.transform.DOLocalMoveX(0, 0.6f).SetEase(Ease.OutQuart);
	}
	public void FadeOutContainer()
	{
		Vector2 posCam = CustomCamera.Instance.GetCurrentTransform().position;
		gameObject.transform.DOLocalMoveX(posCam.x - 2000f, 0.6f).SetEase(Ease.InQuart).OnComplete(() =>
		{
			gameObject.SetActive(false);
		});

	}
}
