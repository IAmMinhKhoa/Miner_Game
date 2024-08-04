using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    private RectTransform _rectTransform;
    private CanvasGroup canvasGroup;
    private Transform originalParent;
    private int originalSiblingIndex;
    private Canvas _parentCanvas;
    private void Awake()
    {
        this.TryGetComponent<CanvasGroup>(out canvasGroup);
        this.TryGetComponent<RectTransform>(out _rectTransform);
        _parentCanvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;

        originalParent = transform.parent;
        originalSiblingIndex = transform.GetSiblingIndex();

        transform.SetParent(transform.root, true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_rectTransform == null) return;
        _rectTransform.anchoredPosition += eventData.delta / _parentCanvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.pointerEnter == null) return;
        canvasGroup.blocksRaycasts = true;
  
        transform.SetParent(originalParent, true);
        transform.SetSiblingIndex(originalSiblingIndex);


        InformationBlockShaft _blockShaftManager;
        eventData.pointerEnter.TryGetComponent<InformationBlockShaft>(out _blockShaftManager);
        if (_blockShaftManager != null)
        {
            var _currentDataCard = eventData.pointerDrag.GetComponent<ManagerElementUI>().Data;
            _blockShaftManager.SetData(_currentDataCard);
            eventData.pointerDrag.GetComponent<ManagerElementUI>().IsAssigned = true;

            ManagerSelectionShaft.OnReloadManager?.Invoke();

        }



        ForceRefreshParentLayout();
    }

    public void OnDrop(PointerEventData eventData)
    {


        if (eventData.pointerEnter != null && eventData.pointerEnter.GetComponent<DraggableCard>())
        {
            // Handle merge logic here
            var draggedCard = eventData.pointerDrag.GetComponent<DraggableCard>();
            var targetCard = eventData.pointerEnter.GetComponent<DraggableCard>();
            if (draggedCard == null || targetCard == null || draggedCard == targetCard) return;

            var firstM = draggedCard.GetComponent<ManagerElementUI>().Data;
            var secondM = targetCard.GetComponent<ManagerElementUI>().Data;

            if (ManagersController.Instance.MergeManager(firstM, secondM))
            {
                ManagerChooseUI.OnRefreshManagerTab?.Invoke(firstM.BoostType);
                ManagerSelectionShaft.OnReloadManager?.Invoke(); //after merge -> reload render srollview shaft
            }
        }
       
    }

    private void ForceRefreshParentLayout()
    {
        LayoutGroup layoutGroup = GetComponentInParent<LayoutGroup>();
        if (layoutGroup)
        {
            layoutGroup.enabled = true;
            LayoutRebuilder.ForceRebuildLayoutImmediate(layoutGroup.GetComponent<RectTransform>());
        }
    }
}
