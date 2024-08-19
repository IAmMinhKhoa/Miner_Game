using UnityEngine;
using UnityEngine.UI;
using System;



public class ContentFitterRefresh : MonoBehaviour
{
	private void Awake()
	{
		RefreshContentFitters();
	}

	public void RefreshContentFitters()
	{
		try
		{
			var rectTransform = (RectTransform)transform;
			RefreshContentFitter(rectTransform);
		}
		catch (Exception e)
		{
			Debug.LogException(e);
		}
	}

	private void RefreshContentFitter(RectTransform transform)
	{

		if (transform == null || !transform.gameObject.activeSelf)
		{
			return;
		}

		foreach (Transform child in transform)
		{
			if (child is not RectTransform) continue;
			RefreshContentFitter((RectTransform)child);
		}

		var layoutGroup = transform.GetComponent<LayoutGroup>();
		var contentSizeFitter = transform.GetComponent<ContentSizeFitter>();
		if (layoutGroup != null)
		{
			layoutGroup.SetLayoutHorizontal();
			layoutGroup.SetLayoutVertical();
		}

		if (contentSizeFitter != null)
		{
			LayoutRebuilder.ForceRebuildLayoutImmediate(transform);
		}

	}
}
