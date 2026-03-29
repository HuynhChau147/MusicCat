using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;
using UnityEngine.SocialPlatforms.Impl;
using PoolMN;

public class GameplayMN : MonoBehaviour
{
    [SerializeField] NoteSpawner noteSpawner;
    [SerializeField] CatManager catManager;
    [SerializeField] UIMN uiManager;
    [SerializeField] ScoreMN scoreManager;

    public NoteSpawner NoteSpawner => noteSpawner;
    public CatManager CatManager => catManager;
    public UIMN UIManager => uiManager;
    public ScoreMN ScoreManager => scoreManager;

    public static GameplayMN Instance { get; private set; }
    public static Action<Note, HitType> OnHit;
    public static Action<int, int> OnScoreChanged;
    public static Action<GameState> OnGameStateChanged;

    public GameStateController StateController { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        StateController = new GameStateController(this);
        StateController.OnGameStateChanged += state => OnGameStateChanged?.Invoke(state);
    }

    private void Start()
    {
        PoolManager.instance.Init();
        uiManager.SetUpCanvas();
        StateController.Initialize();
        uiManager.ShowText(false);
    }

    public void StartGame() => StateController.StartGame();
    public void PauseGame() => StateController.PauseGame();
    public void ResumeGame() => StateController.ResumeGame();
    public void PlayAgain() => StateController.PlayAgain();
    public void Lose() => StateController.Lose();
    public void Win() => StateController.Win();
    public float GetSongTime() => StateController.GetSongTime();
    public float GetLaneXPos(int lane) => StateController.GetLaneXPos(lane);
    public void FreezeTime() => StateController.FreezeTime();
}

