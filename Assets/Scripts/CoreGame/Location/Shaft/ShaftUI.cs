using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;

public class ShaftUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_pawText;
    [SerializeField] private Button m_upgradeButton;
    [SerializeField] public Button m_buyNewShaftButton;
    [SerializeField] public TextMeshProUGUI NewShaftCostText;

    [SerializeField] private TextMeshProUGUI m_levelText;
    [SerializeField] private TextMeshProUGUI m_costText;

    [SerializeField] private GameObject m_spineData;
    private SkeletonAnimation tableAnimation;

    private Shaft m_shaft;
    private ShaftUpgrade m_shaftUpgrade;

    private bool _isBrewing = false;

    void Awake()
    {
        m_shaft = GetComponent<Shaft>();
        m_shaftUpgrade = GetComponent<ShaftUpgrade>();
    }

    void Start()
    {
        tableAnimation = m_spineData.GetComponent<SkeletonAnimation>();
    }

    void Update()
    {
        m_pawText.text = Currency.DisplayCurrency(m_shaft.CurrentDeposit.CurrentPaw);
        m_levelText.text = "Level " + m_shaftUpgrade.CurrentLevel;
        m_costText.text = Currency.DisplayCurrency(m_shaftUpgrade.CurrentCost);
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
        if (PawManager.Instance.CurrentPaw >= m_shaftUpgrade.CurrentCost)
        {
            m_shaftUpgrade.Upgrade(1);
        }
    }

    void UpdateUpgradeButton(BaseUpgrade upgrade, int level)
    {
        if (upgrade == m_shaftUpgrade)
        {
            m_levelText.text = "Level " + level;
            m_costText.text = Currency.DisplayCurrency(m_shaftUpgrade.CurrentCost);
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

    public void PlayCollectAnimation(bool isBrewing)
    {
        if (_isBrewing == isBrewing)
        {
            return;
        }
        _isBrewing = isBrewing;

        if (isBrewing)
        {
            tableAnimation.AnimationState.SetAnimation(0, "Active", false);
        }
        else
        {
            tableAnimation.AnimationState.SetAnimation(0, "Idle", true);
        }
    }
}
