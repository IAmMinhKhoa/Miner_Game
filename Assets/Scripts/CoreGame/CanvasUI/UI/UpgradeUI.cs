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
    [SerializeField] private TextMeshProUGUI titleText;

    [Header("Worker Info")]
    [SerializeField] private TextMeshProUGUI workerName;
    [SerializeField] private TextMeshProUGUI workerProduction;
    [SerializeField] private TextMeshProUGUI numberOrSpeed;
    [SerializeField] private TextMeshProUGUI totalProduction;

    [Header("Worker Info Name")]
    [SerializeField] private TextMeshProUGUI s_workerProduction;
    [SerializeField] private TextMeshProUGUI s_numberOrSpeed;
    [SerializeField] private TextMeshProUGUI s_totalProduction;

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
        UpgradeManager.Instance.OnUpgradeRequest?.Invoke(upgradeAmount);
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

    public void SetWorkerInfo(ManagerLocation locationType, string name, double production, string number, double total, int level)
    {
        switch (locationType)
        {
            case ManagerLocation.Shaft:
                titleText.text = MainGameData.UpgradeDetailInfo[ManagerLocation.Shaft][0] + level.ToString();
                s_workerProduction.text = MainGameData.UpgradeDetailInfo[ManagerLocation.Shaft][1];
                s_numberOrSpeed.text = MainGameData.UpgradeDetailInfo[ManagerLocation.Shaft][2];
                s_totalProduction.text = MainGameData.UpgradeDetailInfo[ManagerLocation.Shaft][3];
                
                numberOrSpeed.text = number;
                break;
            case ManagerLocation.Elevator:
                titleText.text = MainGameData.UpgradeDetailInfo[ManagerLocation.Elevator][0] + level.ToString();
                s_workerProduction.text = MainGameData.UpgradeDetailInfo[ManagerLocation.Elevator][1];
                s_numberOrSpeed.text = MainGameData.UpgradeDetailInfo[ManagerLocation.Elevator][2];
                s_totalProduction.text = MainGameData.UpgradeDetailInfo[ManagerLocation.Elevator][3];

                numberOrSpeed.text = number + " s";
                break;
            case ManagerLocation.Counter:
                titleText.text = MainGameData.UpgradeDetailInfo[ManagerLocation.Counter][0] + level.ToString();
                s_workerProduction.text = MainGameData.UpgradeDetailInfo[ManagerLocation.Counter][1];
                s_numberOrSpeed.text = MainGameData.UpgradeDetailInfo[ManagerLocation.Counter][2];
                s_totalProduction.text = MainGameData.UpgradeDetailInfo[ManagerLocation.Counter][3];

                numberOrSpeed.text = number;
                break;
        }

        workerName.text = name;
        workerProduction.text = Currency.DisplayCurrency(production) + "/s";
        totalProduction.text = Currency.DisplayCurrency(total);
    }
}
