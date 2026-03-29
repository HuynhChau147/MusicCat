using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using PoolMN;
using UnityEngine;

public class Note : MonoBehaviour
{
    private float hitTime;
    private int candyNumber;
    private float travelTime;
    private Vector3 startPos;
    private Vector3 targetPos;
    private bool isLastNote = false;
    [SerializeField] private int score;
    public int Score => score;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Sprite[] candyTypes;

    private bool isHit;

    public void Init(float hitTime, float travelTime, Vector3 startPos, Vector3 targetPos, int _candyNumber, bool isLolipop, bool _isLastNote = false)
    {
        this.hitTime = hitTime;
        this.travelTime = travelTime;
        this.startPos = startPos;
        this.targetPos = targetPos;
        this.candyNumber = _candyNumber;
        this.isLastNote = _isLastNote;

        if (!isLolipop)
            spriteRenderer.sprite = candyTypes[candyNumber];

        isHit = false;
        gameObject.SetActive(true);
    }

    void Update()
    {
        UpdatePosition();
    }

    void UpdatePosition()
    {
        float songTime = GameplayMN.Instance.GetSongTime();

        float spawnTime = hitTime - travelTime;
        float t = (songTime - spawnTime) / travelTime;

        t = Mathf.Clamp01(t);

        transform.position = Vector3.Lerp(startPos, targetPos, t);
    }

    public void Hit()
    {
        if (isHit) return;

        isHit = true;
        gameObject.SetActive(false);
        PoolManager.instance.ReturnToPool(this.gameObject);
        if (isLastNote)
            DOVirtual.DelayedCall(0.5f, () => GameplayMN.Instance.Win());
    }
}

[System.Serializable]
public class NoteData
{
    public int id;
    public int n;
    public float ta;
    public float ts;
    public float d;
    public int v;
    public int pid;
}

[Serializable]
public enum NoteType
{
    Normal,
    Long,
    Strong,
    Lollipop
}
