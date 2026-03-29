using UnityEngine;

public abstract class GameStateBase : MonoBehaviour
{
    protected GameplayMN gameplay;

    public GameStateBase(GameplayMN gameplay)
    {
        this.gameplay = gameplay;
    }

    public virtual void Enter() { }
    public virtual void Exit() { }
}

public enum GameState
{
    None,
    Preplay,
    Playing,
    Lose,
    Win
}