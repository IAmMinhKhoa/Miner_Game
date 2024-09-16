using DG.Tweening;
using PlayFab.EconomyModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;
using static UI.Inventory.ItemInventoryUI;

namespace UI.Inventory
{
    public class InventoryUIManager : MonoBehaviour
	{
		[Header("UI")]
        [SerializeField] private Toggle tgNoiThat;
        [SerializeField] private Toggle tgNhanVien;
        [SerializeField] private GameObject pnNoiThat;
        [SerializeField] private GameObject pnNhanVien;
        [SerializeField] GameObject inventoryPanel;
		[SerializeField] BackGroundItemController bgList;

		[Header("Item Handle")]
		[SerializeField]
		GameObject shaftContent;
		[SerializeField]
		ShaftUIController shaftUIController;
		List<ShaftUIController> listShaftUI;


		[Header("BackGroundItem")]
		[SerializeField] DecoratorItem counterSkin;
		[SerializeField] DecoratorItem elevatorSkin;

		int shaftCount = 0;
		bool isBackgroundItemOpening;
        void Start()
        {
			listShaftUI = new();
			isBackgroundItemOpening = false;
            tgNhanVien.onValueChanged.AddListener(delegate
            {
                SlideInContainer(pnNhanVien, tgNhanVien);
            });
            tgNoiThat.onValueChanged.AddListener(delegate
            {
                SlideInContainer(pnNoiThat, tgNoiThat);
            });
			ShaftManager.Instance.OnShaftUpdated += HandleShaftUI;
			counterSkin.OnItemClick += OpenListBg;
			elevatorSkin.OnItemClick += OpenListBg;
			ShaftManager.Instance.OnUpdateShaftUI += HanleUpdateShaftUI;
		}

		private void HanleUpdateShaftUI(int index)
		{
			listShaftUI[index].UpdateShaftUI();
		}

		public void SlideInContainer(GameObject panel, Toggle tg)
        {
            if (tg.isOn) tg.gameObject.GetComponent<ToggleBehaviour>().DoAnimate();
            panel.SetActive(tg.isOn);
            Vector2 posCam = CustomCamera.Instance.GetCurrentTransform().position;
            panel.transform.localPosition = new Vector2(posCam.x - 2000, panel.transform.localPosition.y);
            panel.transform.DOLocalMoveX(0, 0.6f).SetEase(Ease.OutElastic, 1, 1f);
        }
        public void CloseInvetoryUI()
        {
            if (isBackgroundItemOpening == false)
                gameObject.SetActive(false);
			if(isBackgroundItemOpening)
			{
				isBackgroundItemOpening = false;
				bgList.gameObject.SetActive(false);
				inventoryPanel.SetActive(true);
			}
        }
		private void OpenListBg(InventoryItemType type, int index = -1)
		{
			isBackgroundItemOpening = true;
			bgList.gameObject.SetActive(true);
			bgList.SetItemHandle(type, index);
			inventoryPanel.SetActive(false);

		}
		private void HandleShaftUI()
		{
			var shaft = Instantiate(shaftUIController, Vector3.zero, Quaternion.identity);
			shaft.transform.SetParent(shaftContent.transform, false);
			shaft.SetShaftIndex(shaftCount);
			shaft.items[0].OnItemClick += OpenListBg;
			shaftCount++;
			listShaftUI.Add(shaft);
		}
	}
}
