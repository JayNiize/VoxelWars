using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    private List<WeaponSO> weaponList = new List<WeaponSO>();
    public static ItemManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        weaponList = Resources.LoadAll<WeaponSO>("Models/Items/Weapons").ToList();
        Debug.Log($"Loaded {weaponList.Count} weapons");
    }

    public List<WeaponSO> GetAllWeapons()
    {
        return weaponList;
    }

    public List<WeaponSO> GetAllWeaponsByRarity(WeaponRarity weaponRarity)
    {
        return weaponList.Where(x => x.weaponRarity == weaponRarity).ToList();
    }
}