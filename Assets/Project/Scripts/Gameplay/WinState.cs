using DG.Tweening;

public class WinState : GameStateBase
{
    public WinState(GameplayMN gameplay) : base(gameplay) { }

    public override void Enter()
    {
        gameplay.FreezeTime();
        gameplay.NoteSpawner.StopPlay();
        gameplay.CatManager.OnWinAnim();

        DOVirtual.DelayedCall(1f, () =>
        {
            gameplay.UIManager.ShowEndGame(() =>
            {
                gameplay.NoteSpawner.ClearNotes();
                gameplay.CatManager.ResetCatAnim();
                gameplay.UIManager.ShowText(false);
            });
        });
    }
}
