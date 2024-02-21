using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GUIWeaponScope : MonoBehaviour
{
    [SerializeField] private Image weaponScopeImage;

    internal void ShowScope(Sprite weaponScopeImage)
    {
        this.weaponScopeImage.sprite = weaponScopeImage;
        this.weaponScopeImage.gameObject.SetActive(true);
    }

    internal void HideScope()
    {
        this.weaponScopeImage.gameObject.SetActive(false);
    }
}