using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InformationBlockShaft : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IDropHandler, IEndDragHandler
{
    [SerializeField] TMP_Text textIndex;
    [SerializeField] Image icon;
    [SerializeField] TMP_Text textLevel;
    [SerializeField] GameObject objIcon;

    private RectTransform _rectTransform;
    private Vector2 _originalPosition;
    private Transform _originalParent;
    private Canvas _parentCanvas;
    private Shaft _shaft;

    private void Awake()
    {
        _rectTransform = objIcon.GetComponent<RectTransform>();
    }
    private void Start()
    {
        _parentCanvas = GetComponentInParent<Canvas>();
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

        _originalParent = objIcon.transform.parent;
        _originalPosition = _rectTransform.anchoredPosition;
        objIcon.transform.SetParent(transform.root, true);
    }

    public void OnDrag(PointerEventData eventData)
    { 
        if (_rectTransform == null || _shaft.ManagerLocation.Manager == null) return;
        _rectTransform.anchoredPosition += eventData.delta/_parentCanvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (_shaft.ManagerLocation.Manager == null) return;

        objIcon.transform.SetParent(_originalParent, true);
        _rectTransform.anchoredPosition = _originalPosition;

        if (eventData.pointerEnter != null)
        {
            if (!eventData.pointerEnter.GetComponent<InformationBlockShaft>())
            {
                Debug.Log("Drag end drag on: " + eventData.pointerEnter.name);
                ManagersController.Instance.UnassignManager(_shaft.ManagerLocation.Manager);
            }
        }
            
    }

    public void OnDrop(PointerEventData eventData)
    {
     
    }
}