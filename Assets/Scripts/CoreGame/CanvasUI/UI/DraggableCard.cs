using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
   
    private CanvasGroup canvasGroup;
    private GameObject _dragObject;
    private Camera _MainCamera;
    private void Awake()
    {
        this.TryGetComponent<CanvasGroup>(out canvasGroup);
        _MainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;

        _dragObject = Instantiate(gameObject, transform);
        _dragObject.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 250);

        var canvas = _dragObject.AddComponent<Canvas>();
        canvas.overrideSorting = true;
        canvas.sortingLayerName = "GameUI";
        canvas.sortingOrder = 1;
        _dragObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        var screenPoint = (Vector3)Input.mousePosition;
        screenPoint.z = 1000f; //distance of the plane from the camera

        _dragObject.transform.position = _MainCamera.ScreenToWorldPoint(screenPoint);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.pointerEnter == null) return;
        canvasGroup.blocksRaycasts = true;
  
      


        InformationBlockShaft _blockShaftManager;
        eventData.pointerEnter.TryGetComponent<InformationBlockShaft>(out _blockShaftManager);
        if (_blockShaftManager != null)
        {
            var _currentDataCard = eventData.pointerDrag.GetComponent<ManagerElementUI>().Data;
            _blockShaftManager.SetData(_currentDataCard);
            eventData.pointerDrag.GetComponent<ManagerElementUI>().IsAssigned = true;

            ManagerSelectionShaft.OnReloadManager?.Invoke();

        }


        DestroyDragObject();    
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
    private void DestroyDragObject()
    {
        ManagerSelectionShaft.CanDragCardManager = false;

        Destroy(_dragObject);
        _dragObject = null;
    }
}
