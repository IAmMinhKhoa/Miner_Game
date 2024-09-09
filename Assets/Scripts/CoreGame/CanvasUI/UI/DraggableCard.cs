using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    private CanvasGroup canvasGroup;
    private GameObject dragObject;
    private Camera mainCamera;
    private bool isDragging = false;
    [SerializeField] ManagerElementUI currentCard;
	private bool isAction=false;
    private void Update()
    {
    }
    private void Awake()
    {
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        TryGetComponent(out canvasGroup);
        mainCamera = Camera.main;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isDragging) return;
        isDragging = true;
        CreateDragObject();
        PrepareForDrag();
       
    }

    private void PrepareForDrag()
    {
        canvasGroup.blocksRaycasts = false;
        currentCard.IsSelected = true;
    }

    private void CreateDragObject()
    {
        dragObject = Instantiate(gameObject, transform);
        var rectTransform = dragObject.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(170, 170);

        AddDragCanvas(dragObject);
        DisableRaycasts(dragObject);
    }

    private void AddDragCanvas(GameObject obj)
    {
        var canvas = obj.AddComponent<Canvas>();
        canvas.overrideSorting = true;
        canvas.sortingLayerName = "GameUI";
        canvas.sortingOrder = 1;
    }

    private void DisableRaycasts(GameObject obj)
    {
        var dragCanvasGroup = obj.GetComponent<CanvasGroup>();
        dragCanvasGroup.blocksRaycasts = false;
        dragCanvasGroup.interactable = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging) return;
        UpdateDragObjectPosition();
    }

    private void UpdateDragObjectPosition()
    {
        var screenPoint = Input.mousePosition;
        screenPoint.z = 1000f; // Distance of the plane from the camera
        dragObject.transform.position = mainCamera.ScreenToWorldPoint(screenPoint);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("Handle-Merge1:" + isDragging+"/"+ eventData.pointerEnter);
        if (!isDragging|| eventData.pointerEnter == null) return;
        Debug.Log("Handle-Merge2:" + isDragging + "/" + eventData.pointerEnter);
        HandleDropOnShaft(eventData);
        HandleManagerMerge(eventData.pointerEnter);
        CleanupAfterDrag();
    }

    private void HandleDropOnShaft(PointerEventData eventData)
    {

        if (eventData.pointerEnter.TryGetComponent(out InformationBlockShaft blockShaftManager))
        {
            AssignManagerToShaft(blockShaftManager, eventData.pointerDrag);
            RefreshUI();
        }
    }

    private void AssignManagerToShaft(InformationBlockShaft blockShaftManager, GameObject pointerDrag)
    {
        var managerData = pointerDrag.GetComponent<ManagerElementUI>().Data;
        blockShaftManager.SetData(managerData);
    }

    private void RefreshUI()
    {
      //  var currentManager = canvasGroup.GetComponent<ManagerElementUI>().Data;
	//	Debug.Log("khoa:"+currentManager)
        ManagerChooseUI.OnRefreshManagerTab?.Invoke(currentCard.Data.BoostType,false);
        ManagerSelectionShaft.OnReloadManager?.Invoke();
    }


    private void HandleManagerMerge(GameObject pointerEnterSecond) //event run when drop card
    {
        Debug.Log("Handle-Merge:" + pointerEnterSecond.GetComponent<DraggableCard>());
        if (pointerEnterSecond.GetComponent<DraggableCard>() == null) return;

        var firstManager = currentCard.Data;
        var secondManager = pointerEnterSecond.GetComponent<ManagerElementUI>();
		if (ManagersController.Instance.MergeManager(firstManager, secondManager.Data)) //if can merge
        {
			//Play Fx
			secondManager.RunFxMergeSuccess();
			StartCoroutine(DelayedManagerRemoval(firstManager, secondManager.Data));
		}
		else //can not merge
        {

			if (firstManager.Level == secondManager.Data.Level && firstManager.Level == ManagerLevel.Executive)
			{
				ManagerChooseUI.MergeSuccess?.Invoke(TypeMerge.FailLevelMax);
			}

			else
			{
				ManagerChooseUI.MergeSuccess?.Invoke(TypeMerge.FailNotSameLevel);
			}
        }
    }

	private IEnumerator DelayedManagerRemoval(Manager firstManager, Manager secondManager)
	{
		yield return new WaitForSeconds(0.35f);
		ManagersController.Instance.MergeManagerTimes(firstManager, secondManager);
		ManagersController.Instance.UpgradeManager(firstManager);
		ManagersController.Instance.RemoveManager(secondManager);
		ManagerChooseUI.MergeSuccess?.Invoke(TypeMerge.Success);
		ManagerChooseUI.OnRefreshManagerTab?.Invoke(firstManager.BoostType, false);
		ManagerSelectionShaft.OnReloadManager?.Invoke();
	}

	public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null && eventData.pointerDrag.GetComponent<DraggableCard>() != null)
        {
            var dragManagerUI = eventData.pointerDrag.GetComponent<ManagerElementUI>();
            var enterManagerUI = eventData.pointerEnter?.GetComponent<ManagerElementUI>();

            if (dragManagerUI != null)
            {
              
                bool CanMeger= ManagersController.Instance.CanMergeManagers(dragManagerUI.Data, enterManagerUI.Data);
                enterManagerUI.CanMerge = CanMeger;
            }
           

            ScaleUp();
        }
    }

    private void ScaleUp()
    {
        transform.DOScale(new Vector2(1.2f, 1.2f), 0.2f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ScaleDown();
        currentCard.ClearStateCard();
    }

    private void ScaleDown()
    {
        transform.DOScale(new Vector2(1f, 1f), 0.2f);
    }

    private void CleanupAfterDrag()
    {
        RestoreCanvasGroup();
        DestroyDragObject();
        ForceRefreshParentLayout();
        isDragging = false;
    }

    private void RestoreCanvasGroup()
    {
        currentCard.IsSelected = false ;
        canvasGroup.blocksRaycasts = true;
    }

    private void DestroyDragObject()
    {
        if (dragObject != null)
        {
            Destroy(dragObject);
            dragObject = null;
        }
    }

    private void ForceRefreshParentLayout()
    {
        var layoutGroup = GetComponentInParent<LayoutGroup>();
        if (layoutGroup != null)
        {
            layoutGroup.enabled = true;
            LayoutRebuilder.ForceRebuildLayoutImmediate(layoutGroup.GetComponent<RectTransform>());
        }
    }
}
