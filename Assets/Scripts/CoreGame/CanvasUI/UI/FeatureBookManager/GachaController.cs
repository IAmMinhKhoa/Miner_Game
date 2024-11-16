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
	[SerializeField] private GameObject FXBackCard;

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
		SetFXColor((int)data.Level);
	}
	private void OnEnable()
	{
		this.closeButton.onClick.AddListener(() =>
		{
			gameObject.SetActive(false);
			container.localRotation = Quaternion.identity;
		});
	}
	private void OnDisable()
	{
		this.closeButton.onClick.RemoveListener(() =>
		{
			gameObject.SetActive(true);
		});
	}

	private void SetFXColor(int level)
	{
		ParticleSystem fx = FXBackCard.GetComponent<ParticleSystem>();
		var mainModule = fx.main;
		switch (level)
		{
			case 0:
			case 1:
			case 2:
				mainModule.startColor = new ParticleSystem.MinMaxGradient(new Color32(255, 222, 124, 128));
				break;

			case 3:
				mainModule.startColor = new ParticleSystem.MinMaxGradient(new Color32(135, 39, 194, 128));
				break;

			case 4:
				mainModule.startColor = new ParticleSystem.MinMaxGradient(new Color32(239, 39, 39, 128));
				break;

			default:
				break;
		}
	}
}
