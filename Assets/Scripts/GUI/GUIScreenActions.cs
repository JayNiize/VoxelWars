using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GUIScreenActions : MonoBehaviour
{
    private float DURATION = 0.2f;
    private float OFFSET_X = 32;
    private float OFFSET_Y = 32;

    [SerializeField] private GameObject damageAmountLabel;

    public void SpawnDamageLabel(Vector3 hitpoint, string text)
    {
        Vector3 spawnSpoint = CameraManager.Instance.GetMainCamera().WorldToScreenPoint(hitpoint) + new Vector3(Random.Range(-OFFSET_X, OFFSET_X), Random.Range(-OFFSET_Y, OFFSET_Y), 0);
        TextMeshProUGUI damageLabel = Instantiate(damageAmountLabel, spawnSpoint, Quaternion.identity, transform).GetComponent<TextMeshProUGUI>();
        damageLabel.text = text;
        Transform t = damageLabel.transform;

        t.localScale = Vector3.zero;
        t.DOScale(1, DURATION).SetEase(Ease.OutBack);
        Vector3 center = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        t.DOMove(spawnSpoint + (Vector3.up * (Screen.height / 8)), DURATION * 2f);
        t.DOScale(0, DURATION).SetDelay(DURATION).SetEase(Ease.InBack).OnComplete(() => { t.DOKill(); Destroy(t.gameObject); });
    }
}