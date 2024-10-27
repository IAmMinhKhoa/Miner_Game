using DG.Tweening;
using Sirenix.OdinInspector;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MiniGameUI : MonoBehaviour
{
	[SerializeField] private float _fadeSpeed = 3f;
	[SerializeField] private Button miniGame_FlappyBird;
	[SerializeField] private Button miniGame_Fruit;
	[SerializeField] private Button btn_back;
	[SerializeField] private Button btn_redeempoints;
	private CanvasGroup _canvasGroup;
	private CancellationTokenSource _disableToken;
	private Vector3 originalScaleRedeemPoints;
	private Vector3[] originalScalesGameMachine;
	private float maxScaleFactor = 1.11f;
	[Header("Spine")]
	public SkeletonGraphic points_redemption_booth;
	public SkeletonGraphic[] game_machine;
	private void Start()
	{
		originalScaleRedeemPoints = points_redemption_booth.transform.localScale;
		originalScalesGameMachine = new Vector3[game_machine.Length];
		for (int i = 0; i < game_machine.Length; i++)
		{
			originalScalesGameMachine[i] = game_machine[i].transform.localScale;
		}
	}
	void Awake()
	{
		_canvasGroup = this.GetComponent<CanvasGroup>();
		miniGame_FlappyBird.onClick.AddListener(() => { MiniGame(1); });
		miniGame_Fruit.onClick.AddListener(() => { MiniGame(2); });
		_disableToken = new CancellationTokenSource();
		btn_back.onClick.AddListener(OnBack);
		btn_redeempoints.onClick.AddListener(OnRedeempoints);
	}
	void OnDisable()
	{
		_disableToken.Cancel();
	}

	private void OnDestroy()
	{
		btn_back.onClick.RemoveListener(FadeOutContainer);
	}
	public void Show()
	{
		this.gameObject.SetActive(true);
		_canvasGroup.interactable = true;
		_canvasGroup.DOFade(1, _fadeSpeed).SetEase(Ease.Flash);
	}
	public void Hide()
	{
		_canvasGroup.interactable = false;
		_canvasGroup.DOFade(0, _fadeSpeed).SetEase(Ease.Flash).OnComplete(() => this.gameObject.SetActive(false));
	}
	public void OnBack()
	{
		gameObject.SetActive(false);
	}
	public void MiniGame(int index)
	{
		SkeletonGraphic selectedMachine = game_machine[index - 1];
		selectedMachine.transform.DOKill();
		selectedMachine.transform.localScale = originalScalesGameMachine[index - 1];
		selectedMachine.transform.DOScale(originalScalesGameMachine[index - 1] * 1.05f, 0.2f)
			.OnComplete(() =>
			{
				selectedMachine.transform.DOScale(originalScalesGameMachine[index - 1], 0.2f).OnComplete(() =>
				{
					SceneManager.LoadScene(index, LoadSceneMode.Additive);
				});
			});
	}
	public void OnRedeempoints()
	{
		points_redemption_booth.transform.DOKill();
		points_redemption_booth.AnimationState.SetAnimation(0,"Click",false);
		if (points_redemption_booth.transform.localScale.x > maxScaleFactor * originalScaleRedeemPoints.x)
		{
			points_redemption_booth.transform.localScale = originalScaleRedeemPoints;
		}
		Vector3 punchScale = originalScaleRedeemPoints * 1.05f;
		points_redemption_booth.transform.DOScale(punchScale, 0.2f)
			.OnComplete(() =>
			{
				points_redemption_booth.transform.DOScale(originalScaleRedeemPoints, 0.2f);
				points_redemption_booth.AnimationState.SetAnimation(0, "Idle", true);
			});
	}
	#region AnimateUI
	[Button]
	public void FadeInContainer()
	{
		gameObject.SetActive(true);
		Vector2 posCam = CustomCamera.Instance.GetCurrentTransform().position;
		Debug.Log("khoaa:" + posCam);
		gameObject.transform.localPosition = new Vector2(posCam.x - 2000, posCam.y); //Left Screen
		gameObject.transform.DOLocalMoveX(0, 0.6f).SetEase(Ease.OutQuart);


	}
	[Button]
	public void FadeOutContainer()
	{
		Vector2 posCam = CustomCamera.Instance.GetCurrentTransform().position;
		gameObject.transform.DOLocalMoveX(posCam.x - 2000f, 0.6f).SetEase(Ease.InQuart).OnComplete(() =>
		{
			gameObject.SetActive(false);
		});

	}
	#endregion
}
