using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NOOD.Sound;
using Sirenix.OdinInspector;
using Spine.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutotrialUI : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI tutorialTextUI;
	[SerializeField] private Image tutorialImgUI;
	[SerializeField] private GameObject coinPrefab;
	[SerializeField] private RectTransform spawnPoint;
	[SerializeField] private RectTransform targetPoint;
	[SerializeField] private Transform coinParent;
	[SerializeField] private Button closeTutorialTextButton;
	[SerializeField] private Button tutorialClickNextStepButton;
	[SerializeField] private SkeletonGraphic skeletonGraphic;

	public Button TutorialClickNextStepButton { private set; get; }
	public Button CloseTutorialTextButton => closeTutorialTextButton;

	private void Awake()
	{
		closeTutorialTextButton.onClick.AddListener(CloseTutorialText);
	}
	public void CreateTutorialClickNextStepButton()
	{
		TutorialClickNextStepButton = Instantiate(tutorialClickNextStepButton, transform);
		TutorialClickNextStepButton.transform.SetAsFirstSibling();
	}
	public void DestroyTutorialClickNextStepButton()
	{
		Destroy(TutorialClickNextStepButton);
	}
	private void CloseTutorialText()
	{
		tutorialImgUI.gameObject.SetActive(false);
		closeTutorialTextButton.gameObject.SetActive(false);
	}

	private int totalCoins;
	private int coinsReachedTarget;
	public void SetTextTutorial(string text)
	{
		closeTutorialTextButton.gameObject.SetActive(true);
		tutorialImgUI.gameObject.SetActive(true);
		tutorialTextUI.text = text;
	}
	[Button]
	public void TriggerAddCoinEffect()
	{
		int coinCount = 10; // Số lượng coin
		totalCoins = coinCount;
		coinsReachedTarget = 0;
		StartCoroutine(SpawnCoins(coinCount));
	}

	public float radius = 20f;
	public float range = 1f;
	public float duration = 0.3f;
	private IEnumerator SpawnCoins(int count)
	{
		List<GameObject> coinsGO = new List<GameObject>();
		for (int i = 0; i < count; i++)
		{
			GameObject coin = Instantiate(coinPrefab, coinParent);
			RectTransform coinRect = coin.GetComponent<RectTransform>();

			// Set vị trí ban đầu
			coinRect.anchoredPosition = spawnPoint.anchoredPosition;

			// Random vị trí trong vòng tròn bán kính 100

			Vector2 randomDir = Random.insideUnitCircle.normalized * Random.Range(range, radius);

			Vector2 targetPos = spawnPoint.anchoredPosition + randomDir;

			// Animate tới vị trí xung quanh với easing OutBack
			coinRect.DOAnchorPos(targetPos, duration)
				.SetEase(Ease.OutBack)
				.SetDelay(i * 0.05f); // delay nhẹ cho hiệu ứng tỏa

			coinsGO.Add(coin);
			yield return new WaitForSeconds(0.1f);

		}

		//
		foreach (var coin in coinsGO)
		{
			RectTransform coinRect = coin.GetComponent<RectTransform>();
			StartCoroutine(MoveCoin(coinRect, targetPoint, 0.8f));
			yield return new WaitForSeconds(0.1f);
		}

	}

	private IEnumerator MoveCoin(RectTransform coin, RectTransform target, float duration)
	{
		// Chuyển vị trí thế giới của target thành vị trí local so với cha của coin
		Vector2 localTargetPos = coin.parent.InverseTransformPoint(target.position);

		coin.DOAnchorPos(localTargetPos, duration)
			.SetEase(Ease.InOutCubic)
			.OnComplete(() =>
			{
				SoundManager.PlaySound(SoundEnum.coin);
				Destroy(coin.gameObject, 0.1f);

				coinsReachedTarget++;
				if (coinsReachedTarget == totalCoins)
				{
					TutorialManager.Instance.TutorialStateMachine.TransitonToState((TutorialState)2);
				}
			});


		yield return new WaitForSeconds(0);
	}

	/// <summary>
	/// Chạy animation Spine trên SkeletonGraphic
	/// </summary>
	/// <param name="animationName">Tên animation trong Spine</param>
	/// <param name="loop">Có lặp lại không</param>
	public void PlayAnimation(string animationName, bool loop = true)
	{
		if (skeletonGraphic == null || skeletonGraphic.AnimationState == null) return;
		skeletonGraphic.Initialize(true);
		skeletonGraphic.AnimationState.SetAnimation(0, animationName, loop);

		skeletonGraphic.Update();
	}
}
