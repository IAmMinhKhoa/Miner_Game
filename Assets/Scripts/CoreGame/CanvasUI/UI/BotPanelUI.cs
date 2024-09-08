using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BotPanelUI : MonoBehaviour
{
    [Header("Support")]
    [SerializeField] private SoundSetting _soundSetting;
    [SerializeField] private SettingUI _settingUI;
    [SerializeField] private BankUI _bankUI;

    [Header("Button")]
    [SerializeField] private ButtonBehavior _btnSound;
    [SerializeField] private ButtonBehavior _btnSetting;
    [SerializeField] private ButtonBehavior _btnManager;
    [SerializeField] private ButtonBehavior _btnStore;
    [SerializeField] private ButtonBehavior _btnBoost;

    private void Awake()
    {
        _btnSound.onClickEvent.AddListener(_soundSetting.FadeInContainer);
        _btnSetting.onClickEvent.AddListener(_settingUI.Show);
        _btnStore.onClickEvent.AddListener(_bankUI.Show);
    }

    void OnDestroy()
    {
        _btnSound.onClickEvent.RemoveListener(_soundSetting.FadeInContainer);
        _btnSetting.onClickEvent.RemoveListener(_settingUI.Show);
        _btnStore.onClickEvent.RemoveListener(_bankUI.Show);
    }
}
