using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Brewer : BaseWorker
{
    // [SerializeField] private Transform m_brewLocation;
    // [SerializeField] private Transform m_depositLocation;

    public Shaft CurrentShaft { get; set; }

    [SerializeField] private bool isWorking = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            if (!isWorking)
            {
                isWorking = true;
                Move(CurrentShaft.BrewLocation.position);
            }
        }
    }

    protected override async void Collect()
    {
        Debug.Log("Brewing");
        ChangeGoal();
        await IECollect();
    }

    protected override void Deposit()
    {
        CurrentShaft.CurrentDeposit.DepositPaw(CurrentProduct);
        CurrentProduct = 0;
        ChangeGoal();
        isWorking = false;
        Debug.Log("Brewing finished" + isWorking);
    }

    protected override async UniTask IECollect()
    {
        await UniTask.Delay((int)WorkingTime * 1000);
        CurrentProduct = ProductPerSecond * WorkingTime;
        Move(CurrentShaft.BrewerLocation.position);
    }
}
