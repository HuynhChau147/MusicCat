using System;
using UnityEngine;

public class GameStateController
{
    public event Action<GameState> OnGameStateChanged;

    public GameState CurrentState { get; private set; } = GameState.None;

    private readonly GameplayMN gameplay;
    private GameStateBase currentState;
    private float frozenTime;

    public GameStateController(GameplayMN gameplay)
    {
        this.gameplay = gameplay ?? throw new ArgumentNullException(nameof(gameplay));
    }

    public void Initialize()
    {
        ChangeState(GameState.Preplay);
    }

    public void ChangeState(GameState newState)
    {
        if (CurrentState == newState) return;

        currentState?.Exit();
        CurrentState = newState;

        currentState = newState switch
        {
            GameState.Preplay => new PreplayState(gameplay),
            GameState.Playing => new PlayingState(gameplay),
            GameState.Lose => new LoseState(gameplay),
            GameState.Win => new WinState(gameplay),
            _ => null
        };

        Debug.Log($"Changed game state to {CurrentState}");

        currentState?.Enter();
        OnGameStateChanged?.Invoke(CurrentState);
    }

    public void StartGame()
    {
        if (CurrentState != GameState.Preplay) return;
        ChangeState(GameState.Playing);
    }

    public void PauseGame()
    {
        if (CurrentState != GameState.Playing) return;

        FreezeTime();
        gameplay.NoteSpawner.StopPlay();
        ChangeState(GameState.Preplay); // maybe dedicated pause state later
    }

    public void ResumeGame()
    {
        if (CurrentState != GameState.Preplay) return;

        ChangeState(GameState.Playing);
        gameplay.NoteSpawner.Ready();
    }

    public void PlayAgain()
    {
        gameplay.NoteSpawner.ClearNotes();
        gameplay.CatManager.ResetCatAnim();
        gameplay.UIManager.PlayAgain();
        frozenTime = 0f;
        ChangeState(GameState.Preplay);
    }

    public void Lose()
    {
        if (CurrentState == GameState.Lose || CurrentState == GameState.Win) return;

        FreezeTime();
        ChangeState(GameState.Lose);
    }

    public void Win()
    {
        if (CurrentState != GameState.Playing) return;
        ChangeState(GameState.Win);
    }

    public float GetSongTime()
    {
        if (CurrentState == GameState.Lose || CurrentState == GameState.Preplay)
            return frozenTime;

        return AudioMN.Instance.GetTime();
    }

    public void FreezeTime()
    {
        frozenTime = AudioMN.Instance.GetTime();
    }

    public float GetLaneXPos(int lane)
    {
        return gameplay.CatManager.GetLaneXPos(lane);
    }
}