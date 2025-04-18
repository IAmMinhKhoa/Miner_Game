using DG.Tweening;
using NOOD.Sound;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarketUIHandler : MonoBehaviour
{
	private Vector3 scale_tablet = new Vector3(1f, 1f, 1f);
	[SerializeField] private Toggle tgNoiThat, tgNhanVien, tgSideTabHotNV, tgSideTabHotNT;
	[SerializeField] private GameObject pnNoiThat, pnNhanVien, pnHotContentNT, pnContentNT, pnHotContentNV, pnContentNV;
	[SerializeField] private Button btExit;

	[SerializeField] private Transform sideTab1, sideTab2;
	[SerializeField] float _durationSldeTab = 0.03f;
	private Coroutine currentCoroutine1, currentCoroutine2;


	private void OnEnable()
	{
		CheckResetSideTab(1);
		CheckResetSideTab(2);
		currentCoroutine1 = StartCoroutine(OnEnableSideTab(sideTab1));
		currentCoroutine2 = StartCoroutine(OnEnableSideTab(sideTab2));
	}

	private void CheckResetSideTab(int id)
	{
		if(id == 1)
		{
			if (currentCoroutine1 != null)
			{
				StopCoroutine(currentCoroutine1);
				foreach (Transform item in sideTab1)
				{
					DOTween.Kill(item);
				}
			}
		}
		if(id == 2)
		{
			if (currentCoroutine2 != null)
			{
				StopCoroutine(currentCoroutine2);
				foreach (Transform item in sideTab2)
				{
					DOTween.Kill(item);
				}
			}
		}
	}

	IEnumerator OnEnableSideTab(Transform sideTab)
	{
		yield return new WaitForSeconds(0.05f);
		SoundManager.PlaySound(SoundEnum.insertPaper);
		float tempX = sideTab.transform.GetChild(0).position.x;
		foreach (Transform item in sideTab)
		{
			RectTransform rectTransform = item.GetComponent<RectTransform>();
			rectTransform.DOAnchorPos(new Vector2(177, rectTransform.anchoredPosition.y), 0f);

		}

		foreach (Transform item in sideTab)
		{
			item.GetComponent<RectTransform>().DOAnchorPosX(0f, 0.25f).SetEase(Ease.OutQuad);
			yield return new WaitForSeconds(_durationSldeTab);
		}
	}

	private void Start()
	{

		if (Common.IsTablet)
		{
			Debug.Log("modal market reponsive to tablet");
			gameObject.transform.localScale = scale_tablet;
		}
		tgNhanVien.onValueChanged.AddListener(delegate
		{
			OnChoosingPanel(pnNhanVien, tgNhanVien);
			tgSideTabHotNV.isOn = true;
			CheckResetSideTab(1);
			currentCoroutine1 = StartCoroutine(OnEnableSideTab(sideTab1));
		});
		tgNoiThat.onValueChanged.AddListener(delegate
		{
			OnChoosingPanel(pnNoiThat, tgNoiThat);
			tgSideTabHotNT.isOn = true;
			CheckResetSideTab(2);
			currentCoroutine2 = StartCoroutine(OnEnableSideTab(sideTab2));
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
