using System.Collections.Generic;
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


    [Header("UI Image")]
    [SerializeField] private Image _imgNumberIcon;
    [SerializeField] private Image _imgFrame;
    [SerializeField] private Image _imgStroke;
    [SerializeField] private Image _icon;
    [SerializeField] List<Sprite> _imgStrokeLevels=new List<Sprite>();
    [SerializeField] List<Sprite> _imgBtnHireFire = new List<Sprite>();

    [SerializeField] private Manager _manager;

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

    void Update()
    {
      
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
    }
    private void ValidateData(Manager _data)
    {
        _imgNumberIcon.sprite = Resources.Load<Sprite>(MainGameData.IconLevelNumber[(int)_data.Level]);
        _imgFrame.sprite = Resources.Load<Sprite>(MainGameData.FrameLevelAvatar[(int)_data.Level]);
        _icon.sprite = (int)_data.Level == 4 ? _data.IconSpecial : _data.Icon;
        _imgStroke.sprite = _imgStrokeLevels[(int)_data.Level];

        //set data description
        _textTimeSkill.text = _data.BoostTime.ToString() +" Phút";
        _textTimeCD.text = _data.CooldownTime.ToString() + " Phút";
        _textValueBuff.text = _data.BoostValue.ToString()+" %";
    }
}
