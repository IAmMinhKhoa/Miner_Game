using UnityEngine;
using TMPro;

public class DropdownDirectionHandler : MonoBehaviour
{
	public TMP_Dropdown dropdown;

	private void Start()
	{
		// Gắn sự kiện khi Dropdown được mở
		dropdown.onValueChanged.AddListener((index) => AdjustDropdownDirection());
	}

	private void AdjustDropdownDirection()
	{
		// Tìm GameObject tạm của "Dropdown List"
		Transform dropdownList = dropdown.transform.parent.Find("Dropdown List");
		if (dropdownList != null)
		{
			RectTransform dropdownRect = dropdownList.GetComponent<RectTransform>();

			// Đặt hướng sổ lên
			dropdownRect.pivot = new Vector2(0.5f, 0f); // Điểm gốc dưới giữa
			dropdownRect.anchoredPosition = new Vector2(0, dropdownRect.sizeDelta.y); // Đẩy lên trên
		}
	}
}
