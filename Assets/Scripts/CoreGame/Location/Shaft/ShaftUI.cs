using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;
using Cysharp.Threading.Tasks;
using System;
using Sirenix.OdinInspector;
using NOOD.SerializableDictionary;
using System.Linq;

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
    [SerializeField] private TextMeshProUGUI m_indexText;


    [Header("Visual object")]
    [SerializeField] private GameObject m_table;
    [SerializeField] private GameObject m_lyNuocHolder;
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private SerializableDictionary<int, SkeletonDataAsset> skeletonDataAssetDic;
	[Header("Skin Object")]
	[SerializeField] private SpriteRenderer m_br;
    [SerializeField] private SpriteRenderer m_waitTable;


    private SkeletonAnimation tableAnimation;
    private ShaftUpgrade m_shaftUpgrade;
    private Shaft m_shaft;

    private bool _isBrewing = false;


    void Awake()
    {
        m_shaft = GetComponent<Shaft>();
        m_shaftUpgrade = GetComponent<ShaftUpgrade>();
        m_lyNuocHolder.gameObject.SetActive(false);
        tableAnimation = m_table.GetComponent<SkeletonAnimation>();
    }

    void Start()
    {
        m_shaft.CurrentDeposit.OnChangePaw += ChangePawHandler;

        mainPanel.transform.SetParent(GameWorldUI.Instance.transform, true);
        m_indexText.text = (m_shaft.shaftIndex + 1).ToString();


        //First init Data frame by current lvl of shaft
        UpdateFrameButtonUpgrade(m_shaftUpgrade.CurrentLevel);

      

    }
   
    void Update()
    {
        m_pawText.text = Currency.DisplayCurrency(m_shaft.CurrentDeposit.CurrentPaw);
        m_levelText.text = m_shaftUpgrade.CurrentLevel.ToString();
        m_costText.text = Currency.DisplayCurrency(m_shaftUpgrade.CurrentCost);
    }

    void OnEnable()
    {
        BaseUpgrade.OnUpgrade += UpdateUpgradeButton;
        m_upgradeButton.onClick.AddListener(UpgradeRequest);
        m_buyNewShaftButton.onClick.AddListener(BuyNewShaft);
        m_managerButton.onClick.AddListener(OpenManagerPanel);
        m_boostButton.onClick.AddListener(ActiveBoost);
        m_shaft.OnUpgrade += Shaft_OnUpgradeHandler;
    }

    void OnDisable()
    {
        BaseUpgrade.OnUpgrade -= UpdateUpgradeButton;
        m_upgradeButton.onClick.RemoveListener(UpgradeRequest);
        m_buyNewShaftButton.onClick.RemoveListener(BuyNewShaft);
        m_managerButton.onClick.RemoveListener(OpenManagerPanel);
        m_boostButton.onClick.RemoveListener(ActiveBoost);
        m_shaft.OnUpgrade -= Shaft_OnUpgradeHandler;
        // m_shaft.CurrentDeposit.OnChangePaw -= ChangePawHandler;
    }

    private void Shaft_OnUpgradeHandler(int currentLevel)
    {
        foreach (var item in skeletonDataAssetDic.Dictionary)
        {
            if (currentLevel >= item.Key)
            {
                UpgradeTable(item.Value);
            }
        }
    }

    private void UpgradeTable(SkeletonDataAsset tableDataAsset)
    {
        tableAnimation.skeletonDataAsset = tableDataAsset;
        tableAnimation.Initialize(true, true);
    }

    private void ChangePawHandler(double value)
    {
        Debug.Log("Change Paw: " + value);
        if (value > 0)
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
        else if (currentLevel > 200 && currentLevel <= 400)
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
        public void ChangeSkin(ShaftSkin data)
	    {
            //set init Data Skin shaft
            SkinShaftBg backgroundEnum = (SkinShaftBg)int.Parse(data.idBackGround);
            SkinShaftWaitTable waitTableEnum = (SkinShaftWaitTable)int.Parse(data.idWaitTable);
            SkinShaftMilkCup milkCupEnum = (SkinShaftMilkCup)int.Parse(data.idMilkCup);


            m_br.sprite = SkinManager.SkinDataSO.GetBrShaft(backgroundEnum);
            m_waitTable.sprite = SkinManager.SkinDataSO.GetWaitTableShaft(waitTableEnum);
            foreach (Transform child in m_lyNuocHolder.transform)
            {
              child.GetComponent<SpriteRenderer>().sprite = SkinManager.SkinDataSO.GetMilkCupShaft(milkCupEnum);
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
        m_shaftUpgrade.Upgrade(valueAdd);
    }
    [Button]
    private void TestChangeSkinShaft(SkinShaftBg value1=SkinShaftBg.BR1, SkinShaftWaitTable value2=SkinShaftWaitTable.WT1, SkinShaftMilkCup value3=SkinShaftMilkCup.MC1)
    {
        m_shaft.shaftSkin.idBackGround = ((int)value1).ToString();
        m_shaft.shaftSkin.idWaitTable = ((int)value2).ToString();
        m_shaft.shaftSkin.idMilkCup = ((int)value3).ToString();
        ChangeSkin(m_shaft.shaftSkin);
    }
    #endregion
}
