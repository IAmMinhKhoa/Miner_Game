using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataLoadManager : BaseGameManager
{
    protected override void Update()
    {
        base.Update();
    }

    protected override void UpdateGameStates()
    {
        switch (baseGameState)
        {
            case GameState.CheckingForUpdates:
                gameStateContent = "Checking for updates...";
                gameStateCount++;
                NextState();
                break;
            case GameState.LoadData:
                gameStateContent = "Loading data...";
                gameStateCount++;
                LoadData();
                NextState();
                break;
            case GameState.ProcessLoadingData:
                gameStateContent = "Processing data...";
                gameStateCount++;
                NextState();
                break;
            case GameState.LoadAudio:
                gameStateContent = "Loading audio...";
                gameStateCount++;
                NextState();
                break;
            case GameState.ProcessLoadingAudio:
                gameStateContent = "Processing audio...";
                gameStateCount++;
                NextState();
                break;
            case GameState.Done:
                break;
        }
    }

    private void LoadData()
    {
        //MainGameData.managerDataSOList = new List<ManagerDataSO>();
        MainGameData.managerDataSOList = Resources.LoadAll<ManagerDataSO>("ScriptableObjects/ManagerData").ToList();
        MainGameData.managerSpecieDataSOList = Resources.LoadAll<ManagerSpecieDataSO>("ScriptableObjects/ManagerSpecieData").ToList();
        MainGameData.managerTimeDataSOList = Resources.LoadAll<ManagerTimeDataSO>("ScriptableObjects/ManagerTimeData").ToList();
        Debug.Log("ManagerDataSO count: " + MainGameData.managerDataSOList.Count);
    }
}
