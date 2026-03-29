using UnityEngine;

public class ScoreMN : MonoBehaviour
{
    public int combo;
    public int perfectCount;
    public int goodCount;
    public int missCount;
    public int totalScore;

    void OnEnable()
    {
        GameplayMN.OnHit += HandleHit;
    }

    void OnDisable()
    {
        GameplayMN.OnHit -= HandleHit;
    }

    public void ResetScore()
    {
        combo = 0;
        perfectCount = 0;
        goodCount = 0;
        missCount = 0;
        totalScore = 0;
    }

    void HandleHit(Note note, HitType hitType)
    {
        AddHit(note.Score, hitType);
    }

    public void AddHit(int baseScore, HitType type)
    {
        int finalScore = 0;

        switch (type)
        {
            case HitType.Perfect:
                finalScore = baseScore;
                combo++;
                perfectCount++;
                break;

            case HitType.Good:
                finalScore = (int)(baseScore * 0.7f);
                combo++;
                goodCount++;
                break;

            case HitType.Miss:
                combo = 0;
                missCount++;
                return;
        }

        totalScore += finalScore * GetMultiplier();
        GameplayMN.OnScoreChanged?.Invoke(combo, totalScore);
    }

    int GetMultiplier()
    {
        if (combo < 10) return 1;
        if (combo < 20) return 2;
        if (combo < 50) return 3;
        return 4;
    }
}

public enum HitType
{
    Perfect,
    Good,
    Miss
}