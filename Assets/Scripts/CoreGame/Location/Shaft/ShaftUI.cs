using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;
using Cysharp.Threading.Tasks;
using System;
using Sirenix.OdinInspector;

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

    [Header("Visual object")]
    [SerializeField] private GameObject m_spineData;
    [SerializeField] private GameObject m_lyNuocHolder;
    [SerializeField] private GameObject mainPanel;
    

    private SkeletonAnimation tableAnimation;
    private ShaftUpgrade m_shaftUpgrade;
    private Shaft m_shaft;

    private bool _isBrewing = false;


    #region TOOL DEBUG
    #endregion

    void Awake()
    {
        m_shaft = GetComponent<Shaft>();
        m_shaftUpgrade = GetComponent<ShaftUpgrade>();
        m_lyNuocHolder.gameObject.SetActive(false);
    }

    void Start()
    {
        m_shaft.CurrentDeposit.OnChangePaw += ChangePawHandler;
        
        tableAnimation = m_spineData.GetComponent<SkeletonAnimation>();
        mainPanel.transform.SetParent(GameWorldUI.Instance.transform, true);


        //First init Data frame by current lvl of shaft
        UpdateFrameButtonUpgrade(m_shaftUpgrade.CurrentLevel);



       
    }
    private void checklevel(int currentLvl)
    {

    }
    void Update()
    {
        m_pawText.text = Currency.DisplayCurrency(m_shaft.CurrentDeposit.CurrentPaw);
        m_levelText.text =m_shaftUpgrade.CurrentLevel.ToString();
        m_costText.text = Currency.DisplayCurrency(m_shaftUpgrade.CurrentCost);
    }

    void OnEnable()
    {
        BaseUpgrade.OnUpgrade += UpdateUpgradeButton;
        m_upgradeButton.onClick.AddListener(UpgradeRequest);
        m_buyNewShaftButton.onClick.AddListener(BuyNewShaft);
        m_managerButton.onClick.AddListener(OpenManagerPanel);
        m_boostButton.onClick.AddListener(ActiveBoost);
        Debug.Log("khoa:" + m_shaft.CurrentDeposit);
    }

    void OnDisable()
    {
        BaseUpgrade.OnUpgrade -= UpdateUpgradeButton;
        m_upgradeButton.onClick.RemoveListener(UpgradeRequest);
        m_buyNewShaftButton.onClick.RemoveListener(BuyNewShaft);
        m_managerButton.onClick.RemoveListener(OpenManagerPanel);
        m_boostButton.onClick.RemoveListener(ActiveBoost);
       // m_shaft.CurrentDeposit.OnChangePaw -= ChangePawHandler;
    }

    private void ChangePawHandler(double value)
    {
        Debug.Log("Change Paw: " + value);
        if(value > 0)
        {
            m_lyNuocHolder.gameObject.SetActive(true);
        }
        else
        {
            m_lyNuocHolder.gameObject.SetActive(false);
        }
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
            UpdateFrameButtonUpgrade(level);
   
        }
    }

    void UpdateFrameButtonUpgrade(int currentLevel)
    {

        Image imgButtonUpgrade = m_upgradeButton.GetComponent<Image>(); 
        if (currentLevel <= 200)
        {
            imgButtonUpgrade.sprite = Resources.Load<Sprite>(MainGameData.FrameLevelButton[ManagerLocation.Shaft][0]);
        }
        else if( currentLevel>200 && currentLevel <= 400)
        {
            imgButtonUpgrade.sprite = Resources.Load<Sprite>(MainGameData.FrameLevelButton[ManagerLocation.Shaft][1]);
        }
        else if (currentLevel > 400 && currentLevel <= 600)
        {
            imgButtonUpgrade.sprite = Resources.Load<Sprite>(MainGameData.FrameLevelButton[ManagerLocation.Shaft][2]);
        }
        else if (currentLevel > 600 && currentLevel <= 800)
        {
            imgButtonUpgrade.sprite = Resources.Load<Sprite>(MainGameData.FrameLevelButton[ManagerLocation.Shaft][3]);
        }
    }


    void BuyNewShaft()
    {
        if (PawManager.Instance.CurrentPaw >= ShaftManager.Instance.CurrentCost)
        {
            PawManager.Instance.RemovePaw(ShaftManager.Instance.CurrentCost);
            ShaftManager.Instance.AddShaft();
            m_buyNewShaftButton.gameObject.SetActive(false);
            PawManager.Instance.OnPawChanged?.Invoke(PawManager.Instance.CurrentPaw);
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

    #region DEBUG
    [Button]
    private void AddLevel(int valueAdd)
    {
        m_shaftUpgrade.Upgrade(valueAdd );
    }
    #endregion
}
