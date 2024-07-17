using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour
{
    [SerializeField] private Button closeButton;
    [SerializeField] private Button upgradeButton;

    private void OnEnable()
    {
        closeButton.onClick.AddListener(ClosePanel);
        upgradeButton.onClick.AddListener(Upgrade);
    }

    private void OnDisable()
    {
        closeButton.onClick.RemoveListener(ClosePanel);
        upgradeButton.onClick.RemoveListener(Upgrade);
    }

    private void ClosePanel()
    {
        gameObject.SetActive(false);
    }

    private void Upgrade()
    {
        UpgradeManager.OnUpdrageRequest?.Invoke(1);
    }

}
