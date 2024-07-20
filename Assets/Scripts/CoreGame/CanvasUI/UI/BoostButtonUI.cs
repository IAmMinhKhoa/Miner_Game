using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class BoostButtonUI : MonoBehaviour
{
[SerializeField] private Image boostImage;
[SerializeField] private Image cooldownImage;
[SerializeField] private Image activeImage;

private Manager _manager;

[SerializeField] private BaseManagerLocation baseManagerLocation;

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
        boostImage.fillAmount = 0;

        var boostType = _manager.BoostType;
        boostImage.sprite = Resources.Load<Sprite>(MainGameData.BoostButtonUIs[boostType][0]);
        cooldownImage.sprite = Resources.Load<Sprite>(MainGameData.BoostButtonUIs[boostType][1]);
        activeImage.sprite = Resources.Load<Sprite>(MainGameData.BoostButtonUIs[boostType][2]);
    }
}

void Update()
{
    if (_manager == null)
    {
        return;
    }

    if (_manager.CurrentBoostTime > 0)
    {
        float value = _manager.CurrentBoostTime / (_manager.BoostTime * 60);
        boostImage.fillAmount = value;
    }
    else if (_manager.CurrentCooldownTime > 0)
    {
        float value = _manager.CurrentBoostTime / (_manager.BoostTime * 60);
        cooldownImage.fillAmount = value;
    }
}
}
