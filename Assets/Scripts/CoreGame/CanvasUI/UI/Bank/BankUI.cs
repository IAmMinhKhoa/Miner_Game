using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using NOOD.SerializableDictionary;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.UI;

public class BankUI : MonoBehaviour
{
    [SerializeField] private SerializableDictionary<int, int> _moneyPackageDictionary = new SerializableDictionary<int, int>();
    [SerializeField] private SerializableDictionary<int, int> _pawPackageDictionary = new SerializableDictionary<int, int>();

    [SerializeField] private MoneyButton _moneyBtnPref;
    [SerializeField] private PawButton _pawBtnPref;
    [SerializeField] private Transform _moneyContentTrans, _pawContentTrans;
    [SerializeField] private RectTransform _content;


    void Start()
    {
        LoadFromDictionary();
        _moneyBtnPref.gameObject.SetActive(false);
        _pawBtnPref.gameObject.SetActive(false);
    }
    private void LoadFromDictionary()
    {
        foreach (var moneyPair in _moneyPackageDictionary.Dictionary)
        {
            MoneyButton moneyBtn = Instantiate<MoneyButton>(_moneyBtnPref, _moneyContentTrans);
            moneyBtn.gameObject.SetActive(true);
            moneyBtn.SetData(moneyPair.Key, moneyPair.Value);
        }
        foreach (var pawPair in _pawPackageDictionary.Dictionary)
        {
            PawButton pawBtn = Instantiate<PawButton>(_pawBtnPref, _pawContentTrans);
            pawBtn.gameObject.SetActive(true);
            pawBtn.SetData(pawPair.Key, pawPair.Value);
            LayoutRebuilder.ForceRebuildLayoutImmediate(_content);
        }
    }
}
