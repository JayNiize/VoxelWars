using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class WeaponController : NetworkBehaviour
{
    private const float BULLET_TRAIL_TIME = 0.1f;
    [SerializeField] private Transform weaponParent;

    private WeaponSO currentWeapon;

    public WeaponSO CurrentWeapon
    { get { return currentWeapon; } }

    private float tempShootingDuration = -1;
    private float tempReloadDuration;
    private WorldWeaponInHand currentWeaponWorld;
    private InventoryController inventoryController;

    private bool isReloading;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            this.enabled = false;
        }
    }

    private void Start()
    {
        inventoryController = GetComponent<InventoryController>();
    }

    public void EquipWeapon(WeaponSO weaponSO)
    {
        if (inventoryController.GetCurrentSlot().Ammo <= 0)
        {
            isReloading = true;
            StopAllCoroutines();
            StartCoroutine(ReloadWeapon());
        }
        else
        {
            tempShootingDuration = -1f;
            isReloading = false;
        }

        if (weaponParent.childCount > 0)
        {
            for (int i = 0; i < weaponParent.childCount - 1; i++)
            {
                Destroy(weaponParent.GetChild(i).gameObject);
            }
        }

        if (weaponSO == null)
        {
            currentWeapon = null;
            return;
        }
        GameObject go = Instantiate(weaponSO.Prefab, weaponParent.transform);

        currentWeaponWorld = go.AddComponent<WorldWeaponInHand>();
        currentWeaponWorld.transform.localPosition = Vector3.zero;
        currentWeaponWorld.transform.localRotation = Quaternion.Euler(Vector3.zero);
        currentWeaponWorld.gameObject.layer = 3;
        currentWeapon = weaponSO;
    }

    public void SetCurrentWeapon(WeaponSO weaponSO)
    {
        currentWeapon = weaponSO;
    }

    public void ShootWithWeapon()
    {
        if (currentWeapon != null && !isReloading)
        {
            tempShootingDuration -= Time.deltaTime;
            if (tempShootingDuration <= 0)
            {
                inventoryController.GetCurrentSlot().Ammo--;
                tempShootingDuration = currentWeapon.Speed;
                Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
                Ray ray = Camera.main.ScreenPointToRay(screenCenter);
                RequestShootServerRpc(ray, "JD");
                if (inventoryController.GetCurrentSlot().Ammo <= 0)
                {
                    isReloading = true;
                    StartCoroutine(ReloadWeapon());
                }
            }
        }
    }

    private bool CalculateCriticalHit()
    {
        int critNumber = UnityEngine.Random.Range(0, 1000);
        return critNumber < 50;
    }

    internal bool HasWeaponEquipped()
    {
        return currentWeapon != null;
    }

    public IEnumerator ReloadWeapon()
    {
        if (currentWeapon == null)
        {
            yield break;
        }
        int availableTotalAmmo = inventoryController.GetTotalAmmo(currentWeapon.AmmoType);
        if (availableTotalAmmo <= 0)
        {
            Debug.Log("No Ammo");
            yield break;
        }
        Debug.Log("Reload");
        float tempTime = 0;
        while (tempTime < currentWeapon.ReloadTime)
        {
            tempTime += Time.deltaTime;
            GUIInventory.Instance.UpdateReloadSlider(tempTime / currentWeapon.ReloadTime);
            yield return new WaitForEndOfFrame();
        }

        GUIInventory.Instance.UpdateReloadSlider(0);
        if (availableTotalAmmo < currentWeapon.MagazineSize)
        {
            inventoryController.GetCurrentSlot().Ammo = availableTotalAmmo;
            inventoryController.RemoveFromInventory(currentWeapon.AmmoType, availableTotalAmmo);
        }
        else
        {
            inventoryController.GetCurrentSlot().Ammo = currentWeapon.MagazineSize;
            inventoryController.RemoveFromInventory(currentWeapon.AmmoType, currentWeapon.MagazineSize);
        }
        inventoryController.OnInventorySlotChanged.Invoke(inventoryController.GetInventorySlots(), inventoryController.GetCurrentSlot().SlotId, inventoryController.GetTotalAmmo(currentWeapon.AmmoType));
        isReloading = false;
    }

    [ServerRpc]
    private void RequestShootServerRpc(Ray ray, string sourcePlayerId)
    {
        ShootClientRpc(ray, sourcePlayerId);
    }

    [ClientRpc]
    private void ShootClientRpc(Ray ray, string sourcePlayerId)
    {
        RaycastHit hit;
        Instantiate(PrefabManager.Instance.ParticlesShooting, currentWeaponWorld.GetMuzzlePosition(), Quaternion.LookRotation(ray.direction, Vector3.up));
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~LayerMask.GetMask("Player")))
        {
            Debug.Log($"hit from {sourcePlayerId}");
            Transform bulletTrail = Instantiate(PrefabManager.Instance.TrailBullet, currentWeaponWorld.GetMuzzlePosition(), Quaternion.identity).transform;
            bulletTrail.DOMove(hit.point, BULLET_TRAIL_TIME);
            Instantiate(PrefabManager.Instance.ParticlesHit, hit.point, Quaternion.identity);
            if (hit.transform.TryGetComponent<IHitable>(out IHitable hitable))
            {
                bool isCritialHit = CalculateCriticalHit();
                int damage = (int)(currentWeapon.GetWeaponDamage() * (isCritialHit ? 1.2f : 1f) * UnityEngine.Random.Range(0.95f, 1.05f));
                GUIManager.Instance.ScreenActions.SpawnDamageLabel(hit.point, damage.ToString(), isCritialHit);
                hitable.Hit(damage, transform);
            }
        }
        else
        {
            Transform bulletTrail = Instantiate(PrefabManager.Instance.TrailBullet, currentWeaponWorld.GetMuzzlePosition(), Quaternion.identity).transform;
            bulletTrail.DOMove(ray.GetPoint(50f), BULLET_TRAIL_TIME);
        }
    }
}