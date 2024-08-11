using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ManagerSelectionShaft : MonoBehaviour
{
    public static Action OnReloadManager;
    [SerializeField] InformationBlockShaft _prefabShaft;
    [SerializeField] Transform _parentContent;

    [SerializeField] private ScrollRect scrollRect;   // Reference to the Scroll View's ScrollRect component
    [SerializeField] private float scrollSpeed = 0.1f; // Speed at which the scroll happens

    private List<Shaft> _shaftManagers
    {
        get
        {
            return ShaftManager.Instance.Shafts;
        }
    }
  
    private void OnDisable()
    {
        OnReloadManager -= RenderData;

    }
    private void OnEnable()
    {
        _prefabShaft.gameObject.SetActive(false);
       // gameObject.SetActive(true);
        OnReloadManager += RenderData;
        RenderData();
    }

    private void RenderData()
    {
        ClearChildrenExceptFirst(_parentContent);
        for (int index = 0; index < _shaftManagers.Count; index++)
        {
            var item = _shaftManagers[index];
            InformationBlockShaft prefab = Instantiate(_prefabShaft, _parentContent);
            prefab.gameObject.SetActive(true);

            prefab.SetDataInit(item); 
        }
    }
    void ClearChildrenExceptFirst(Transform parentContent)
    {
  
        if (parentContent.childCount <= 1) return;

        for (int i = parentContent.childCount - 1; i > 0; i--)
        {
         
            Transform child = parentContent.GetChild(i);
        
            GameObject.Destroy(child.gameObject);
        }
    }

    public void ScrollUp()
    {
        // Move the scroll position up
        scrollRect.verticalNormalizedPosition += scrollSpeed;

        // Clamp the position to 1 (max scroll up)
        scrollRect.verticalNormalizedPosition = Mathf.Clamp(scrollRect.verticalNormalizedPosition, 0, 1);
    }

    public void ScrollDown()
    {
        // Move the scroll position down
        scrollRect.verticalNormalizedPosition -= scrollSpeed;

        // Clamp the position to 0 (max scroll down)
        scrollRect.verticalNormalizedPosition = Mathf.Clamp(scrollRect.verticalNormalizedPosition, 0, 1);
    }

}
