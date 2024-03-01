using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSO : ScriptableObject
{
    public string Name;
    public GameObject Prefab;
    public RarityEnum Rarity;
    public Sprite PreviewSprite;

    public string GetName()
    {
        return $"{GetFormattedEnumName(Rarity)} {Name}";
    }

    public static string GetFormattedEnumName(Enum enumValue)
    {
        string enumName = enumValue.ToString();
        return char.ToUpper(enumName[0]) + enumName.Substring(1).ToLower();
    }

    public Color GetItemColor()
    {
        switch (Rarity)
        {
            case RarityEnum.NORMAL:
                return new Color(0.6f, 0.6f, 0.6f);

            case RarityEnum.RARE:
                return new Color(0, 0.6f, 0f);

            case RarityEnum.LEGENDARY:
                return new Color(0f, 0.3f, 0.6f);

            case RarityEnum.ULTIMATE:
                return new Color(1, 0.6f, 0);

            case RarityEnum.GOD:
                return new Color(0.7f, 0f, 1f);

            default:
                return new Color(0.6f, 0.6f, 0.6f);
        }
    }
}

public enum RarityEnum
{
    NORMAL = 0,
    RARE = 1,
    LEGENDARY = 2,
    ULTIMATE = 3,
    GOD = 4
}

public enum RarityWeight
{
    DEFAULT = 500,
    RARE = 250,
    LEGENDARY = 125,
    ULTIMATE = 75,
    GOD = 50
}

public static class WeaponRarityConverter
{
    public static RarityEnum ConvertFromWeight(RarityWeight weight)
    {
        switch (weight)
        {
            case RarityWeight.DEFAULT:
                return RarityEnum.NORMAL;

            case RarityWeight.RARE:
                return RarityEnum.RARE;

            case RarityWeight.LEGENDARY:
                return RarityEnum.LEGENDARY;

            case RarityWeight.ULTIMATE:
                return RarityEnum.ULTIMATE;

            case RarityWeight.GOD:
                return RarityEnum.GOD;

            default:
                throw new ArgumentOutOfRangeException(nameof(weight), weight, null);
        }
    }
}