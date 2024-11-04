using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemBoxsGacha : MonoBehaviour
{
	[SerializeField]
	GameObject shortContent;
	[SerializeField]
	GameObject longContent;
	[SerializeField]
	Button closeUI;
	List<ShortGachaItem> shortItems;
	List<LongGachaItem> longGachaItems;

	private void Start()
	{
		closeUI.onClick.AddListener(CloseUI);
	}
	public void InitialData(List<ShortGachaItem> shortItems, List<LongGachaItem> longGachaItems)
	{
		gameObject.SetActive(true);
		this.shortItems = shortItems;
		this.longGachaItems = longGachaItems;
		foreach (var item in shortItems)
		{
			item.gameObject.transform.SetParent(shortContent.transform, false);
		}
		foreach (var item in longGachaItems)
		{
			item.gameObject.transform.SetParent(longContent.transform, false);
		}
	}
	public void CloseUI()
	{
		gameObject.SetActive(false);
		
		foreach (var item in shortItems)
		{
			Destroy(item.gameObject);
		}
		foreach (var item in longGachaItems)
		{
			Destroy(item.gameObject);
		}
	}
}
