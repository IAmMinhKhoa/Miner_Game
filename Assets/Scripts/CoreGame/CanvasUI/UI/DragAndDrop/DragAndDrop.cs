using System.Collections;
using System.Collections.Generic;
using NOOD;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class DragAndDrop : MonoBehaviour, IBeginDragHandler, IDropHandler, IDragHandler, IEndDragHandler
{
    private RectTransform _rectTransform;

    void Awake()
    {
        _rectTransform = null;
        this.TryGetComponent<RectTransform>(out _rectTransform);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(_rectTransform == null) return;
        else
        {
            _rectTransform.anchoredPosition += eventData.delta;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("End drag");
        DragAndDropManager.Instance.DragAndDropObject = null;
        ForceRefreshParentLayout();
    }

    public void OnDrop(PointerEventData eventData)
    {
        if(DragAndDropManager.Instance.LastDragObject != null)
        {
            var firstManager = this.GetComponent<ManagerElementUI>().Data;
            var secondManager = DragAndDropManager.Instance.LastDragObject.GetComponent<ManagerElementUI>().Data;
            ManagersController.Instance.MergeManager(firstManager, secondManager);
            ManagerChooseUI.OnRefreshManagerTab?.Invoke(firstManager.BoostType);

        }
        else
        {
            Debug.Log("No drag and drop object");
        }
    }

    private void ForceRefreshParentLayout()
    {
        LayoutGroup layoutGroup = this.GetComponentInParent<LayoutGroup>();
        if (layoutGroup)
        {
            layoutGroup.enabled = true;
            LayoutRebuilder.ForceRebuildLayoutImmediate(layoutGroup.GetComponent<RectTransform>());
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        DragAndDropManager.Instance.DragAndDropObject = this.gameObject;
    }
}
