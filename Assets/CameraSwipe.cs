using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwipe : MonoBehaviour
{
    [SerializeField] private Vector3 targetRotation;
    [SerializeField] private float duration = 90f;

    private void Awake()
    {
        transform.DOLocalRotate(targetRotation, duration, RotateMode.FastBeyond360).SetEase(Ease.OutSine).SetLoops(-1, LoopType.Yoyo);
    }
}