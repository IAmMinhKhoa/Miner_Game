using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GachaController : MonoBehaviour
{
	[SerializeField] Transform container;
	[SerializeField] CardInformation cardInfor;
	[SerializeField] SpriteRenderer imgBehind;


	public void OpenFxGacha(Manager data)
	{
		gameObject.SetActive(true);

		container.localPosition = new Vector3(0,-1000,0);

		imgBehind.sprite=Resources.Load<Sprite>(MainGameData.PanelBehindCardManager[(int)data.BoostType]);
		cardInfor.SetData(data);
		StartCoroutine(Common.IeDoSomeThing(3f, () =>
		{
			gameObject.SetActive(false);
			container.localRotation = Quaternion.identity;
		}));

	}
}
