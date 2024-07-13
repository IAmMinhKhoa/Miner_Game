using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManagerPanelUI : MonoBehaviour
{
    [SerializeField] private Button _closeButton;

    void OnEnable()
    {
        _closeButton.onClick.AddListener(ClosePanel);
    }

    void OnDisable()
    {
        _closeButton.onClick.RemoveListener(ClosePanel);
    }

    private void ClosePanel()
    {
        gameObject.SetActive(false);
    }
}
