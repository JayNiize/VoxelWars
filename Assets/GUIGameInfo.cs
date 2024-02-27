using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIGameInfo : MonoBehaviour
{
    [SerializeField] private Image zoneImage;
    [SerializeField] private TMPro.TextMeshProUGUI labelTimer;

    public void UpdateTimer(float timer)
    {
        TimeSpan time = TimeSpan.FromSeconds((int)timer);
        labelTimer.text = time.ToString(@"mm\:ss");
    }

    public void UpdateIsResting(bool isResting)
    {
        Debug.Log("isresting " + isResting);
        if (!isResting)
        {
            zoneImage.transform.DOScale(0.5f, 1).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.OutCirc);
        }
        else
        {
            zoneImage.transform.DOKill();
            zoneImage.transform.localScale = Vector3.one;
        }
    }
}