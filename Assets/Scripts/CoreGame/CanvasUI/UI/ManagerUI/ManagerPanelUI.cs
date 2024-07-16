using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManagerPanelUI : MonoBehaviour
{
    [SerializeField] private Button _closeButton;
    [SerializeField] private Button _hireOrFiredButton;

    [SerializeField] private Manager _manager;

    void OnEnable()
    {
        _closeButton.onClick.AddListener(ClosePanel);
        _hireOrFiredButton.onClick.AddListener(HireOrFireManager);
    }

    void OnDisable()
    {
        _closeButton.onClick.RemoveListener(ClosePanel);
        _hireOrFiredButton.onClick.RemoveListener(HireOrFireManager);
    }

    private void ClosePanel()
    {
        gameObject.SetActive(false);
    }

    private void HireOrFireManager()
    {
        if (_manager.IsAssigned && _manager.Location == ManagersController.Instance.CurrentManagerLocation)
        {
            _manager.UnassignManager();
            ClosePanel();
            return;
        }
        else
        {
            _manager.AssignManager();
            ClosePanel();
        }
    }

    public void SetManager(Manager manager)
    {
        _manager = manager;
    }
}
