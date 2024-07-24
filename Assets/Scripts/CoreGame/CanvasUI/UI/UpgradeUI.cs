using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeUI : MonoBehaviour
{
    [Header("Buttons UI")]
    [SerializeField] private Button closeButton;
    [SerializeField] private Button upgradeButton;

    [Header("Slider UI")]
    [SerializeField] private Slider upgradeSlider;

    [Header("Text UI")]
    [SerializeField] private TextMeshProUGUI upgradeAmountText;
    [SerializeField] private TextMeshProUGUI upgradeCostText;

    private void OnEnable()
    {
        closeButton.onClick.AddListener(ClosePanel);
        upgradeButton.onClick.AddListener(Upgrade);
        upgradeSlider.onValueChanged.AddListener(UpdateUpgradeAmount);
    }

    private void OnDisable()
    {
        closeButton.onClick.RemoveListener(ClosePanel);
        upgradeButton.onClick.RemoveListener(Upgrade);
        upgradeSlider.onValueChanged.RemoveListener(UpdateUpgradeAmount);
    }

    private void ClosePanel()
    {
        gameObject.SetActive(false);
    }

    private void Upgrade()
    {
        int upgradeAmount = (int)upgradeSlider.value;
        UpgradeManager.Instance.OnUpdrageRequest?.Invoke(upgradeAmount);
    }

    private void UpdateUpgradeAmount(float value)
    {
        upgradeAmountText.text = value.ToString();
        double cost = UpgradeManager.Instance.GetUpgradeCost((int)value);
        upgradeCostText.text = Currency.DisplayCurrency(cost);
    }

    public void SetUpPanel(int max)
    {
        upgradeSlider.maxValue = Mathf.Max(max, 1);
        upgradeSlider.value = 1;
        upgradeAmountText.text = "1";
        upgradeCostText.text = Currency.DisplayCurrency(UpgradeManager.Instance.GetUpgradeCost((int)upgradeSlider.value));
        Debug.Log("Max value:" + max + " Upgrade cost:" + UpgradeManager.Instance.GetUpgradeCost((int)upgradeSlider.value) + " Upgrade initial cost:" + UpgradeManager.Instance.GetInitCost());
    }
}
