using System;
using UnityEngine;

public class CatManager : MonoBehaviour
{
    [SerializeField] DuetCat cat1;
    [SerializeField] DuetCat cat2;

    public void OnLoseAnim()
    {
        if (cat1 != null)
            cat1.OnLose();
        if (cat2 != null)
            cat2.OnLose();
    }

    public void ResetCatAnim()
    {
        if (cat1 != null)
            cat1.ResetCat();
        if (cat2 != null)
            cat2.ResetCat();
    }

    internal float GetLaneXPos(int lane)
    {
        return lane switch
        {
            0 => cat1.transform.position.x,
            1 => cat2.transform.position.x,
            _ => throw new ArgumentException($"Invalid lane: {lane}")
        };
    }

    public void OnWinAnim()
    {
        if (cat1 != null)
            cat1.OnWin();
        if (cat2 != null)
            cat2.OnWin();
    }
}