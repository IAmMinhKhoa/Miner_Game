using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DialogManager : Patterns.Singleton<DialogManager>
{
    public Transform anchorDialog;
    private Dictionary<DialogIndex, BaseDialog> dic_Dialog = new Dictionary<DialogIndex, BaseDialog>();
    private List<BaseDialog> ls_dialog_show = new List<BaseDialog>();
    // Start is called before the first frame update
    void Start()
    {
        foreach (DialogIndex dl in DialogConfig.dialogIndices)
        {
            string dialog_name = dl.ToString();
            GameObject dialog_object = Instantiate(Resources.Load("Dialog/" + dialog_name, typeof(GameObject))) as GameObject;
            dialog_object.transform.SetParent(anchorDialog, false);
            BaseDialog base_dialog = dialog_object.GetComponent<BaseDialog>();
            dic_Dialog.Add(dl, base_dialog);
            dialog_object.SetActive(false);

        }
    }
    public void ShowDialog(DialogIndex dialogIndex, DialogParam param = null, Action callback = null)
    {
        BaseDialog base_dl = dic_Dialog[dialogIndex];
        base_dl.gameObject.SetActive(true);
        base_dl.Setup(param);
        DialogCallback dialog_callback = new DialogCallback();
        dialog_callback.callback = callback;

        base_dl.SendMessage("ShowDialog", dialog_callback);
        if(!ls_dialog_show.Contains(base_dl))
        {
            ls_dialog_show.Add(base_dl);
        }
    }
    public void HideDialog(DialogIndex dialogIndex)
    {
        BaseDialog base_dl = dic_Dialog[dialogIndex];
      
        DialogCallback dialog_callback = new DialogCallback();
        dialog_callback.callback = () =>
        {
            base_dl.gameObject.SetActive(false);
        };

        base_dl.SendMessage("HideDialog", dialog_callback);
        if (ls_dialog_show.Contains(base_dl))
        {
            ls_dialog_show.Remove(base_dl);
        }
    }
    public void HideAllDialog()
    {
        foreach(BaseDialog dl in ls_dialog_show)
        {
            DialogCallback dialog_callback = new DialogCallback();
            dialog_callback.callback = () =>
            {
                dl.gameObject.SetActive(false);

            };
            dl.SendMessage("HideDialog", dialog_callback);
        }
        ls_dialog_show.Clear();
    }
    // Update is called once per frame

    //private void OnShowNewView(ViewIndex viewIndex, ViewParam param = null, Action callback = null)
    //{
    //    current_view = dic_Dialog[viewIndex];
    //    current_view.gameObject.SetActive(true);
    //    current_view.Setup(param);
    //    ViewCallback viewCallback = new ViewCallback();
    //    viewCallback.callback = callback;

    //    current_view.SendMessage("ShowView", viewCallback);
    //}
}
public class DialogCallback
{
    public Action callback;

}
