using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemInventoryUI : MonoBehaviour, IPointerClickHandler
{
	[SerializeField]
	private Image image;
	[SerializeField]
	private TextMeshProUGUI nameItem;
	public void ChangeItem(Sprite sprite, string  name)
	{
		this.image.sprite = sprite;
		this.nameItem.text = name;
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		var dic = ShaftManager.Instance.Shafts[0].shaftSkin.GetDataSkin();
		Debug.Log(dic["skinBgShaft"].path);
		image.sprite = Resources.Load<Sprite>(dic["skinBgShaft"].path);
	}
}
