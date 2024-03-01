using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class InventoryController : NetworkBehaviour
{
    private const int INVENTORY_SIZE = 4;
    private WeaponController weaponController;

    private List<InventorySlot> inventorySlots = new List<InventorySlot>();
    private Dictionary<AmmoSO, int> totalAmmoSlots = new Dictionary<AmmoSO, int>();

    private int currentInventorySlotIndex = 0;

    public UnityEvent<List<InventorySlot>> OnItemAddedToInventory;
    public UnityEvent<List<InventorySlot>> OnItemRemovedInventory;
    public UnityEvent<List<InventorySlot>, int, int> OnInventorySlotChanged;
    public UnityEvent<Dictionary<AmmoSO, int>> OnAmmoAddedToInventory;

    private void Awake()
    {
        weaponController = GetComponent<WeaponController>();
    }

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            this.enabled = false;
        }
    }

    private void Start()
    {
        for (int i = 0; i < INVENTORY_SIZE; i++)
        {
            inventorySlots.Add(new InventorySlot(i));
        }
        GUIManager.Instance.RegisterInventoryController(this);
        OnInventorySlotChanged.Invoke(inventorySlots, currentInventorySlotIndex, 0);
    }

    public void SwitchItem(float value)
    {
        currentInventorySlotIndex += (value < 0) ? 1 : -1;

        if (currentInventorySlotIndex >= INVENTORY_SIZE)
        {
            currentInventorySlotIndex = 0;
        }

        if (currentInventorySlotIndex < 0)
        {
            currentInventorySlotIndex = INVENTORY_SIZE - 1;
        }

        InventorySlot CurrentSlot = inventorySlots[currentInventorySlotIndex];
        if (CurrentSlot.Item != null)
            weaponController.EquipWeapon((WeaponSO)CurrentSlot.Item);

        OnInventorySlotChanged.Invoke(inventorySlots, currentInventorySlotIndex, CurrentSlot.Item == null ? 0 : GetTotalAmmo(((WeaponSO)CurrentSlot.Item).AmmoType));
    }

    public void AddToInventory(WeaponSO weapon)
    {
        if (inventorySlots.Where(x => x.Item == null).Count() == 0)
        {
            Debug.Log("Inventory is full");
            return;
        }

        InventorySlot freeSlot = inventorySlots.Where(x => x.Item == null).FirstOrDefault();
        if (freeSlot != null)
        {
            freeSlot = inventorySlots.Where(x => x.Item == null).FirstOrDefault();
            freeSlot.Item = weapon;
            freeSlot.Ammo = weapon.MagazineSize;
        }

        if (freeSlot == GetCurrentSlot())
        {
            weaponController.EquipWeapon(weapon);
            OnInventorySlotChanged.Invoke(inventorySlots, currentInventorySlotIndex, freeSlot.Item == null ? 0 : GetTotalAmmo(((WeaponSO)freeSlot.Item).AmmoType));
        }

        OnItemAddedToInventory.Invoke(inventorySlots);
    }

    public void AddToInventory(AmmoSO ammo, int amount)
    {
        if (totalAmmoSlots.ContainsKey(ammo))
        {
            totalAmmoSlots[ammo] += amount;
        }
        else
        {
            totalAmmoSlots.Add(ammo, amount);
        }
        if (GetCurrentAmmo() <= 0)
        {
            StartCoroutine(weaponController.ReloadWeapon());
        }
        OnAmmoAddedToInventory.Invoke(totalAmmoSlots);
    }

    public List<InventorySlot> GetInventorySlots()
    {
        return inventorySlots;
    }

    public int GetCurrentAmmo()
    {
        return inventorySlots[currentInventorySlotIndex].Ammo;
    }

    public int GetTotalAmmo(AmmoSO ammo)
    {
        return !totalAmmoSlots.ContainsKey(ammo) ? 0 : totalAmmoSlots[ammo];
    }

    public void RemoveFromInventory(WeaponSO weapon)
    {
        InventorySlot foundSlot = inventorySlots.Where(x => x.Item == weapon).FirstOrDefault();
        if (foundSlot != null)
        {
            foundSlot.Item = null;
            foundSlot.Ammo = 0;
            OnItemRemovedInventory.Invoke(inventorySlots);
        }
    }

    internal void RemoveFromInventory(AmmoSO ammoSO, int ammoAmount)
    {
        if (totalAmmoSlots.ContainsKey(ammoSO))
        {
            totalAmmoSlots[ammoSO] -= ammoAmount;
            OnAmmoAddedToInventory.Invoke(totalAmmoSlots);
        }
    }

    internal InventorySlot GetCurrentSlot()
    {
        return inventorySlots[currentInventorySlotIndex];
    }
}

public class InventorySlot
{
    private int slotId;
    private ItemSO item;
    private int ammo;

    public UnityEvent<int> OnCurrentAmmoChanged = new UnityEvent<int>();

    public int SlotId
    { get { return slotId; } set { slotId = value; } }

    public ItemSO Item
    { get { return item; } set { item = value; } }

    public int Ammo
    { get { return ammo; } set { ammo = value; OnCurrentAmmoChanged.Invoke(ammo); } }

    public InventorySlot(int slotId, ItemSO item = null, int ammo = 0)
    {
        this.slotId = slotId;
        this.item = item;
        this.ammo = ammo;
    }
}