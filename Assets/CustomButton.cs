using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomButton : Button, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public override void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOKill();
        transform.DOScale(1.05f, 0.1f).SetEase(Ease.OutCirc);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        transform.DOKill();
        transform.DOScale(1f, 0.1f).SetEase(Ease.OutCirc);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        transform.DOKill();
        transform.localScale = Vector3.one;
    }
}