public class PreplayState : GameStateBase
{
    public PreplayState(GameplayMN gameplay) : base(gameplay) { }

    public override void Enter()
    {
        gameplay.UIManager.ShowPreplay(true);
    }

    public override void Exit()
    {
        gameplay.UIManager.ShowPreplay(false);
    }
}