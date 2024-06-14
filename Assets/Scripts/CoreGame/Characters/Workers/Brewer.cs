using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using TMPro;
using NOOD;

public class Brewer : BaseWorker
{
    // [SerializeField] private Transform m_brewLocation;
    // [SerializeField] private Transform m_depositLocation;

    public Shaft CurrentShaft { get; set; }
    private TextMeshPro numberText;

    [SerializeField] private bool isWorking = false;

    void Start()
    {
        // Create text on head
        numberText = GameData.Instance.InstantiatePrefab(PrefabEnum.HeadText).GetComponent<TextMeshPro>();
        numberText.transform.parent = this.transform;
    }

    private void Update()
    {
        // if (Input.GetKeyDown(KeyCode.N))
        // {
        //     if (!isWorking)
        //     {
        //         isWorking = true;
        //         Move(CurrentShaft.BrewLocation.position);
        //     }
        // }

        if (!isWorking)
            {
                isWorking = true;
                Move(CurrentShaft.BrewLocation.position);
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
        CurrentShaft.CurrentDeposit.AddPaw(CurrentProduct);
        CurrentProduct = 0;
        ChangeGoal();
        isWorking = false;
        Debug.Log("Brewing finished" + isWorking);
    }

    protected override async UniTask IECollect()
    {

        await UniTask.Delay((int)config.WorkingTime * 1000);
        CurrentProduct = config.ProductPerSecond * config.WorkingTime * CurrentShaft.BoostScale;
        Move(CurrentShaft.BrewerLocation.position);
    }

    private void PlayTextAnimation(float to)
    {

    }
}
