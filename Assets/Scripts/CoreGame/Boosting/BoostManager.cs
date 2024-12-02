using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class BoostManager : MonoBehaviour
{
    private float _adsBoost = 2f;
    private float _adsBoostTime = 0f;
    private readonly float _MAX_ADS_BOOST_TIME = 30 * 60 * 12;
    private readonly float _TIME_PER_ADS_BOOST = 30 * 60;

    [SerializeField] private float _currentBoostValue = 1f;

    public float CurrentBoostValue => _currentBoostValue;

    public static BoostManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        _currentBoostValue = 1f;
    }

    private async UniTaskVoid ActiveAdsBoost()
    {
        _currentBoostValue = _adsBoost;
        while (_adsBoostTime > 0)
        {
            _adsBoostTime -= Time.deltaTime;
            await UniTask.Yield();
        }

        _currentBoostValue = 1f;
    }

    public void BonusAdsBoost()
    {
        bool needBoost = _adsBoostTime <= 0;

        _adsBoostTime += _TIME_PER_ADS_BOOST;
        if (_adsBoostTime > _MAX_ADS_BOOST_TIME)
        {
            _adsBoostTime = _MAX_ADS_BOOST_TIME;
        }

        if (needBoost)
        {
            ActiveAdsBoost().Forget();
        }
    }
}
