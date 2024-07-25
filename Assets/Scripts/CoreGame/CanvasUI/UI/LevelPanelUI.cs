using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPanelUI : MonoBehaviour
{
    [SerializeField] private GameObject imageUpgarde;
    [SerializeField] private BaseUpgrade baseUpgrade;

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
        imageUpgarde.SetActive(paw >= baseUpgrade.CurrentCost);
    }
}
