using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SettingUI : MonoBehaviour
{
    [SerializeField] private float _fadeSpeed = 3f;
    [SerializeField] private Button _closeButton;
    private CanvasGroup _canvasGroup;
    private CancellationTokenSource _disableToken;

    void Awake()
    {
        _canvasGroup = this.GetComponent<CanvasGroup>();
        _closeButton.onClick.AddListener(Hide);
        _disableToken = new CancellationTokenSource();
        Hide();
    }
    void OnDisable()
    {
        _disableToken.Cancel();
    }
    void OnDestroy()
    {
        _closeButton.onClick.RemoveListener(Hide);
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
}
