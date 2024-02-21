using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIManager : MonoBehaviour
{
    [SerializeField] private GUIHealthbar healthbar;
    [SerializeField] private GUIInventory inventory;
    [SerializeField] private GUIScreenActions screenActions;

    public GUIScreenActions ScreenActions
    { get { return screenActions; } }

    [SerializeField] private GUIWeaponScope weaponScope;

    public GUIWeaponScope WeaponScope
    { get { return weaponScope; } }

    private Player player;
    private InventoryController inventoryController;

    public static GUIManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void RegisterPlayer(Player player)
    {
        this.player = player;
        this.player.OnCurrentHealthChange.AddListener(healthbar.UpdateHealth);
    }

    public void RegisterInventoryController(InventoryController controller)
    {
        this.inventoryController = controller;
        this.inventoryController.OnItemAddedToInventory.AddListener(inventory.UpdateInventory);
        this.inventoryController.OnItemRemovedInventory.AddListener(inventory.UpdateInventory);
        this.inventoryController.OnInventorySlotChanged.AddListener(inventory.UpdateInventorySlots);
    }
}