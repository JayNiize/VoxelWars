using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIInventorySlot : MonoBehaviour
{
    [SerializeField] private RawImage weaponImage;
    [SerializeField] private TMPro.TextMeshProUGUI labelAmmo;
    private bool isActiveSlot;

    public bool IsActiveSlot
    { get { return isActiveSlot; } }

    public void SetSlotData(Texture2D weaponSprite, int ammoCount)
    {
        weaponImage.texture = weaponSprite;
        labelAmmo.text = ammoCount.ToString() + "x";
    }

    public bool ToggleActiveSlot()
    {
        isActiveSlot = !isActiveSlot;
        return isActiveSlot;
    }

    internal void SetAmmoLabel(int ammo)
    {
        labelAmmo.text = ammo.ToString() + "x";
    }
}