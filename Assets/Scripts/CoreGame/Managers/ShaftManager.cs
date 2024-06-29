using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.UIElements;

public class ShaftManager : Patterns.Singleton<ShaftManager>
{
    [Header("Prefab")]
    [SerializeField] private Shaft shaftPrefab;
    [SerializeField] private float shaftYOffset = 1.716f;
    [SerializeField] private double currentCost = 0;

    [Header("Basement")]
    [SerializeField] public List<Shaft> Shafts = new();

    [Header("Shaft")]
    [SerializeField] private Vector3 firstShaftPosition = new(0.656000018f, -0.0390000008f, 0);
    [SerializeField] int maxShaftCount = 30;

    public double CurrentCost => currentCost;
    private void Start()
    {
        InitializeShafts();
    }

    public void AddShaft()
    {
        Transform lastShaft = Shafts[^1].transform;
        Shaft newShaft = Instantiate(shaftPrefab, lastShaft.position, Quaternion.identity);
        newShaft.transform.localPosition += new Vector3(0, -shaftYOffset, 0);
        newShaft.IndexBoost = BaseShaftIndexScale();

        ShaftUpgrade shaftUpgrade = newShaft.GetComponent<ShaftUpgrade>();
        shaftUpgrade.SetInitialValue(CalculateNextShaftCost(), 1);

        Shafts.Add(newShaft);
        newShaft.shaftIndex = Shafts.Count - 1;
        newShaft.gameObject.GetComponent<ShaftUI>().NewShaftCostText.text = Currency.DisplayCurrency(CalculateNextShaftCost());
    }

    private void InitializeShafts()
    {
        if (!Load())
        {
            Shaft firstShaft = Instantiate(shaftPrefab, firstShaftPosition, Quaternion.identity);
            Shafts.Add(firstShaft);
            firstShaft.shaftIndex = Shafts.Count - 1;

            ShaftUpgrade shaftUpgrade = firstShaft.GetComponent<ShaftUpgrade>();
            shaftUpgrade.SetInitialValue(10, 1);

            firstShaft.gameObject.GetComponent<ShaftUI>().NewShaftCostText.text = Currency.DisplayCurrency(CalculateNextShaftCost());
        }
    }

    private double CalculateNextShaftCost()
    {
        int shaftCount = Shafts.Count;
        currentCost = shaftCount switch
        {
            0 => 10,
            1 => 2600,
            2 => 78000,
            _ => Mathf.Pow(20, shaftCount - 2) * 78000,
        };

        return currentCost;
    }

    private double BaseShaftIndexScale()
    {
        double scale = 1;

        int shaftCount = Shafts.Count;

        scale = shaftCount switch
        {
            0 => 1,
            1 => 50,
            _ => Mathf.Pow(10, shaftCount - 1) * 50,
        };
        return scale;
    }

    public void Save()
    {
        //create JSON to save data
        Dictionary<string, object> saveData = new Dictionary<string, object>();
        saveData.Add("Shafts", Shafts.Count);
        saveData.Add("CurrentCost", currentCost);

        List<object> shafts = new List<object>();
        foreach (Shaft shaft in Shafts)
        {
            Dictionary<string, object> shaftData = new Dictionary<string, object>
            {
                { "Index", shaft.shaftIndex },
                { "LevelBoost", shaft.LevelBoost },
                { "IndexBoost", shaft.IndexBoost },
                { "Brewers", shaft.Brewers.Count },
                { "CurrentDeposit", shaft.CurrentDeposit.CurrentPaw },
                {"Level", shaft.gameObject.GetComponent<ShaftUpgrade>().CurrentLevel},
                {"InitCost", shaft.gameObject.GetComponent<ShaftUpgrade>().GetInitialCost()}
            };
            shafts.Add(shaftData);
        }
        saveData.Add("ShaftsData", shafts);
        Debug.Log("save data: " + saveData.ToString());
        string json = JsonConvert.SerializeObject(saveData, Formatting.Indented);
        Debug.Log("save: " + json);
        PlayerPrefs.SetString("ShaftManager", json);
    }

    private bool Load()
    {
        if (PlayerPrefs.HasKey("ShaftManager"))
        {
            string json = PlayerPrefs.GetString("ShaftManager");
            Debug.Log(json);
            Data saveData = JsonConvert.DeserializeObject<Data>(json);

            int shaftsCount = saveData.Shafts;
            currentCost = saveData.CurrentCost;

            foreach (ShaftData shaftData in saveData.ShaftsData)
            {
                int index = shaftData.Index;
                double levelBoost = shaftData.LevelBoost;
                double indexBoost = shaftData.IndexBoost;
                int brewers = shaftData.Brewers;
                double currentDeposit = shaftData.CurrentDeposit;
                int level = shaftData.Level;
                double initCost = shaftData.InitCost;

                Vector3 position = Shafts.Count switch
                {
                    0 => firstShaftPosition,
                    _ => Shafts[^1].transform.position + new Vector3(0, -shaftYOffset, 0),
                };

                Shaft shaft = Instantiate(shaftPrefab, position, Quaternion.identity);
                shaft.LevelBoost = levelBoost;
                shaft.IndexBoost = indexBoost;
                shaft.shaftIndex = index;
                shaft.numberBrewer = brewers;
                shaft.gameObject.GetComponent<ShaftUpgrade>().SetInitialValue(initCost, level);
                shaft.SetDepositValue(currentDeposit);

                Shafts.Add(shaft);
            }
            return true;
        }
        return false;
    }


    public class Data
    {
        public int Shafts { get; set; }
        public double CurrentCost { get; set; }
        public List<ShaftData> ShaftsData { get; set; }
    }

    public class ShaftData
    {
        public int Index { get; set; }
        public double LevelBoost { get; set; }
        public double IndexBoost { get; set; }
        public int Brewers { get; set; }
        public double CurrentDeposit { get; set; }
        public int Level { get; set; }
        public double InitCost { get; set; }
    }
}


