using System.Collections;
using System.Collections.Generic;
using NOOD;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class DragAndDrop : MonoBehaviour, IPointerDownHandler, IDragAndDropEvent, IDropHandler, IDragHandler, IEndDragHandler
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
        Debug.Log(this.name + " drag");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        ForceRefreshParentLayout();
    }

    public void OnDrop(PointerEventData eventData)
    {
        if(DragAndDropManager.Instance.DragAndDropObject != null)
        {
            Debug.Log("Drag object " + DragAndDropManager.Instance.DragAndDropObject.name);
        }
        else
        {
            Debug.Log("No drag and drop object");
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        DragAndDropManager.Instance.DragAndDropObject = this.gameObject;
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
}
