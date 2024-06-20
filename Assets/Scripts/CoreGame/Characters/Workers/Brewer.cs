using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using TMPro;
using NOOD;

public class Brewer : BaseWorker
{
    public Shaft CurrentShaft { get; set; }
    private TextMeshPro numberText;

    [SerializeField] private bool isWorking = false;
    public double ProductPerSecond
    {
        get => config.ProductPerSecond * CurrentShaft.LevelBoost * CurrentShaft.IndexBoost;
    }

    void Start()
    {
        // Create text on head
        numberText = GameData.Instance.InstantiatePrefab(PrefabEnum.HeadText).GetComponent<TextMeshPro>();
        numberText.transform.SetParent(this.transform);
        numberText.transform.localPosition = new Vector3(0, 1.2f, 0);
    }

    private void Update()
    {
        if (!isWorking)
            {
                isWorking = true;
                Move(CurrentShaft.BrewLocation.position);
            }
    }

    protected override async void Collect()
    {
        ChangeGoal();
        await IECollect();
    }

    protected override void Deposit()
    {
        CurrentShaft.CurrentDeposit.AddPaw(CurrentProduct);
        CurrentProduct = 0;
        numberText.text = "0";
        ChangeGoal();
        isWorking = false;
        Debug.Log("Brewing finished" + isWorking);
    }

    protected override async UniTask IECollect()
    {
        PlayTextAnimation();
        await UniTask.Delay((int)config.WorkingTime * 1000);
        CurrentProduct = ProductPerSecond * config.WorkingTime;
        Move(CurrentShaft.BrewerLocation.position);
    }

    private async void PlayTextAnimation()
    {
        double max = ProductPerSecond * config.WorkingTime;
        double temp = 0; 
        while(temp < max)
        {
            await UniTask.Yield();
            temp += ProductPerSecond * CurrentShaft.LevelBoost * Time.deltaTime;
            numberText.SetText(Currency.DisplayCurrency(temp));
        }
        numberText.SetText(Currency.DisplayCurrency(max));
    }

}
