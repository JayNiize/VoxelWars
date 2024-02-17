using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GUIManager : NetworkBehaviour
{
    [SerializeField] private GUIHealthbar healthbar;
    [SerializeField] private GUIInventory inventory;

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

    public override void OnNetworkSpawn()
    {
        Debug.Log("GUI MANAGER");
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
    }
}