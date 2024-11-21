using System.Collections.Generic;
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
        gameObject.SetActive(false);
    }

    private void HireOrFireManager()
    {
        var  managersController = ManagersController.Instance;

        if (_manager.IsAssigned)
        {
            managersController.UnassignManager(_manager);
			Debug.Log("-99999");
        }
        else
        {
            managersController.AssignManager(_manager, managersController.CurrentManagerLocation);
        }
	
		ManagerSelectionShaft.OnReloadManager?.Invoke();
        ClosePanel();
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
 
}
