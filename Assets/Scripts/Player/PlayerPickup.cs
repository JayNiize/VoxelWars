using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerPickup : NetworkBehaviour
{
    private WeaponController weaponController;
    private InventoryController inventoryController;
    private List<GameObject> actionables = new List<GameObject>();

    private void Awake()
    {
        weaponController = GetComponentInParent<WeaponController>();
        inventoryController = GetComponentInParent<InventoryController>();
    }

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            this.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IActionable actionable))
        {
            actionable.ShowActionInfo();
            if (!actionables.Contains(other.gameObject))
            {
                actionables.Add(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out IActionable pickupable))
        {
            pickupable.HideActionInfo();
            if (actionables.Contains(other.gameObject))
            {
                actionables.Remove(other.gameObject);
            }
        }
    }

    public void ExecuteAction()
    {
        if (actionables.Count > 0)
        {
            GameObject pickupable = actionables[0];
            actionables.Remove(pickupable);

            pickupable.GetComponent<IActionable>().ExecuteAction();
        }
    }
}