using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CounterUI : MonoBehaviour
{
    public static Action OnUpgradeRequest;

    [Header("UI Button")]
    [SerializeField] private Button m_upgradeButton;
    [SerializeField] private Button m_managerButton;
    [SerializeField] private Button m_boostButton;

    [Header("UI Text")]
    [SerializeField] private TextMeshProUGUI m_pawText;
    [SerializeField] private TextMeshProUGUI m_levelText;
    [SerializeField] private TextMeshProUGUI m_costText;

	[SerializeField]
	SpriteRenderer m_bgCounter;
	// [Header("Visual object")]
	// [SerializeField] private GameObject m_quayGiaoNuocHolder;

	private Counter m_counter;
    private CounterUpgrade m_counterUpgrade;

    void Awake()
    {
        m_counter = GetComponent<Counter>();
        m_counterUpgrade = GetComponent<CounterUpgrade>();
    }

    void Start()
    {
        m_levelText.text =  m_counterUpgrade.CurrentLevel.ToString();
        m_costText.text = Currency.DisplayCurrency(m_counterUpgrade.CurrentCost);
        UpdateFrameButtonUpgrade(m_counterUpgrade.CurrentLevel);
    }

    void Update()
    {
        m_pawText.text = Currency.DisplayCurrency(PawManager.Instance.CurrentPaw);
        m_costText.text = Currency.DisplayCurrency(m_counterUpgrade.CurrentCost);
        m_levelText.text =  m_counterUpgrade.CurrentLevel.ToString();
    }

    void OnEnable()
    {
        m_upgradeButton.onClick.AddListener(UpgradeRequest);
        m_managerButton.onClick.AddListener(OpenManagerPanel);
        m_boostButton.onClick.AddListener(ActiveBoost);
        BaseUpgrade.OnUpgrade += UpdateUpgradeButton;
    }

    void OnDisable()
    {
        m_upgradeButton.onClick.RemoveListener(UpgradeRequest);
        m_managerButton.onClick.RemoveListener(OpenManagerPanel);
        m_boostButton.onClick.RemoveListener(ActiveBoost);
        BaseUpgrade.OnUpgrade -= UpdateUpgradeButton;
    }

    void CallUpgrade()
    {
        if (PawManager.Instance.CurrentPaw >= m_counterUpgrade.CurrentCost)
        {
            m_counterUpgrade.Upgrade(1);
        }
    }

    void UpdateUpgradeButton(BaseUpgrade upgrade, int level)
    {
        if (upgrade == m_counterUpgrade)
        {
            m_levelText.text = "Level " + level;
            m_costText.text = Currency.DisplayCurrency(m_counterUpgrade.CurrentCost);
            UpdateFrameButtonUpgrade(level);
        }
    }
    void UpdateFrameButtonUpgrade(int currentLevel)
    {

        Image imgButtonUpgrade = m_upgradeButton.GetComponent<Image>();
        if (currentLevel <= 600)
        {
            imgButtonUpgrade.sprite = Resources.Load<Sprite>(MainGameData.FrameLevelButton[ManagerLocation.Counter][0]);
        }
        else if (currentLevel > 600 && currentLevel <= 1200)
        {
            imgButtonUpgrade.sprite = Resources.Load<Sprite>(MainGameData.FrameLevelButton[ManagerLocation.Counter][1]);
        }
        else if (currentLevel > 1200 && currentLevel <= 1800)
        {
            imgButtonUpgrade.sprite = Resources.Load<Sprite>(MainGameData.FrameLevelButton[ManagerLocation.Counter][2]);
        }
        else if (currentLevel > 1800 && currentLevel <= 2400)
        {
            imgButtonUpgrade.sprite = Resources.Load<Sprite>(MainGameData.FrameLevelButton[ManagerLocation.Counter][3]);
        }
    }
    void ActiveBoost()
    {
        m_counter.RunBoost();
    }

    void OpenManagerPanel()
    {
        ManagersController.Instance.OpenManagerPanel(m_counter.ManagerLocation);
    }

    public void UpgradeRequest()
    {
        OnUpgradeRequest?.Invoke();
    }

	public void ChangeSkin(CounterSkin data)
	{
		m_bgCounter.sprite = SkinManager.Instance.skinResource.skinBgCounter[int.Parse(data.idBackGround)].sprite; ;
	}
}
