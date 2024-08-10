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


    private Shaft _shaft;
    private GameObject _dragObject;
    private Camera _MainCamera;

    private void Start()
    {
        _MainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }
    public void SetDataInit(Shaft shaft)
    {
        textIndex.text = (shaft.shaftIndex + 1).ToString();
        _shaft = shaft;

        Manager manager = shaft.ManagerLocation.Manager;
        if (manager != null)
        {
            icon.sprite = manager.Icon;
            textLevel.text = ((int)manager.Level).ToString();
        }
        else
        {
            icon.sprite = null;
            textLevel.text = "";
        }
    }

    public void SetData(Manager manager)
    {
        if (manager != null)
        {
            icon.sprite = manager.Icon;
            textLevel.text = ((int)manager.Level).ToString();
            ManagersController.Instance.AssignManager(manager, _shaft.ManagerLocation);
           
        }
        else
        {
            icon.sprite = null;
            textLevel.text = "";
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null && eventData.pointerDrag.GetComponent<DraggableCard>())
            transform.DOScale(new Vector2(1.2f, 1.2f), 0.2f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(new Vector2(1f, 1f), 0.2f);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        
        ManagersController.Instance.OpenManagerDetailPanel(true, _shaft.ManagerLocation.Manager);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_shaft.ManagerLocation.Manager == null) return;

        /* _originalParent = objIcon.transform.parent;
         _originalPosition = _rectTransform.anchoredPosition;
         objIcon.transform.SetParent(transform.root, true);*/


        _dragObject = Instantiate(gameObject, transform);
        _dragObject.transform.localScale = new Vector3(1.05f, 1.05f, 1.05f);

        var canvas = _dragObject.AddComponent<Canvas>();
        canvas.overrideSorting = true;
        canvas.sortingLayerName = "GameUI";
        canvas.sortingOrder = 1;
        _dragObject.GetComponent<CanvasGroup>().blocksRaycasts = false   ;
    }

    public void OnDrag(PointerEventData eventData)
    { 
        if ( _shaft.ManagerLocation.Manager == null) return;
        var screenPoint = (Vector3)Input.mousePosition;
        screenPoint.z = 1000f; //distance of the plane from the camera

        _dragObject.transform.position = _MainCamera.ScreenToWorldPoint(screenPoint);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (_shaft.ManagerLocation.Manager == null) return;

       

        if (eventData.pointerEnter != null)
        {
            if (!eventData.pointerEnter.GetComponent<InformationBlockShaft>())
            {
                Debug.Log("Drag end drag on: " + eventData.pointerEnter.name);
                ManagersController.Instance.UnassignManager(_shaft.ManagerLocation.Manager);
            }
        }

        DestroyDragObject();

    }
    private void DestroyDragObject()
    {
        ManagerSelectionShaft.CanDragCardManager = false;

        Destroy(_dragObject);
        _dragObject = null;
    }
}