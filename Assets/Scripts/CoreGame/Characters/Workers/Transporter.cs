using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Transporter : BaseWorker
{
    public Couter Couter { get; set; }

    [SerializeField] private bool isWorking = false;

    private void Update()
    {
        if (!isWorking)
        {
            isWorking = true;
            Move(Couter.CouterLocation.position);
        }
    }

    protected override async void Collect()
    {
        ChangeGoal();
        await IECollect();
    }

    protected override void Deposit()
    {
        Couter.CurrentDeposit.AddPaw(Couter.CurrentProduct);
        Couter.CurrentProduct = 0;
        ChangeGoal();
        isWorking = false;
    }

    protected override async UniTask IECollect()
    {
        await UniTask.Delay((int)config.WorkingTime * 1000);
        Couter.CurrentProduct = config.ProductPerSecond * config.WorkingTime * Couter.BoostScale;
        Move(Couter.TransporterLocation.position);
    }
}
