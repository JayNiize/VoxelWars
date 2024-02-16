using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIHealthbar : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI labelHealth;
    [SerializeField] private Slider sliderHealth;

    public void UpdateHealth(int currentHealth, int maxHealth)
    {
        DOTween.Kill(labelHealth.transform);
        sliderHealth.value = currentHealth / (float)maxHealth;
        labelHealth.transform.DOScale(Vector3.one * 1.2f, 0.25f).SetLoops(0, LoopType.Yoyo);
        labelHealth.text = $"{currentHealth}/{maxHealth}";
    }
}