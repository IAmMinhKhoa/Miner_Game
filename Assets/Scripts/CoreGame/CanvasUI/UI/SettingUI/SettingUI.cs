using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using NOOD.Sound;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class SettingUI : MonoBehaviour
{
	private Vector3 scale_tablet = new Vector3(1.2f, 1.2f, 1.2f);
	[SerializeField] private float _fadeSpeed = 3f;
    [SerializeField] private Button _closeButton;
    private CanvasGroup _canvasGroup;
    private CancellationTokenSource _disableToken;

    void Awake()
    {
        _canvasGroup = this.GetComponent<CanvasGroup>();
        _closeButton.onClick.AddListener(FadeOutContainer);
        _disableToken = new CancellationTokenSource();
    }
	private void Start()
	{
		if (Common.IsTablet)
		{
			gameObject.transform.localScale = scale_tablet;
		}
	}
	void OnDisable()
    {
        _disableToken.Cancel();
    }
    void OnDestroy()
    {
        _closeButton.onClick.RemoveListener(FadeOutContainer);
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


	#region AnimateUI
	[Button]
	public void FadeInContainer()
	{
		gameObject.SetActive(true);
		Vector2 posCam = CustomCamera.Instance.GetCurrentTransform().position;
		Debug.Log("khoaa:" + posCam);
		gameObject.transform.localPosition = new Vector2(posCam.x - 2000, posCam.y); //Left Screen
		gameObject.transform.DOLocalMoveX(0, 0.4f).SetEase(Ease.OutQuart);


	}
	[Button]
	public void FadeOutContainer()
	{
		SoundManager.PlaySound(SoundEnum.mobileTexting2);
		Vector2 posCam = CustomCamera.Instance.GetCurrentTransform().position;
		gameObject.transform.DOLocalMoveX(posCam.x - 2000f, 0.6f).SetEase(Ease.InQuart).OnComplete(() =>
		{
			Destroy(this.gameObject);
		});

	}
	#endregion
}
