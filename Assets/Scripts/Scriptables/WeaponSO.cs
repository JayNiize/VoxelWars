using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Custom/Weapon")]
public class WeaponSO : ItemSO
{
    public int Damage;
    public float ScopeStrength;
    public float Speed;
    public float ReloadTime;
    public int MagazineSize;

    public Sprite ScopeSprite;

    public AmmoSO AmmoType;

    public int GetWeaponDamage()
    {
        switch (Rarity)
        {
            case RarityEnum.NORMAL:
                return Damage;

            case RarityEnum.RARE:
                return (int)(Damage * 1.2f);

            case RarityEnum.LEGENDARY:
                return (int)(Damage * 1.4f);

            case RarityEnum.ULTIMATE:
                return (int)(Damage * 1.75f);

            case RarityEnum.GOD:
                return (int)(Damage * 2f);

            default:
                return Damage;
        }
    }
}