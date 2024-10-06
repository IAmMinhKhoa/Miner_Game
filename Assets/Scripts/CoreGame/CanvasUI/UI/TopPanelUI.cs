using UnityEngine;

public class TopPanelUI : MonoBehaviour
{
	[Header("Support")]
	[SerializeField] private BankUI _bankUI;

	[Header("Button")]
	[SerializeField] private ButtonBehavior _btnAddHeart;
	[SerializeField] private ButtonBehavior _btnAddCoin;

	private void Awake()
	{
		_btnAddCoin.onClickEvent.AddListener(_bankUI.FadeInContainer);
		_btnAddHeart.onClickEvent.AddListener(_bankUI.FadeInContainer);
	}

	void OnDestroy()
	{
		_btnAddCoin.onClickEvent.RemoveListener(_bankUI.FadeInContainer);
		_btnAddHeart.onClickEvent.RemoveListener(_bankUI.FadeInContainer);
	}
}
