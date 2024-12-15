using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
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
	List<StaffGachaItem> staffGachaItems;
	private void Start()
	{
		closeUI.onClick.AddListener(CloseUI);
	}

	public async UniTask InitialData(List<ShortGachaItem> shortItems, List<LongGachaItem> longGachaItems, List<StaffGachaItem> staffItem)
	{
		gameObject.SetActive(true);
		closeUI.interactable = false;
		this.shortItems = shortItems;
		this.longGachaItems = longGachaItems;
		this.staffGachaItems = staffItem;

		foreach (var item in shortItems)
		{
			item.gameObject.SetActive(true);
			PopUpAnim(item.gameObject);
			item.gameObject.transform.SetParent(shortContent.transform, false);
			await UniTask.Delay(300);
		}
		foreach (var item in staffItem)
		{
			item.gameObject.SetActive(true);
			PopUpAnim(item.gameObject);
			item.gameObject.transform.SetParent(shortContent.transform, false);
			await UniTask.Delay(300);
		}
		foreach (var item in longGachaItems)
		{
			item.gameObject.SetActive(true);
			PopUpAnim(item.gameObject);
			item.gameObject.transform.SetParent(longContent.transform, false);
			await UniTask.Delay(300);
		}
		closeUI.interactable = true;
	}
	public void CloseUI()
	{
		gameObject.SetActive(false);
		
		foreach (var item in shortItems)
		{
			Destroy(item.gameObject);
		}
		foreach (var item in staffGachaItems)
		{
			Destroy(item.gameObject);
		}
		foreach (var item in longGachaItems)
		{
			Destroy(item.gameObject);
		}
	}

	void PopUpAnim(GameObject item)
	{
		RectTransform _rectTransform = item.GetComponent<RectTransform>();
		_rectTransform.DOScale(0, 0);
		_rectTransform.DOScale(1.5f, 0.3f)
			.SetEase(Ease.OutQuad)
			.OnComplete(() =>
			{
				_rectTransform.DOScale(1, 0.2f).SetEase(Ease.InQuad);
			});
	}
}
