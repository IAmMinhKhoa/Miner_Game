using System.Collections;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BoostButtonUI : MonoBehaviour
{

    #region UI
    [SerializeField] private Image boostImage;
    [SerializeField] private Image cooldownImage;
    [SerializeField] private Image activeImage;
    [SerializeField] private TMP_Text textTiming;
    #endregion



    #region Controller
    private Manager _manager;

    [SerializeField] private BaseManagerLocation baseManagerLocation;

    [ReadOnly] float TickRate = 1f;
    private float _timerTickRate = 0;
    #endregion

    void OnEnable()
    {
        baseManagerLocation.OnChangeManager += OnChangeManager;
    }

    void OnDisable()
    {
        baseManagerLocation.OnChangeManager -= OnChangeManager;
    }

    void OnChangeManager(Manager manager)
    {
        _manager = manager;

        if (_manager == null)
        {
            // Hide all images
            boostImage.fillAmount = 0;
            cooldownImage.fillAmount = 0;
            activeImage.fillAmount = 0;
        }
        else
        {
            // Show active image
            activeImage.fillAmount = 1;
            cooldownImage.fillAmount = 0;
            boostImage.fillAmount = 1;

            var boostType = _manager.BoostType;
            boostImage.sprite = Resources.Load<Sprite>(MainGameData.BoostButtonUIs[boostType][2]);
            cooldownImage.sprite = Resources.Load<Sprite>(MainGameData.BoostButtonUIs[boostType][1]);
            activeImage.sprite = Resources.Load<Sprite>(MainGameData.BoostButtonUIs[boostType][0]);
        }
		
    }

    void Update()
    {
        _timerTickRate += Time.deltaTime;

        if (_timerTickRate >= TickRate) // Ki?m tra n?u ?� qua 1 gi�y
        {
            _timerTickRate = 0f; // ??t l?i bi?n ??m

            if (_manager == null)
            {
                return;
            }

            if (_manager.CurrentBoostTime > 0)
            {
                float value = _manager.CurrentBoostTime / (_manager.BoostTime * 60);
                activeImage.fillAmount = value;
                SetUiTiming(_manager.CurrentBoostTime);
            }
            else if (_manager.CurrentCooldownTime > 0)
            {
                activeImage.fillAmount = 0;
                float value = _manager.CurrentCooldownTime / (_manager.CooldownTime * 60);
                cooldownImage.fillAmount = value;
                SetUiTiming(_manager.CurrentCooldownTime);
            }
            else
            {
                activeImage.fillAmount = 1;
                textTiming.text = "";
            }
        }
    }

    private void SetUiTiming(float value)
    {
        textTiming.text = Common.ConvertSecondsToMinutes(value).ToString();
    }
}
