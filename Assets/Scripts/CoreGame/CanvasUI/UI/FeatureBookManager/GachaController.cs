using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GachaController : MonoBehaviour
{
	[SerializeField] Transform container;
	[SerializeField] CardInformation cardInfor;
	[SerializeField] SpriteRenderer imgBehind;
	[SerializeField] private Button closeButton;

	public void OpenFxGacha(Manager data)
	{
		gameObject.SetActive(true);

		container.localPosition = new Vector3(0,-1000,0);

		imgBehind.sprite=Resources.Load<Sprite>(MainGameData.PanelBehindCardManager[(int)data.BoostType]);
		cardInfor.SetData(data);
		//StartCoroutine(Common.IeDoSomeThing(3f, () =>
		//{
		//	gameObject.SetActive(false);
		//	container.localRotation = Quaternion.identity;
		//}));

	}
	private void OnEnable()
	{
		this.closeButton.onClick.AddListener(() =>
		{
			gameObject.SetActive(false);
		});
	}
	private void OnDisable()
	{
		this.closeButton.onClick.RemoveListener(() =>
		{
			gameObject.SetActive(true);
		});
	}
}
