using DG.Tweening;
using NOOD.Sound;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarketUIHandler : MonoBehaviour
{
	[SerializeField] private Toggle tgNoiThat, tgNhanVien, tgSideTabHotNV, tgSideTabHotNT;
	[SerializeField] private GameObject pnNoiThat, pnNhanVien, pnHotContentNT, pnContentNT, pnHotContentNV, pnContentNV;
	[SerializeField] private Button btExit;

	[SerializeField] private Transform sideTab1, sideTab2;
	[SerializeField] float _durationSldeTab = 0.06f;
		private void OnEnable()
	{
		StartCoroutine(OnEnableSideTab(sideTab1));
		StartCoroutine(OnEnableSideTab(sideTab2)); 
	}

	IEnumerator OnEnableSideTab(Transform sideTab)
	{
		yield return new WaitForSeconds(0.2f);
		SoundManager.PlaySound(SoundEnum.insertPaper);
		float tempX = sideTab.transform.GetChild(0).position.x;
		foreach (Transform item in sideTab)
		{
			item.position = new Vector3(-1.08501472473f, item.position.y, item.position.z);
		}
		foreach (Transform item in sideTab)
		{
			item.DOMoveX(-2.190700054168701f, 0.3f).SetEase(Ease.OutQuad);
			yield return new WaitForSeconds(_durationSldeTab);
		}
	}

	private void Start()
	{
		tgNhanVien.onValueChanged.AddListener(delegate
		{
			OnChoosingPanel(pnNhanVien, tgNhanVien);
			tgSideTabHotNV.isOn = true;
			StartCoroutine(OnEnableSideTab(sideTab1));
		});
		tgNoiThat.onValueChanged.AddListener(delegate
		{
			OnChoosingPanel(pnNoiThat, tgNoiThat);
			tgSideTabHotNT.isOn = true;
			StartCoroutine(OnEnableSideTab(sideTab2));
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
		gameObject.transform.DOLocalMoveX(0, 0.4f).SetEase(Ease.OutQuart);
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
