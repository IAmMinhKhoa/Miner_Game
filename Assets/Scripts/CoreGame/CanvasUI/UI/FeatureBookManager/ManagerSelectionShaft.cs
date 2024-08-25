using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class ManagerSelectionShaft : MonoBehaviour
{
    public static Action OnReloadManager;
    [SerializeField] InformationBlockShaft _prefabShaft;
    [SerializeField] Transform _parentContent;
    [SerializeField] Button _btnTop;
    [SerializeField] Button _btnBottom;
	private List<GameObject> _shafts=new List<GameObject>();

    [SerializeField] private ScrollRect scrollRect;   // Reference to the Scroll View's ScrollRect component
    [SerializeField] private float scrollSpeed = 0.1f; // Speed at which the scroll happens
	[SerializeField] CanvasGroup canvasGroupContent;
	public float value1 = -250f;
	public float value2 = 0f;

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
        _btnTop.onClick.RemoveAllListeners();
        _btnBottom.onClick.RemoveAllListeners();
    }
    private void OnEnable()
    {
        _prefabShaft.gameObject.SetActive(false);
        OnReloadManager += RenderData;
        RenderData();

        _btnTop.onClick.AddListener(ScrollUp);
        _btnBottom.onClick.AddListener(ScrollDown);


		RectTransform contentRect = scrollRect.content;
		contentRect.anchoredPosition = new Vector2(contentRect.anchoredPosition.x, 0f);



		canvasGroupContent.alpha = 0;
		testAnimateSlide();
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
			_shafts.Add(prefab.gameObject);

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
		_shafts.Clear();

	   RectTransform contentRect = scrollRect.content;
        RectTransform viewportRect = scrollRect.viewport;

        // Ki?m tra xem n?i dung có l?n h?n viewport không (?? cu?n d?c)
        if (contentRect.rect.height > viewportRect.rect.height)
        {
            Debug.Log("Reached the bottom of the content.");
            _btnTop.gameObject.SetActive(true);
            _btnBottom.gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("NOT REACHED the bottom of the content.");
            _btnTop.gameObject.SetActive(false);
            _btnBottom.gameObject.SetActive(false);
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
        scrollRect.verticalNormalizedPosition = Mathf.Clamp(scrollRect.verticalNormalizedPosition, 0,1);
    }
	[Button]
	public void testAnimateSlide()
	{
		StartCoroutine(animateSlide());	
	}
	[Button]
	public void resetPositionOfPrefab()
	{
		foreach (var item in _shafts)
		{
			Vector3 newPosition = item.transform.localPosition;
			newPosition.x = value1;
			item.transform.localPosition = newPosition;
		}
	}
	public IEnumerator animateSlide(bool isOpen=true)
	{
		
		yield return new WaitForSeconds(0.1f);
		canvasGroupContent.alpha = 1f;
		resetPositionOfPrefab();
		foreach (var prefab in _shafts)
		{
			prefab.transform.DOMoveX(value2, 0.15f).SetEase(Ease.OutQuad);
			yield return new WaitForSeconds(0.06f);
		}

	}
}
