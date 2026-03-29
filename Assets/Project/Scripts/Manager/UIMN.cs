using System;
using DG.Tweening;
using PoolMN;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIMN : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] ComboFXController comboFXController;
    [SerializeField] PreplayPanel preplayPanel;
    [SerializeField] EndGamePanel endGamePanel;
    [SerializeField] Image bgCircleMask;
    [SerializeField] Material mat;
    [SerializeField] Image pawIcon;
    [SerializeField] Image portraitBG;
    [SerializeField] Image landscapeBG;

    float radius = 1f;

    public void SetUpCanvas()
    {
        GetComponent<CanvasScaler>().matchWidthOrHeight = Screen.width >= Screen.height ? 1 : 0;
        portraitBG.gameObject.SetActive(Screen.height >= Screen.width);
        landscapeBG.gameObject.SetActive(Screen.width > Screen.height);
    }

    void OnEnable()
    {
        GameplayMN.OnScoreChanged += UpdateScore;
    }

    void OnDisable()
    {
        GameplayMN.OnScoreChanged -= UpdateScore;
    }

    public void UpdateScore(int combo, int score)
    {
        scoreText.text = $"{score}";
        comboFXController.OnComboChanged(combo, true);
    }

    public void ShowPreplay(bool isShow)
    {
        if (isShow)
            preplayPanel.Show();
        else
            preplayPanel.Hide();
    }

    public void ShowText(bool isShow)
    {
        scoreText.gameObject.SetActive(isShow);
        comboFXController.ShowText(isShow);
    }

    public void PlayAgain()
    {
        UpdateScore(0, 0);
        endGamePanel.Hide();
        ShowText(true);
    }

    [ContextMenu("Show End Game")]
    public void ShowEndGame(Action onCloseCompleted = null)
    {
        ChangeScene(() => onCloseCompleted?.Invoke(), () =>
        {
            endGamePanel.Show();
        });
    }

    public void ChangeScene(Action onCloseComplted, Action onComplete)
    {
        bgCircleMask.gameObject.SetActive(true);

        Sequence seq = DOTween.Sequence();

        // 1. Đóng vòng tròn
        seq.Append(DOTween.To(() => radius, x =>
        {
            radius = x;
            mat.SetFloat("_Radius", radius);
        }, 0f, 1f));

        seq.AppendCallback(() =>
        {
            pawIcon.rectTransform.localScale = Vector3.zero;
            pawIcon.gameObject.SetActive(true);
            onCloseComplted?.Invoke();
        });

        seq.Append(pawIcon.rectTransform.DOScale(1f, 0.5f).SetEase(Ease.OutBack));

        // 3. Thu nhỏ lại
        seq.Append(pawIcon.rectTransform.DOScale(0f, 0.5f).SetEase(Ease.InOutQuad));
        // seq.Append(pawIcon.rectTransform.DOScale(0f, 0.5f));

        seq.Append(DOTween.To(() => radius, x =>
        {
            radius = x;
            mat.SetFloat("_Radius", radius);
        }, 1f, 1f));


        // 5. Kết thúc
        seq.OnComplete(() =>
        {
            pawIcon.gameObject.SetActive(false);
            bgCircleMask.gameObject.SetActive(false);
            onComplete?.Invoke();
        });
    }
}