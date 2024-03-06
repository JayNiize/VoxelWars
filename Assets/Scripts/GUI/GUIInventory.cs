using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GUIInventory : MonoBehaviour
{
    private List<GUIInventorySlot> guiInventorySlots = new List<GUIInventorySlot>();
    private GUIInventorySlot guiActiveInventorySlot;
    public static GUIInventory Instance;

    [SerializeField] private TMPro.TextMeshProUGUI labelCurrentItem;
    [SerializeField] private TMPro.TextMeshProUGUI labelCurrentAmmo;
    [SerializeField] private TMPro.TextMeshProUGUI labelTotalAmmo;
    [SerializeField] private Slider sliderReload;
    [SerializeField] private Image backgroundCurrentWeapon;

    private ItemSO currentItem;

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
        foreach (InventorySlot slot in inventorySlots.Where(x => x.Item != null).ToList())
        {
            guiInventorySlots[index].SetSlotData(slot.Item, slot.Ammo);
            index++;
        }
    }

    internal void UpdateInventorySlots(List<InventorySlot> inventorySlots, int activeSlotId, int totalAmmoForCurrentItem)
    {
        for (int i = 0; i < guiInventorySlots.Count; i++)
        {
            if (i == activeSlotId)
            {
                guiActiveInventorySlot = guiInventorySlots[i];
                inventorySlots[i].OnCurrentAmmoChanged.AddListener(UpdateAmmoLabel);
                guiInventorySlots[i].SetActiveSlot(true);
                currentItem = inventorySlots[i].Item;

                labelCurrentAmmo.gameObject.SetActive(currentItem != null);
                labelTotalAmmo.gameObject.SetActive(currentItem != null);

                labelCurrentAmmo.text = inventorySlots[i].Ammo.ToString();
                labelTotalAmmo.text = totalAmmoForCurrentItem.ToString();

                labelCurrentItem.text = currentItem == null ? "" : currentItem.GetName();
                backgroundCurrentWeapon.color = currentItem == null ? new Color(0, 0, 0, 0) : currentItem.GetItemColor();
            }
            else
            {
                inventorySlots[i].OnCurrentAmmoChanged.RemoveAllListeners();
                guiInventorySlots[i].SetActiveSlot(false);
            }
        }
    }

    private void UpdateAmmoLabel(int ammo)
    {
        labelCurrentAmmo.text = ammo.ToString();
    }

    //internal void UpdateTotalAmmo(Dictionary<AmmoSO, int> ammoSlots)
    //{
    //    if (currentItem == null)
    //    {
    //        return;
    //    }
    //    if (!ammoSlots.ContainsKey(currentItem.Ammo))
    //    {
    //        return;
    //    }
    //    labelTotalAmmo.text = ammoSlots[currentItem.Ammo].ToString();
    //}

    internal void UpdateTotalAmmo(InventorySlot slot, int totalAmmo)
    {
        if ((WeaponSO)slot.Item == null)
        {
            labelTotalAmmo.text = "";
            labelCurrentAmmo.text = "";
        }

        labelTotalAmmo.text = slot.Ammo.ToString();
        labelCurrentAmmo.text = totalAmmo.ToString();
    }

    internal void UpdateReloadSlider(float value)
    {
        sliderReload.value = value;
    }
}