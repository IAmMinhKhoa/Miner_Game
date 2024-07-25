using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class LevelPanelUI : MonoBehaviour
{
    [SerializeField] private Image imageUpgarde;
    [SerializeField] private BaseUpgrade baseUpgrade;
    private float y_pos;
    private Tween tween;

    void Start()
    {
        y_pos = imageUpgarde.gameObject.GetComponent<RectTransform>().position.y;
    }

    void OnEnable()
    {
        PawManager.Instance.OnPawChanged += OnPawChanged;
    }

    void OnDisable()
    {
        PawManager.Instance.OnPawChanged -= OnPawChanged;
    }

    private void OnPawChanged(double paw)
    {
        bool isActive = paw >= baseUpgrade.CurrentCost;
        imageUpgarde.gameObject.SetActive(isActive);

        if (isActive)
        {
            int amount = UpgradeManager.Instance.CalculateUpgradeAmount(paw, baseUpgrade);

            if (amount < 51 && amount > 0)
            {
                imageUpgarde.sprite = Resources.Load<Sprite>(MainGameData.CanUpgradeButton[0]);
            }
            else if (amount < 101)
            {
                imageUpgarde.sprite = Resources.Load<Sprite>(MainGameData.CanUpgradeButton[1]);
            }
            else
            {
                imageUpgarde.sprite = Resources.Load<Sprite>(MainGameData.CanUpgradeButton[2]);
            }
            var rect = imageUpgarde.gameObject.GetComponent<RectTransform>();
            if (tween == null || !tween.IsActive())
            {
                tween = rect.DOMoveY(y_pos + 0.05f, 0.5f).SetLoops(-1, LoopType.Yoyo);
            }
        }
        else
        {
            if (tween != null)
            {
                tween.Kill();
            }
            var rect = imageUpgarde.gameObject.GetComponent<RectTransform>();
            rect.position = new Vector3(rect.position.x, y_pos, rect.position.z);
        }
    }
}
