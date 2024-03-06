using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    private List<WeaponSO> weaponList = new List<WeaponSO>();
    private List<AmmoSO> ammoList = new List<AmmoSO>();
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
        ammoList = Resources.LoadAll<AmmoSO>("Models/Items/Ammo").ToList();
        Debug.Log($"Loaded {weaponList.Count} weapons");
    }

    public List<WeaponSO> GetAllWeapons()
    {
        return weaponList;
    }

    public List<WeaponSO> GetAllWeaponsByRarity(RarityEnum weaponRarity)
    {
        return weaponList.Where(x => x.Rarity == weaponRarity).ToList();
    }

    public List<AmmoSO> GetAllAmmo()
    {
        return ammoList;
    }
}