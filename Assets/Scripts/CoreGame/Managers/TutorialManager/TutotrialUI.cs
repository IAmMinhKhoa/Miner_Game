using System.Collections;
using System.Collections.Generic;
using NOOD.Sound;
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

	public void TriggerAddCoinEffect()
	{
		int coinCount = 5; // Số lượng coin
		totalCoins = coinCount;
		coinsReachedTarget = 0;
		StartCoroutine(SpawnCoins(coinCount));
	}

	private IEnumerator SpawnCoins(int count)
	{
		for (int i = 0; i < count; i++)
		{
			GameObject coin = Instantiate(coinPrefab, coinParent);
			RectTransform coinRect = coin.GetComponent<RectTransform>();
			coinRect.anchoredPosition = spawnPoint.anchoredPosition;

			StartCoroutine(MoveCoin(coinRect, targetPoint.anchoredPosition, 1f));
			yield return new WaitForSeconds(0.2f);
		}
	}

	private IEnumerator MoveCoin(RectTransform coin, Vector2 target, float duration)
	{
		float elapsedTime = 0;
		Vector2 startPos = coin.anchoredPosition;

		while (elapsedTime < duration)
		{
			coin.anchoredPosition = Vector2.Lerp(startPos, target, elapsedTime / duration);
			elapsedTime += Time.deltaTime;
			yield return null;
		}

		coin.anchoredPosition = target;
		SoundManager.PlaySound(SoundEnum.coin);
		Destroy(coin.gameObject, 0.1f);

		coinsReachedTarget++;
		if (coinsReachedTarget == totalCoins)
		{
			TutorialManager.Instance.TutorialStateMachine.TransitonToState((TutorialState)2);
		}
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
