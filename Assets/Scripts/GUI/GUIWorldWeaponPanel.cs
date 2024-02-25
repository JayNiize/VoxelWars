using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIWorldWeaponPanel : MonoBehaviour
{
    private const float SHOW_DURATION = 0.25f;

    private Canvas weaponInfoCanvas;
    [SerializeField] private TMPro.TextMeshProUGUI labelName;
    [SerializeField] private Image panel;
    [SerializeField] private TMPro.TextMeshProUGUI labelActionKey;

    private void Awake()
    {
        weaponInfoCanvas = GetComponent<Canvas>();
        transform.localPosition = Vector3.zero;
        transform.DOLocalMoveY(1.5f, SHOW_DURATION).SetEase(Ease.OutBack);
    }

    public void Setup(WeaponSO weaponSO)
    {
        panel.color = weaponSO.GetWeaponColor();
        labelActionKey.color = weaponSO.GetWeaponColor();
        labelName.text = weaponSO.weaponName;
        weaponInfoCanvas.worldCamera = CameraManager.Instance.GetMainCamera();
    }

    public void Setup(AmmoSO ammoSO, int ammoAmount)
    {
        panel.color = new Color(0.6f, 0.6f, 0.6f);
        labelActionKey.color = new Color(0.6f, 0.6f, 0.6f);
        labelName.text = ammoAmount.ToString() + "x " + ammoSO.ammoName;
        weaponInfoCanvas.worldCamera = CameraManager.Instance.GetMainCamera();
    }

    public void Hide()
    {
        DOTween.Kill(transform);
        transform.DOLocalMoveY(0, SHOW_DURATION).SetEase(Ease.InBack).OnComplete(() => { Destroy(gameObject); });
    }

    private void LateUpdate()
    {
        weaponInfoCanvas.GetComponent<RectTransform>().rotation = weaponInfoCanvas.worldCamera.transform.rotation;
    }
}