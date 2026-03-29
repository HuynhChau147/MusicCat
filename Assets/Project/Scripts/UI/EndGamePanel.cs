using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class EndGamePanel : MonoBehaviour
{
    [SerializeField] List<Image> images;

    public void Show()
    {
        gameObject.SetActive(true);

        foreach (var img in images)
        {
            img.color = new Color(img.color.r, img.color.g, img.color.b, 0f);
            img.gameObject.SetActive(true);
            img.DOFade(1f, 0.5f);
        }
    }

    public void Hide()
    {
        foreach (var img in images)
        {
            img.DOFade(0f, 0.5f).OnComplete(() => img.gameObject.SetActive(false));
        }

        DOVirtual.DelayedCall(0.5f, () =>
        {
            gameObject.SetActive(false);
        });
    }
}
