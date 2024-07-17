using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameManager : BaseGameManager
{
    #region ----Enums----
    public enum GameState
    {
        Setup,
        LoadData,
        ProcessLoadingData,
        Done,
    }
    #endregion

    #region ----Variables----
    private GameState mainGameState;
    private string gameStateContent;
    private int totalGameStates;
    private int gameStateCount;
    #endregion

    #region ----Unity Methods----
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
    #endregion

    #region ----Custom Methods----
    protected override void Init()
    {
        base.Init();
    }

    protected override void Setup()
    {
        base.Setup();
    }

    protected override void UpdateGameStates()
    {
        base.UpdateGameStates();
    }
    #endregion
}
