using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Custom/Weapon")]
public class WeaponSO : ScriptableObject
{
    public string weaponName;
    public int weaponDamage;
    public float weaponScopeStrength;
    public float weaponReachRadius;
    public float weaponSpeed;
    public float weaponReloadTime;
    public int weaponmagazineSize;
    public GameObject weaponPrefab;
    public WeaponRarity weaponRarity;
    public Sprite weaponScopeImage;
    public Sprite weaponPreviewImage;
    public AmmoSO weaponAmmo;

    public Color GetWeaponColor()
    {
        switch (weaponRarity)
        {
            case WeaponRarity.DEFAULT:
                return new Color(0.6f, 0.6f, 0.6f);

            case WeaponRarity.RARE:
                return new Color(0, 0.6f, 0f);

            case WeaponRarity.LEGENDARY:
                return new Color(0f, 0.3f, 0.6f);

            case WeaponRarity.ULTIMATE:
                return new Color(1, 0.6f, 0);

            case WeaponRarity.GOD:
                return new Color(0.7f, 0f, 1f);

            default:
                return new Color(0.6f, 0.6f, 0.6f);
        }
    }

    public int GetWeaponDamage()
    {
        switch (weaponRarity)
        {
            case WeaponRarity.DEFAULT:
                return weaponDamage;

            case WeaponRarity.RARE:
                return (int)(weaponDamage * 1.2f);

            case WeaponRarity.LEGENDARY:
                return (int)(weaponDamage * 1.4f);

            case WeaponRarity.ULTIMATE:
                return (int)(weaponDamage * 1.75f);

            case WeaponRarity.GOD:
                return (int)(weaponDamage * 2f);

            default:
                return weaponDamage;
        }
    }
}

public enum WeaponRarity
{
    DEFAULT = 0,
    RARE = 1,
    LEGENDARY = 2,
    ULTIMATE = 3,
    GOD = 4
}

public enum WeaponRarityWeight
{
    DEFAULT = 500,
    RARE = 250,
    LEGENDARY = 125,
    ULTIMATE = 75,
    GOD = 50
}

public static class WeaponRarityConverter
{
    public static WeaponRarity ConvertFromWeight(WeaponRarityWeight weight)
    {
        switch (weight)
        {
            case WeaponRarityWeight.DEFAULT:
                return WeaponRarity.DEFAULT;

            case WeaponRarityWeight.RARE:
                return WeaponRarity.RARE;

            case WeaponRarityWeight.LEGENDARY:
                return WeaponRarity.LEGENDARY;

            case WeaponRarityWeight.ULTIMATE:
                return WeaponRarity.ULTIMATE;

            case WeaponRarityWeight.GOD:
                return WeaponRarity.GOD;

            default:
                throw new ArgumentOutOfRangeException(nameof(weight), weight, null);
        }
    }
}