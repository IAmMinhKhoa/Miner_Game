using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InformationBlockShaft : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] TMP_Text textIndex;
    [SerializeField] Image icon;
    [SerializeField] TMP_Text textLevel;
    [SerializeField] Image sliderTimeActive;
    [SerializeField] Image sliderTimeCD;
    [SerializeField] GameObject objDrag;
    [SerializeField] Sprite iconDefault;

    private Shaft shaft;
    private GameObject dragObject;
    private Camera mainCamera;
    private bool isDragging = false;

    private float tickRate = 1.5f;
    private float nextTickTime = 0f;

    #region Initialization

    private void Start()
    {
        mainCamera = Camera.main;
    }

    public void SetDataInit(Shaft shaft)
    {
        this.shaft = shaft;
        UpdateUI();
    }

    #endregion

    #region UI Updates

    private void Update()
    {
        if (Time.time >= nextTickTime)
        {
            // Do something periodically
            RenderSkillManager();
            nextTickTime = Time.time + tickRate;
        }
    }

    private void RenderSkillManager()
    {
        if (shaft.ManagerLocation.Manager == null) return;

        Manager manager = shaft.ManagerLocation.Manager;
        if (manager.CurrentBoostTime > 0)
        {
            float value = manager.CurrentBoostTime / (manager.BoostTime * 60);
            sliderTimeActive.fillAmount = value;
        }
        else if (manager.CurrentCooldownTime > 0)
        {
            sliderTimeActive.fillAmount = 0;
            float value = manager.CurrentCooldownTime / (manager.CooldownTime * 60);
            sliderTimeCD.fillAmount = value;
        }
        else
        {
            sliderTimeActive.fillAmount = 1;
        }
    }

    private void UpdateUI()
    {
        textIndex.text = (shaft.shaftIndex + 1).ToString();
        Manager manager = shaft.ManagerLocation.Manager;

        if (manager != null)
        {
            icon.sprite = manager.Icon;
            textLevel.text = ((int)manager.Level).ToString();
        }
        else
        {
            icon.sprite = iconDefault;
            textLevel.text = string.Empty;
        }
    }

    public void SetData(Manager manager) //use in case when drag and drop card to list shaft -> update data to shaft
    {
        if (manager != null)
        {
            icon.sprite = manager.Icon;
            textLevel.text = ((int)manager.Level).ToString();
            ManagersController.Instance.AssignManager(manager, shaft.ManagerLocation);
        }
        else
        {
            icon.sprite = iconDefault;
            textLevel.text = string.Empty;
        }
    }

    #endregion

    #region Event Handling

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (IsDraggable(eventData))
        {
            ScaleUp();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ScaleDown();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (shaft.ManagerLocation.Manager != null)
        {
            ManagersController.Instance.OpenManagerDetailPanel(true, shaft.ManagerLocation.Manager);
        }
    }

    #endregion

    #region Dragging Logic

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!CanBeginDrag()) return;

        isDragging = true;
        CreateDragObject();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!CanDrag()) return;
        UpdateDragObjectPosition();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (shaft.ManagerLocation.Manager == null) return;

        HandleDrop(eventData);
        DestroyDragObject();
        isDragging = false;
    }

    private bool CanBeginDrag()
    {
        return shaft.ManagerLocation.Manager != null && !isDragging;
    }

    private bool CanDrag()
    {
        return shaft.ManagerLocation.Manager != null && isDragging;
    }

    private void CreateDragObject()
    {
        dragObject = Instantiate(objDrag, transform);
        dragObject.transform.localScale = new Vector3(1.05f, 1.05f, 1.05f);
        AddDragCanvas(dragObject);
    }

    private void UpdateDragObjectPosition()
    {
        var screenPoint = Input.mousePosition;
        screenPoint.z = 1000f; // Distance from the camera

        dragObject.transform.position = mainCamera.ScreenToWorldPoint(screenPoint);
    }

    private void HandleDrop(PointerEventData eventData)
    {
        if (eventData.pointerEnter == null) return;

        if (IsDroppingOutside(eventData.pointerEnter))
        {
            ManagersController.Instance.UnassignManager(shaft.ManagerLocation.Manager);
        }
        else if (IsSwappingManager(eventData))
        {
            SwapManager(eventData);
        }
    }

    private void DestroyDragObject()
    {
        if (dragObject != null)
        {
            Destroy(dragObject);
            dragObject = null;
        }
    }

    #endregion

    #region Helper Methods

    private bool IsDraggable(PointerEventData eventData)
    {
        return eventData.pointerDrag != null &&
               (eventData.pointerDrag.GetComponent<DraggableCard>() != null ||
                eventData.pointerDrag.GetComponent<InformationBlockShaft>() != null);
    }

    private bool IsDroppingOutside(GameObject target)
    {
        return !target.GetComponent<InformationBlockShaft>();
    }

    private bool IsSwappingManager(PointerEventData eventData)
    {
        return eventData.pointerEnter != gameObject &&
               eventData.pointerEnter.GetComponent<InformationBlockShaft>() != null;
    }

    private void ScaleUp()
    {
        transform.DOScale(new Vector2(1.2f, 1.2f), 0.2f);
    }

    private void ScaleDown()
    {
        transform.DOScale(new Vector2(1f, 1f), 0.2f);
    }

    private void AddDragCanvas(GameObject obj)
    {
        var canvas = obj.AddComponent<Canvas>();
        canvas.overrideSorting = true;
        canvas.sortingLayerName = "GameUI";
        canvas.sortingOrder = 1;

        var dragCanvasGroup = obj.GetComponent<CanvasGroup>();
        dragCanvasGroup.blocksRaycasts = false;
    }

    private void SwapManager(PointerEventData eventData)
    {
        var targetBlockShaft = eventData.pointerEnter.GetComponent<InformationBlockShaft>();
        if (targetBlockShaft == null) return;

        var currentManager = eventData.pointerDrag.GetComponent<InformationBlockShaft>().shaft.ManagerLocation.Manager;
        targetBlockShaft.SetData(currentManager);

        ManagerChooseUI.OnRefreshManagerTab?.Invoke(currentManager.Location.Manager.BoostType);
        ManagerSelectionShaft.OnReloadManager?.Invoke();
    }

    #endregion
}