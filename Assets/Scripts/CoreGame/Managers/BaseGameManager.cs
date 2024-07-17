using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class BaseGameManager : MonoBehaviour
{
    #region ----Enums----
    protected enum GameState
    {
        CheckingForUpdates,
        LoadData,
        ProcessLoadingData,
        LoadAudio,
        ProcessLoadingAudio,
        Done
    }
    #endregion

    #region ----Variables----
    protected static BaseGameManager s_instance;
    //protected BaseManagerSetting gameManagerSetting;
    protected GameState baseGameState;
    protected string gameStateContent;
    protected int totalGameStates;
    protected int gameStateCount;

    #endregion

    #region ----Properties----
    #endregion

    #region ----Unity Methods----
    protected virtual void Awake()
    {
        Init();
    }

    protected virtual void Start()
    {
        Setup();
    }

    protected virtual void Update()
    {
        UpdateGameStates();
    }

    protected virtual void OnDestroy()
    {
        s_instance = null;
    }
    #endregion

    #region ----Custom Methods----
    protected virtual void Init()
    {
        if (!ReferenceEquals(s_instance, null))
        {
            Destroy(gameObject);
            return;
        }

        s_instance = this;
        totalGameStates += Enum.GetNames(typeof(GameState)).Length - 1;
        //gameManagerSetting = new BaseManagerSetting(null);
        SetState(GameState.CheckingForUpdates);
    }

    protected virtual void Setup()
    {
        //gameManagerSetting = Resources.Load<BaseManagerSetting>("BaseManagerSetting");
    }

    protected virtual void UpdateGameStates()
    {
        switch (baseGameState)
        {
            case GameState.CheckingForUpdates:
                gameStateContent = "Checking for updates...";
                gameStateCount++;
                SetState(GameState.LoadData);
                break;
            case GameState.LoadData:
                gameStateContent = "Loading data...";
                gameStateCount++;
                SetState(GameState.ProcessLoadingData);
                break;
            case GameState.ProcessLoadingData:
                gameStateContent = "Processing data...";
                gameStateCount++;
                SetState(GameState.LoadAudio);
                break;
            case GameState.LoadAudio:
                gameStateContent = "Loading audio...";
                gameStateCount++;
                SetState(GameState.ProcessLoadingAudio);
                break;
            case GameState.ProcessLoadingAudio:
                gameStateContent = "Processing audio...";
                gameStateCount++;
                SetState(GameState.Done);
                break;
            case GameState.Done:
                break;
        }
    }

    protected void SetState(GameState newState)
    {
        baseGameState = newState;
    }
    protected void NextState()
    {
        baseGameState++;
    }
    #endregion
}
