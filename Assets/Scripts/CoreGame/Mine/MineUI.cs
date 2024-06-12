using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MineUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_goldText;

    [SerializeField]
    private Button m_upgradeButton;
    [SerializeField]
    private Button m_buyNewMineButton;

    private Mine m_mine;
    private MineUpdrage m_mineUpdrage;

    void Awake()
    {
        m_mine = GetComponent<Mine>();
        m_mineUpdrage = GetComponent<MineUpdrage>();
    }

    void Update()
    {
        m_goldText.text = Currency.DisplayCurrency(m_mine.CurrentDeposit.CurrentGold.ToString());
    }

    void OnEnable()
    {
        m_upgradeButton.onClick.AddListener(CallUpgrade);
        BaseUpdrage.OnUpgrade += UpdateUpgradeButton;
        m_buyNewMineButton.onClick.AddListener(BuyNewMine);
    }

    void OnDisable()
    {
        m_upgradeButton.onClick.RemoveListener(CallUpgrade);
        BaseUpdrage.OnUpgrade -= UpdateUpgradeButton;
        m_buyNewMineButton.onClick.RemoveListener(BuyNewMine);
    }

    void CallUpgrade()
    {
        m_mineUpdrage.Upgrade(1);
    }

    void UpdateUpgradeButton(BaseUpdrage updrage, int level)
    {
        if (updrage == m_mineUpdrage)
        {
            m_upgradeButton.GetComponentInChildren<TextMeshProUGUI>().text = "Level " + level;
        }   
    }

    void BuyNewMine()
    {
        Debug.Log("Buy new mine");
        if (GoldManager.Instance.CurrentGold >= MineManager.Instance.CurrentCost)
        {
            GoldManager.Instance.RemoveGold(MineManager.Instance.CurrentCost);
            MineManager.Instance.AddMine();
        }
    }
}
