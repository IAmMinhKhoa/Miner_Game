using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;


[RequireComponent(typeof(CanvasGroup))]
public class BaseDialogAnimation : MonoBehaviour
{
    private CanvasGroup canvasGroup_;
    // Start is called before the first frame update
    void Awake()
    {
        canvasGroup_ = gameObject.GetComponent<CanvasGroup>();

    }

    // Update is called once per frame
    public virtual void OnShowDialog(Action callback)
    {
        canvasGroup_.DOFade(1, 0.5f).OnComplete(() =>
        {
            callback?.Invoke();
        }).SetUpdate(true);
    }
    public virtual void OnHideDialog(Action callback)
    {
        canvasGroup_.DOFade(0, 0.25f).OnComplete(() =>
        {
            callback?.Invoke();
        }).SetUpdate(true);
    }
}
