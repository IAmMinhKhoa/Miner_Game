using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField]
    private MineMiner mineMinerPrefab;
    [SerializeField]
    private Deposit depositPrefab;

    [Header("Location")]
    [SerializeField]
    private Transform mineLocation;
    [SerializeField]
    private Transform minerLocation;
    [SerializeField]
    private Transform depositLocation;

    public Transform MineLocation => mineLocation;
    public Transform MinerLocation => minerLocation;
    public Transform DepositLocation => depositLocation;
    public List<MineMiner> Miners => _miners;
    public Deposit CurrentDeposit { get; set; }

    private List<MineMiner> _miners = new();

    public void CreateMiner()
    {
        MineMiner miner = Instantiate(mineMinerPrefab, minerLocation.position, Quaternion.identity);
        miner.transform.SetParent(minerLocation);
        miner.CurrentMine = this;

        if (_miners.Count > 0)
        {
            miner.GoldCapacity = _miners[0].GoldCapacity;
            miner.GoldPerSecond = _miners[0].GoldPerSecond;
            miner.MoveSpeed = _miners[0].MoveSpeed;
        }

        _miners.Add(miner);
    }

    private void CreateDeposit()
    {
        Deposit deposit = Instantiate(depositPrefab, depositLocation.position, Quaternion.identity);
        deposit.transform.SetParent(transform);
        CurrentDeposit = deposit;
    }

    void Start()
    {
        CreateDeposit();
        CreateMiner();
    }
}
