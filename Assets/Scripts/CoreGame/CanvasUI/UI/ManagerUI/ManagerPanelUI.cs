using System;
using System.Collections.Generic;
using NOOD.Sound;
using Spine.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ManagerPanelUI : MonoBehaviour
{
    [Header("UI Button")]
    [SerializeField] private Button _closeButton;
    [SerializeField] private Button _hireOrFiredButton;
	[SerializeField] private TMP_Text hireOrRest_lb;
    [SerializeField] private Button _sellButton;

	[Header("UI Anothers")]
	[SerializeField] List<Sprite> _imgBtnHireFire = new List<Sprite>();
	[SerializeField] CardInformation _cardInfor;

	[SerializeField] private Manager _manager;
	[SerializeField] ContentFitterRefresh refeshInforSize;

	public event Action OnInteractToTutorialUI;
	public Button HireOrFiredButton => _hireOrFiredButton;

	void OnEnable()
    {
        _closeButton.onClick.AddListener(ClosePanel);
        _hireOrFiredButton.onClick.AddListener(HireOrFireManager);
        _sellButton.onClick.AddListener(SellManager);
    }

    void OnDisable()
    {
        _closeButton.onClick.RemoveListener(ClosePanel);
        _hireOrFiredButton.onClick.RemoveListener(HireOrFireManager);
        _sellButton.onClick.RemoveListener(SellManager);
    }

    private void ClosePanel()
    {
		SoundManager.PlaySound(SoundEnum.mobileTexting2);
		gameObject.SetActive(false);
		if (TutorialManager.Instance.isTuroialing)
		{
			OnInteractToTutorialUI?.Invoke();
		}
	}

    private void HireOrFireManager()
    {
        var  managersController = ManagersController.Instance;

        if (_manager.IsAssigned)
        {
            managersController.UnassignManager(_manager);
        }
        else
        {
            managersController.AssignManager(_manager, managersController.CurrentManagerLocation);
        }

		ManagerSelectionShaft.OnReloadManager?.Invoke();
        ClosePanel();
    }
	public void HireManager()
	{
		HireOrFireManager();
	}
    private void SellManager()
    {
        ManagersController.Instance.SellManager(_manager);
        ManagerSelectionShaft.OnReloadManager?.Invoke();
        ClosePanel();
    }

    public void SetManager(Manager manager)
    {
		string titlekey = string.Empty;
        _manager = manager;



		//ValidateData(manager);
		_cardInfor.SetData(manager);


		if (ManagersController.Instance.CurrentManagerLocation.LocationType == ManagerLocation.Shaft)
        {
            _hireOrFiredButton.gameObject.SetActive(false);
        }
        else
        {
            _hireOrFiredButton.gameObject.SetActive(true);
        }

        if (_manager.IsAssigned)
        {
			titlekey = LocalizationManager.GetLocalizedString(LanguageKeys.TitleManagerChooseUIRest);
			_hireOrFiredButton.GetComponent<Image>().sprite = _imgBtnHireFire[0];
		}
        else
        {
			titlekey = LocalizationManager.GetLocalizedString(LanguageKeys.TitleManagerChooseUIHire);
			_hireOrFiredButton.GetComponent<Image>().sprite = _imgBtnHireFire[1];
        }
		hireOrRest_lb.text = titlekey;
		refeshInforSize.RefreshContentFitters();
	}

    #region BLock Button Sell

    public void StateButton(bool isLock)
    {
	    if (isLock)
	    {
		    _sellButton.interactable = false;
	    }
	    else
	    {
		    _sellButton.interactable = true;
	    }
    }


    #endregion
}
