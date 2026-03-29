public class PlayingState : GameStateBase
{
    public PlayingState(GameplayMN gameplay) : base(gameplay) { }

    public override void Enter()
    {
        gameplay.ScoreManager.ResetScore();
        gameplay.UIManager.ShowText(true);
        gameplay.NoteSpawner.Ready();
    }
}