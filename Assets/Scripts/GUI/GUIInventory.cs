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
            guiInventorySlots[index].SetSlotData(AssetPreview.GetMiniThumbnail(slot.Weapon.weaponPrefab), slot.Ammo);
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
                guiInventorySlots[i].transform.DOScale(1.1f, 0.1f).SetEase(Ease.OutCirc);
            }
            else
            {
                inventorySlots[i].OnAmmoChanged.RemoveAllListeners();
                guiInventorySlots[i].transform.DOScale(1f, 0.1f).SetEase(Ease.OutCirc);
            }
        }
    }

    private void UpdateAmmoLabel(int ammo)
    {
        guiActiveInventorySlot.SetAmmoLabel(ammo);
    }
}