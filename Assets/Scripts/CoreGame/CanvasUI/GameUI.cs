using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameUI : Patterns.Singleton<GameUI>
{
    [SerializeField] private TextMeshProUGUI pawText;
    [SerializeField] private TextMeshProUGUI pawPerSecondText;

    private void Update()
    {
        pawText.text = Currency.DisplayCurrency(PawManager.Instance.CurrentPaw);
    }
}
