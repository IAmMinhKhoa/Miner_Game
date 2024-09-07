using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIManager : MonoBehaviour
{
	[SerializeField] private Toggle tgNoiThat;
	[SerializeField] private Toggle tgNhanVien;
	[SerializeField] private GameObject pnNoiThat;
	[SerializeField]private GameObject pnNhanVien;
	void Awake()
    {
		tgNhanVien.onValueChanged.AddListener(delegate {
			SlideInContainer(pnNhanVien, tgNhanVien);
		});
		tgNoiThat.onValueChanged.AddListener(delegate {
			SlideInContainer(pnNoiThat, tgNoiThat);
		});
	}




	public void SlideInContainer(GameObject panel, Toggle tg)
	{
		if(tg.isOn) tg.gameObject.GetComponent<ToggleBehaviour>().DoAnimate();
		panel.SetActive(tg.isOn);
		Vector2 posCam = CustomCamera.Instance.GetCurrentTransform().position;
		panel.transform.localPosition = new Vector2(posCam.x - 2000, panel.transform.localPosition.y);
		panel.transform.DOLocalMoveX(0, 0.6f).SetEase(Ease.OutElastic, 1, 1f);
		
	}
}
