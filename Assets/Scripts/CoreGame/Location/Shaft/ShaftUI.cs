using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;
using Cysharp.Threading.Tasks;
using System;

public class ShaftUI : MonoBehaviour
{
    public static Action<int> OnUpgradeRequest;

    [Header("UI Button")]
    [SerializeField] private Button m_upgradeButton;
    [SerializeField] public Button m_buyNewShaftButton;
    [SerializeField] private Button m_managerButton;
    [SerializeField] private Button m_boostButton;

    [Header("UI Text")]
    [SerializeField] private TextMeshProUGUI m_pawText;
    [SerializeField] public TextMeshProUGUI NewShaftCostText;
    [SerializeField] private TextMeshProUGUI m_levelText;
    [SerializeField] private TextMeshProUGUI m_costText;

    [SerializeField] private GameObject m_spineData;
    [SerializeField] private GameObject mainPanel;
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
        mainPanel.transform.SetParent(GameWorldUI.Instance.transform, true);
    }

    void Update()
    {
        m_pawText.text = Currency.DisplayCurrency(m_shaft.CurrentDeposit.CurrentPaw);
        m_levelText.text = "Level " + m_shaftUpgrade.CurrentLevel;
        m_costText.text = Currency.DisplayCurrency(m_shaftUpgrade.CurrentCost);
    }

    void OnEnable()
    {
        m_upgradeButton.onClick.AddListener(UpgradeRequest);
        BaseUpgrade.OnUpgrade += UpdateUpgradeButton;
        m_buyNewShaftButton.onClick.AddListener(BuyNewShaft);
        m_managerButton.onClick.AddListener(OpenManagerPanel);
        m_boostButton.onClick.AddListener(ActiveBoost);
    }

    void OnDisable()
    {
        m_upgradeButton.onClick.RemoveListener(UpgradeRequest);
        BaseUpgrade.OnUpgrade -= UpdateUpgradeButton;
        m_buyNewShaftButton.onClick.RemoveListener(BuyNewShaft);
        m_managerButton.onClick.RemoveListener(OpenManagerPanel);
        m_boostButton.onClick.RemoveListener(ActiveBoost);
    }

    void OpenManagerPanel()
    {
        Debug.Log("Open Manager Panel");
        ManagersController.Instance.OpenManagerPanel(m_shaft.ManagerLocation);
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

    public async void PlayCollectAnimation(bool isBrewing)
    {
        if (isBrewing == false) return;
        if (_isBrewing == true)
        {
            return;
        }
        _isBrewing = true;

        //Debug.Log("Play Active");
        tableAnimation.AnimationState.SetAnimation(0, "Active", false);
        await UniTask.Delay((int)tableAnimation.skeletonDataAsset.GetAnimationStateData().SkeletonData.FindAnimation("Active").Duration * 1000);
        //Debug.Log("Play Idle");
        tableAnimation.AnimationState.SetAnimation(0, "Idle", true);
        _isBrewing = false;
    }

    public void UpgradeRequest()
    {
        OnUpgradeRequest?.Invoke(m_shaft.shaftIndex);
    }

    public void ActiveBoost()
    {
        if (m_shaft.ManagerLocation.Manager != null)
        {
            m_shaft.ManagerLocation.RunBoost();
        }
    }

    void OnDestroy()
    {
        Destroy(mainPanel);
    }
}
