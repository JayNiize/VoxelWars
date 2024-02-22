using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIInventorySlot : MonoBehaviour
{
    [SerializeField] private Image weaponImage;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private TMPro.TextMeshProUGUI labelAmmo;
    private bool isActiveSlot;

    public bool IsActiveSlot
    { get { return isActiveSlot; } }

    public void SetSlotData(WeaponSO weapon, int ammoCount)
    {
        weaponImage.gameObject.SetActive(true);
        labelAmmo.gameObject.SetActive(true);
        backgroundImage.gameObject.SetActive(true);

        weaponImage.sprite = weapon.weaponPreviewImage;
        backgroundImage.color = weapon.GetWeaponColor();
        labelAmmo.text = ammoCount.ToString() + "x";
    }

    public void SetActiveSlot(bool isActive)
    {
        isActiveSlot = isActive;

        AnimateSlotImage();
        AnimateWeaponImage();
    }

    private void AnimateSlotImage()
    {
        weaponImage.transform.DOKill();
        transform.DOScale(isActiveSlot ? 1.1f : 1f, 0.1f).SetEase(Ease.OutCirc);
    }

    private void AnimateWeaponImage()
    {
        weaponImage.transform.DOKill();
        weaponImage.transform.DOScale(Vector3.one * (isActiveSlot ? 1.2f : 1f), 0.2f);
    }

    internal void SetAmmoLabel(int ammo)
    {
        labelAmmo.text = ammo.ToString() + "x";
    }
}