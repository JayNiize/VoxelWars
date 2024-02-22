using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LootManager : MonoBehaviour
{
    public static LootManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public WeaponRarity CalculateWeaponRarity()
    {
        List<WeaponRarityWeight> weights = Enum.GetValues(typeof(WeaponRarityWeight)).Cast<WeaponRarityWeight>().OrderByDescending(x => x).ToList();

        int randomNumber = UnityEngine.Random.Range(0, weights.Select(x => (int)x).Sum());
        Debug.Log(randomNumber);
        foreach (WeaponRarityWeight weight in weights)
        {
            if (randomNumber <= (int)weight)
            {
                return WeaponRarityConverter.ConvertFromWeight(weight);
            }
            randomNumber -= (int)weight;
        }
        return WeaponRarity.DEFAULT;
    }
}