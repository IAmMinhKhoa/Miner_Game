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
    [SerializeField] private Button _sellButton;

    [Header("UI Text")]
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _textTimeSkill;
    [SerializeField] private TextMeshProUGUI _textTimeCD;
    [SerializeField] private TextMeshProUGUI _textValueBuff;
    [SerializeField] private TextMeshProUGUI _textQuoest;


    [Header("UI Image")]
    [SerializeField] Image _backGroundPanel;
    [SerializeField] Image _bannerName  ;


    [SerializeField] private SkeletonGraphic _spineManager;
    [SerializeField] List<Sprite> _imgStrokeLevels=new List<Sprite>();
    [SerializeField] List<Sprite> _imgBannerNameLevels = new List<Sprite>();
    [SerializeField] List<Sprite> _imgBtnHireFire = new List<Sprite>();
    [SerializeField] List<Image> _starts =new List<Image>();
    [SerializeField] List<Sprite> _stateStart = new List<Sprite>(); //0 active, 1 unActive

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
        _manager = manager;
        _nameText.text = _manager.Specie.ToString();


        ValidateData(manager);


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
            _hireOrFiredButton.GetComponent<Image>().sprite = _imgBtnHireFire[0];
        }
        else
        {
            _hireOrFiredButton.GetComponent<Image>().sprite = _imgBtnHireFire[1];
        }

		refeshInforSize.RefreshContentFitters();
	}
    private void ValidateData(Manager _data)
    {
        /* _imgNumberIcon.sprite = Resources.Load<Sprite>(MainGameData.IconLevelNumber[(int)_data.Level]);
         _imgFrame.sprite = Resources.Load<Sprite>(MainGameData.FrameLevelAvatar[(int)_data.Level]);*/
        // _icon.sprite = (int)_data.Level == 4 ? _data.IconSpecial : _data.Icon;
        //  _imgStroke.sprite = _imgStrokeLevels[(int)_data.Level];

        RenderStart((int)_data.Level);
        _backGroundPanel.sprite = _imgStrokeLevels[(int)_data.Level];
        _bannerName.sprite = _imgBannerNameLevels[(int)_data.Level];
        _spineManager.skeletonDataAsset = _data.SkeletonAsset;
        _spineManager.Initialize(true);

        //set data description
        _textTimeSkill.text = _data.BoostTime.ToString() +" phút";
        _textTimeCD.text = _data.CooldownTime.ToString() + " phút";
        _textValueBuff.text = _data.BoostValue.ToString()+" %";

        _textQuoest.text = _data.Quoest;


	}

    private void RenderStart(int Currentlevel)
    {
        foreach (var item in _starts)
        {
            item.sprite = _stateStart[1];
        }
        for (int i = 0; i <= Currentlevel; i++)
        {
            _starts[i].sprite = _stateStart[0];
        }
    }
}
