using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class GUIInventory : MonoBehaviour
{
    private List<GUIInventorySlot> guiInventorySlots = new List<GUIInventorySlot>();
    private GUIInventorySlot guiActiveInventorySlot;
    public static GUIInventory Instance;

    [SerializeField] private TMPro.TextMeshProUGUI labelCurrentWeapon;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        foreach (GUIInventorySlot slot in transform.GetComponentsInChildren<GUIInventorySlot>())
        {
            guiInventorySlots.Add(slot);
        }
        Debug.Log(guiInventorySlots.Count);
    }

    public void UpdateInventory(List<InventorySlot> inventorySlots)
    {
        int index = 0;
        foreach (InventorySlot slot in inventorySlots.Where(x => x.Weapon != null).ToList())
        {
            guiInventorySlots[index].SetSlotData(slot.Weapon, slot.Ammo);
            index++;
        }
    }

    internal void UpdateInventorySlots(List<InventorySlot> inventorySlots, int activeSlotId)
    {
        for (int i = 0; i < guiInventorySlots.Count; i++)
        {
            if (i == activeSlotId)
            {
                guiActiveInventorySlot = guiInventorySlots[i];
                inventorySlots[i].OnAmmoChanged.AddListener(UpdateAmmoLabel);
                guiInventorySlots[i].SetActiveSlot(true);
                labelCurrentWeapon.text = inventorySlots[i].Weapon == null ? "" : inventorySlots[i].Weapon.weaponName;
            }
            else
            {
                inventorySlots[i].OnAmmoChanged.RemoveAllListeners();
                guiInventorySlots[i].SetActiveSlot(false);
            }
        }
    }

    private void UpdateAmmoLabel(int ammo)
    {
        guiActiveInventorySlot.SetAmmoLabel(ammo);
    }
}