using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ManagerPanelUI : MonoBehaviour
{
    [Header("UI Button")]
    [SerializeField] private Button _closeButton;
    [SerializeField] private Button _hireOrFiredButton;
    [SerializeField] private Button _sellButton;

    [Header("UI Text")]
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private TextMeshProUGUI _descriptionText;
    [SerializeField] private TextMeshProUGUI _cooldownTimeText;

    [Header("UI Image")]
    [SerializeField] private Image _infoIcon;

    [Header("UI Slider")]
    [SerializeField] private Slider _cooldownSlider;

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
        // if (_manager.IsAssigned && _manager.Location == ManagersController.Instance.CurrentManagerLocation)
        // {
        //     _manager.UnassignManager();
        //     ClosePanel();
        //     return;
        // }
        // else
        // {
        //     _manager.AssignManager();
        //     ClosePanel();
        // }

        var  managersController = ManagersController.Instance;

        if (_manager.IsAssigned)
        {
            managersController.UnassignManager(_manager);
        }
        else
        {
            managersController.AssignManager(_manager, managersController.CurrentManagerLocation);
        }

        ClosePanel();
    }

    private void SellManager()
    {
        ManagersController.Instance.SellManager(_manager);
        ClosePanel();
    }

    void Update()
    {
        if (_manager == null)
        {
            return;
        }
        _cooldownSlider.value = _manager.CurrentCooldownTime / _manager.CooldownTime;
    }

    public void SetManager(Manager manager)
    {
        _manager = manager;
        _nameText.text = _manager.Specie.ToString();
        _levelText.text = _manager.Level.ToString();
        _descriptionText.text = "Nothing here yet";
        _infoIcon.sprite = _manager.Icon;
        _cooldownTimeText.text = _manager.CooldownTime + "m";

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
            _hireOrFiredButton.GetComponentInChildren<TextMeshProUGUI>().text = "Nghỉ";
        }
        else
        {
            _hireOrFiredButton.GetComponentInChildren<TextMeshProUGUI>().text = "Chọn";
        }
    }
}
