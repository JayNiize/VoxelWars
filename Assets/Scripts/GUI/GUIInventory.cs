using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GUIInventory : MonoBehaviour
{
    private List<GUIInventorySlot> inventorySlots = new List<GUIInventorySlot>();

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
            inventorySlots.Add(slot);
        }
        Debug.Log(inventorySlots.Count);
    }

    public void UpdateInventory(Dictionary<WeaponSO, int> inventory)
    {
        int index = 0;
        foreach (KeyValuePair<WeaponSO, int> kvp in inventory)
        {
            inventorySlots[index].SetSlotData(AssetPreview.GetMiniThumbnail(kvp.Key.weaponPrefab), kvp.Value);
            index++;
        }
    }
}