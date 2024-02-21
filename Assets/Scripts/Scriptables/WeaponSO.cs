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
    public GameObject weaponPrefab;
    public WeaponRarity weaponRarity;
    public Sprite weaponScopeImage;

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
}

public enum WeaponRarity
{
    DEFAULT = 0,
    RARE = 1,
    LEGENDARY = 2,
    ULTIMATE = 3,
    GOD = 4
}