using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerPickup : NetworkBehaviour
{
    private InventoryController inventoryController;
    private List<GameObject> pickupables = new List<GameObject>();

    private void Awake()
    {
        inventoryController = GetComponentInParent<InventoryController>();
    }

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            Destroy(this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IPickupable pickupable))
        {
            pickupable.ShowPickupInfo();
            if (!pickupables.Contains(other.gameObject))
            {
                pickupables.Add(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out IPickupable pickupable))
        {
            pickupable.HidePickupInfo();
            if (pickupables.Contains(other.gameObject))
            {
                pickupables.Remove(other.gameObject);
            }
        }
    }

    public void Pickup()
    {
        if (pickupables.Count > 0)
        {
            GameObject pickupable = pickupables[0];
            pickupables.Remove(pickupable);
            //OLD

            //NEW
            inventoryController.AddToInventory(pickupable.GetComponent<WorldWeapon>().GetWeaponInformation(), 30);
            Destroy(pickupable);
        }
    }
}