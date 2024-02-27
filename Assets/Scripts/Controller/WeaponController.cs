using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WeaponController : MonoBehaviour
{
    private const float BULLET_TRAIL_TIME = 0.1f;
    [SerializeField] private Transform weaponParent;

    private WeaponSO currentWeapon;

    public WeaponSO CurrentWeapon
    { get { return currentWeapon; } }

    private int currentWeaponSlotIndex = 0;

    private float tempShootingDuration;
    private float tempReloadDuration;
    private WorldWeaponInHand currentWeaponWorld;
    private InventoryController inventoryController;
    private InventorySlot currentSlot;

    private InventorySlot CurrentSlot
    { get { return currentSlot; } set { currentSlot = value; OnCurrentWeaponSlotChange.Invoke(currentSlot); } }

    public UnityEvent<InventorySlot> OnCurrentWeaponSlotChange;

    private bool isReloading;

    private void Start()
    {
        inventoryController = GetComponent<InventoryController>();
    }

    public void EquipWeapon(WeaponSO weaponSO)
    {
        if (weaponParent.childCount > 0)
        {
            for (int i = 0; i < weaponParent.childCount; i++)
            {
                Destroy(weaponParent.GetChild(i).gameObject);
            }
        }

        if (weaponSO == null)
        {
            currentWeapon = null;
            return;
        }
        GameObject go = Instantiate(weaponSO.weaponPrefab, weaponParent.transform);

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
            tempShootingDuration += Time.deltaTime;
            if (tempShootingDuration >= currentWeapon.weaponSpeed)
            {
                currentSlot.Ammo--;
                tempShootingDuration = 0;
                Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
                Ray ray = Camera.main.ScreenPointToRay(screenCenter);
                RaycastHit hit;
                Instantiate(PrefabManager.Instance.ParticlesShooting, currentWeaponWorld.GetMuzzlePosition(), Quaternion.LookRotation(ray.direction, Vector3.up));
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~LayerMask.GetMask("Player")))
                {
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

                if (currentSlot.Ammo <= 0)
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

    internal void SwitchWeapon(float v, bool onPickup = false)
    {
        if (!onPickup)
        {
            currentWeaponSlotIndex += (v < 0) ? 1 : -1;

            if (currentWeaponSlotIndex >= inventoryController.GetInventorySize())
            {
                currentWeaponSlotIndex = 0;
            }

            if (currentWeaponSlotIndex < 0)
            {
                currentWeaponSlotIndex = inventoryController.GetInventorySize() - 1;
            }
        }
        inventoryController.SetCurrentSlotIndex(currentWeaponSlotIndex);
        currentSlot = inventoryController.GetSlotById(currentWeaponSlotIndex);
        EquipWeapon(currentSlot.Weapon);
        StopAllCoroutines();
        if (currentSlot.Ammo <= 0)
        {
            StartCoroutine(ReloadWeapon());
        }
    }

    public IEnumerator ReloadWeapon()
    {
        if (currentWeapon == null)
        {
            yield break;
        }
        int availableTotalAmmo = inventoryController.GetAmmoAmount(currentWeapon.weaponAmmo);
        if (availableTotalAmmo <= 0)
        {
            Debug.Log("No Ammo");
            yield break;
        }
        Debug.Log("Reload");
        float tempTime = 0;
        while (tempTime < currentWeapon.weaponReloadTime)
        {
            tempTime += Time.deltaTime;
            GUIInventory.Instance.UpdateReloadSlider(tempTime / currentWeapon.weaponReloadTime);
            yield return new WaitForEndOfFrame();
        }

        GUIInventory.Instance.UpdateReloadSlider(0);
        if (availableTotalAmmo < currentWeapon.weaponmagazineSize)
        {
            currentSlot.Ammo = availableTotalAmmo;
            inventoryController.RemoveFromInventory(currentWeapon.weaponAmmo, availableTotalAmmo);
        }
        else
        {
            currentSlot.Ammo = currentWeapon.weaponmagazineSize;
            inventoryController.RemoveFromInventory(currentWeapon.weaponAmmo, currentWeapon.weaponmagazineSize);
        }
        isReloading = false;
    }

    internal InventorySlot GetActiveSlot()
    {
        return currentSlot;
    }
}