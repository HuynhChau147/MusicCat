using DG.Tweening;
using PoolMN;
using Spine.Unity;
using TMPro;
using UnityEngine;

public class DuetCat : MonoBehaviour
{
    public int lane; // 0 = left, 1 = right
    [Header("Movement")]
    public float moveSpeed = 15f;
    public float minX = -3f;
    public float maxX = 3f;
    [Header("Animation")]
    [SerializeField] SkeletonAnimation skeletonAnimation;
    [SerializeField] Transform textSpawnHolder;
    string[] texts = { "Great!", "Yummy!", "Sweet!" };

    private float targetX;

    void Start()
    {
        targetX = transform.position.x;
        Debug.LogError("DuetCat subscribing to input lane " + lane);
        InputMN.Instance.OnLaneMove += HandleMove;
        PlayAnimation(CatAnimName.Idle_Playing, true);
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        float currentX = transform.position.x;

        float newX = Mathf.MoveTowards(currentX, targetX, moveSpeed * Time.deltaTime);

        float dir = targetX - currentX;

        var scale = skeletonAnimation.transform.localScale;
        float absX = Mathf.Abs(scale.x);

        if (dir > 0.01f)
            scale.x = absX;
        else if (dir < -0.01f)
            scale.x = -absX;

        skeletonAnimation.transform.localScale = scale;

        newX = Mathf.Clamp(newX, minX, maxX);
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }

    public void SetTargetX(float x)
    {
        targetX = Mathf.Clamp(x, minX, maxX);
    }

    void OnDisable()
    {
        InputMN.Instance.OnLaneMove -= HandleMove;
    }

    void HandleMove(int inputLane, float targetX)
    {
        if (inputLane != lane) return;
        SetTargetX(targetX);
    }

    public void OnLose()
    {
        PlayAnimation(CatAnimName.Miss_Object_Lose, true);
    }

    public void ResetCat()
    {
        PlayAnimation(CatAnimName.Idle_Playing, true);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Note note = collision.GetComponent<Note>();
        if (note != null)
        {
            HandleMatchNote(note);
        }
    }

    private void HandleMatchNote(Note note)
    {
        note.Hit();
        PlayAnimation(CatAnimName.Eat_Shot, false);
        SpawnMatchingText();
        GameplayMN.OnHit?.Invoke(note, HitType.Perfect);
    }

    void PlayAnimation(string animName, bool loop = false)
    {
        skeletonAnimation.state.SetAnimation(0, animName, loop);
    }

    private void SpawnMatchingText()
    {
        var random = Random.Range(0, 2);

        var po = PoolManager.GetPoolByName("MatchingText");
        if (po == null)
        {
            Debug.LogError("Pool not found: MatchingText");
            return;
        }

        GameObject textObj = po.GetPooledObject(false);

        textObj.transform.SetParent(textSpawnHolder, false);
        textObj.transform.localScale = Vector3.zero;

        var tmp = textObj.GetComponent<TextMeshPro>();
        tmp.alpha = 1f;
        tmp.text = texts[Random.Range(0, texts.Length)];

        textObj.SetActive(true);

        Sequence sequence = DOTween.Sequence();

        sequence.Append(textObj.transform.DOScale(0.08f, 0.3f).SetEase(Ease.OutBack));
        sequence.Join(textObj.transform.DOLocalMoveY(1.4f, 0.3f).SetEase(Ease.OutBack));
        // sequence.AppendInterval(0.1f);
        sequence.Join(tmp.DOFade(0f, 0.3f).SetEase(Ease.InBack));

        sequence.OnComplete(() =>
        {
            PoolManager.instance.ReturnToPool(textObj);
        });
    }

    public void OnWin()
    {
        PlayAnimation(CatAnimName.Cheering_Happy_Victory, true);
    }
}