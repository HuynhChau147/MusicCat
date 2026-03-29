using DG.Tweening;

public class LoseState : GameStateBase
{
    public LoseState(GameplayMN gameplay) : base(gameplay) { }

    public override void Enter()
    {
        gameplay.FreezeTime();

        gameplay.NoteSpawner.StopPlay();
        gameplay.CatManager.OnLoseAnim();

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