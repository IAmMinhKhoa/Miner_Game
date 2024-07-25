using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelPanelUI : MonoBehaviour
{
    [SerializeField] private Image imageUpgarde;
    [SerializeField] private BaseUpgrade baseUpgrade;

    void OnEnable()
    {
        PawManager.Instance.OnPawChanged += OnPawChanged;
    }

    void OnDisable()
    {
        PawManager.Instance.OnPawChanged -= OnPawChanged;
    }

    private void OnPawChanged(double paw)
    {
        bool isActive = paw >= baseUpgrade.CurrentCost;
        imageUpgarde.gameObject.SetActive(isActive);

        if (isActive)
        {
            int amount = UpgradeManager.Instance.CalculateUpgradeAmount(paw, baseUpgrade);

            if (amount < 51 && amount > 0)
            {
                imageUpgarde.sprite = Resources.Load<Sprite>(MainGameData.CanUpgradeButton[0]);
            }
            else if (amount < 101)
            {
                imageUpgarde.sprite = Resources.Load<Sprite>(MainGameData.CanUpgradeButton[1]);
            }
            else
            {
                imageUpgarde.sprite = Resources.Load<Sprite>(MainGameData.CanUpgradeButton[2]);
            }
        }
    }
}
