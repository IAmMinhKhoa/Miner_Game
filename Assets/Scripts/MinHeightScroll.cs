using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class MinHeightScroll : MonoBehaviour
{
	[SerializeField]
	private float minHeight = 250f; // Chiều cao tối thiểu, có thể điều chỉnh trong Inspector

	void Start()
	{
		validateUI();
	}
	[Button]
	private void validateUI()
	{
		// Lấy RectTransform của đối tượng này
		RectTransform rectTransform = GetComponent<RectTransform>();

		// Kiểm tra nếu rectTransform tồn tại
		if (rectTransform != null)
		{
			// Kiểm tra chiều cao và điều chỉnh nếu nhỏ hơn minHeight
			if (rectTransform.rect.height < minHeight)
			{
				Vector2 sizeDelta = rectTransform.sizeDelta;
				sizeDelta.y += minHeight - rectTransform.rect.height;
				rectTransform.sizeDelta = sizeDelta;
			}
		}
		else
		{
			Debug.LogWarning("RectTransform không được tìm thấy trên GameObject này.");
		}
	}
}
