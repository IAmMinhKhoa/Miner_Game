using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseDialog : MonoBehaviour
{
    public DialogIndex dialogIndex;
    private BaseDialogAnimation baseDialogAnimation;
    private RectTransform rect_;
    // Start is called before the first frame update
    void Awake()
    {
        rect_ = gameObject.GetComponent<RectTransform>();
        baseDialogAnimation = gameObject.GetComponentInChildren<BaseDialogAnimation>();
    }

    // Update is called once per frame
    public virtual void Setup(DialogParam data)
    { }
    private void ShowDialog(DialogCallback dialogCallback)
    {
        rect_.SetAsLastSibling();
        baseDialogAnimation.OnShowDialog(() =>
        {

            OnShowDialog();
            dialogCallback.callback?.Invoke();
        });
    }
    public virtual void OnShowDialog()
    {


    }
    private void HideDialog(DialogCallback dialogCallback)
    {
        baseDialogAnimation.OnHideDialog(() => {

            OnHideDialog();
            dialogCallback.callback?.Invoke();
        });
    }
    public virtual void OnHideDialog()
    {

    }
}