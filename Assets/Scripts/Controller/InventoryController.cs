using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private int inventorySize = 4;
    private WeaponController weaponController;
    private List<InventorySlot> inventorySlots = new List<InventorySlot>();

    public UnityEvent<List<InventorySlot>> OnItemAddedToInventory;
    public UnityEvent<List<InventorySlot>> OnItemRemovedInventory;
    public UnityEvent<List<InventorySlot>, int> OnInventorySlotChanged;

    private void Awake()
    {
        weaponController = GetComponent<WeaponController>();
        for (int i = 0; i < inventorySize; i++)
        {
            inventorySlots.Add(new InventorySlot(i));
        }
    }

    private void Start()
    {
        GUIManager.Instance.RegisterInventoryController(this);
    }

    public int GetAmmo(WeaponSO weapon)
    {
        InventorySlot foundSlot = inventorySlots.Where(x => x.Weapon == weapon).FirstOrDefault();
        return foundSlot != null ? foundSlot.Ammo : 0;
    }

    public void AddToInventory(WeaponSO weapon, int ammo)
    {
        if (inventorySlots.Where(x => x.Weapon == null).Count() == 0)
        {
            Debug.Log("Inventory is full");
            return;
        }

        InventorySlot foundSlot = inventorySlots.Where(x => x.Weapon == weapon).FirstOrDefault();
        if (foundSlot != null)
        {
            foundSlot.Ammo += ammo;
        }
        else
        {
            foundSlot = inventorySlots.Where(x => x.Weapon == null).First();
            foundSlot.Weapon = weapon;
            foundSlot.Ammo = ammo;
        }

        if (inventorySlots.Where(x => x.Weapon != null).Count() == 1)
        {
            weaponController.EquipWeapon(weapon);
        }

        OnItemAddedToInventory.Invoke(inventorySlots);
    }

    public void RemoveFromInventory(WeaponSO weapon)
    {
        InventorySlot foundSlot = inventorySlots.Where(x => x.Weapon == weapon).FirstOrDefault();
        if (foundSlot != null)
        {
            foundSlot.Weapon = null;
            foundSlot.Ammo = 0;
            OnItemRemovedInventory.Invoke(inventorySlots);
        }
    }

    public int GetInventorySize()
    {
        return inventorySize;
    }

    public InventorySlot GetSlotById(int id)
    {
        return inventorySlots[id];
    }

    internal void SetCurrentSlotIndex(int currentWeaponSlotIndex)
    {
        OnInventorySlotChanged.Invoke(inventorySlots, currentWeaponSlotIndex);
    }
}

public class InventorySlot
{
    private int slotId;
    private WeaponSO weapon;
    private int ammo;

    public UnityEvent<int> OnAmmoChanged = new UnityEvent<int>();

    public int SlotId
    { get { return slotId; } set { slotId = value; } }

    public WeaponSO Weapon
    { get { return weapon; } set { weapon = value; } }

    public int Ammo
    { get { return ammo; } set { ammo = value; OnAmmoChanged.Invoke(ammo); } }

    public InventorySlot(int slotId, WeaponSO weapon = null, int ammo = 0)
    {
        this.slotId = slotId;
        this.weapon = weapon;
        this.ammo = ammo;
    }
}