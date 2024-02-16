using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InventoryController : MonoBehaviour
{
    private WeaponController weaponController;
    private Dictionary<WeaponSO, int> weaponInventory = new Dictionary<WeaponSO, int>();

    public UnityEvent<Dictionary<WeaponSO, int>> OnItemAddedToInventory;
    public UnityEvent<Dictionary<WeaponSO, int>> OnItemRemovedInventory;

    private void Awake()
    {
        weaponController = GetComponent<WeaponController>();
    }

    private void Start()
    {
        GUIManager.Instance.RegisterInventoryController(this);
    }

    public int GetAmmo(WeaponSO weapon)
    {
        if (weaponInventory.ContainsKey(weapon)) return weaponInventory[weapon];
        return 0;
    }

    public void AddToInventory(WeaponSO weapon, int ammo)
    {
        if (weaponInventory.ContainsKey(weapon))
        {
            weaponInventory[weapon] += ammo;
        }
        else
        {
            weaponInventory.Add(weapon, ammo);
        }
        if (weaponInventory.Count == 1)
        {
            weaponController.EquipWeapon(weapon);
        }
        OnItemAddedToInventory.Invoke(weaponInventory);
    }

    public void RemoveFromInventory(WeaponSO weapon)
    {
        if (weaponInventory.ContainsKey(weapon))
        {
            weaponInventory.Remove(weapon);
            OnItemRemovedInventory.Invoke(weaponInventory);
        }
    }
}