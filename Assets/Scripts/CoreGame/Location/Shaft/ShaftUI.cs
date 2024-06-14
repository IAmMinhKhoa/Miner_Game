using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShaftUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_pawText;
    [SerializeField] private Button m_upgradeButton;
    [SerializeField] private Button m_buyNewShaftButton;

    private Shaft m_shaft;
    private ShaftUpgrade m_shaftUpgrade;

    void Awake()
    {
        m_shaft = GetComponent<Shaft>();
        m_shaftUpgrade = GetComponent<ShaftUpgrade>();
    }

    void Update()
    {
        m_pawText.text = Currency.DisplayCurrency(m_shaft.CurrentDeposit.CurrentPaw);
    }

    void OnEnable()
    {
        m_upgradeButton.onClick.AddListener(CallUpgrade);
        BaseUpgrade.OnUpgrade += UpdateUpgradeButton;
        m_buyNewShaftButton.onClick.AddListener(BuyNewShaft);
    }

    void OnDisable()
    {
        m_upgradeButton.onClick.RemoveListener(CallUpgrade);
        BaseUpgrade.OnUpgrade -= UpdateUpgradeButton;
        m_buyNewShaftButton.onClick.RemoveListener(BuyNewShaft);
    }

    void CallUpgrade()
    {
        m_shaftUpgrade.Upgrade(1);
    }

    void UpdateUpgradeButton(BaseUpgrade upgrade, int level)
    {
        if (upgrade == m_shaftUpgrade)
        {
            m_upgradeButton.GetComponentInChildren<TextMeshProUGUI>().text = "Level " + level;
        }
    }

    void BuyNewShaft()
    {
        if (PawManager.Instance.CurrentPaw >= ShaftManager.Instance.CurrentCost)
        {
            PawManager.Instance.RemovePaw(ShaftManager.Instance.CurrentCost);
            ShaftManager.Instance.AddShaft();
            m_buyNewShaftButton.gameObject.SetActive(false);
        }
    }
}
