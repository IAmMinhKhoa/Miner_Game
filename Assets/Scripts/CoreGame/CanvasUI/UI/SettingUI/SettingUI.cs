using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
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
    }
    void OnDisable()
    {
        _disableToken.Cancel();
    }
    void OnDestroy()
    {
        _closeButton.onClick.RemoveListener(Hide);
    }

    public async void Show()
    {
        this.gameObject.SetActive(true);
        _canvasGroup.interactable = true;
        while (_canvasGroup.alpha < 1)
        {
            Debug.Log("Increase");
            _canvasGroup.alpha += Time.deltaTime * _fadeSpeed;
            await UniTask.Yield();
        }
    }
    public async void Hide()
    {
        _canvasGroup.interactable = false;
        while (_canvasGroup.alpha > 0)
        {
            Debug.Log("Decrease");
            _canvasGroup.alpha -= Time.deltaTime * _fadeSpeed;
            await UniTask.Yield();
        }
        _canvasGroup.alpha = 0;
        this.gameObject.SetActive(false);
    }
}
