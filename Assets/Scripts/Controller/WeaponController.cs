using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class WeaponController : NetworkBehaviour
{
    private const float BULLET_TRAIL_TIME = 0.1f;
    [SerializeField] private Transform weaponParent;

    private WeaponSO currentWeapon;

    public WeaponSO CurrentWeapon
    { get { return currentWeapon; } }

    private Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
    private float tempShootingDuration = 99999f;
    private WorldWeaponInHand currentWeaponWorld;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            this.enabled = false;
        }
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
        if (currentWeapon != null)
        {
            tempShootingDuration += Time.deltaTime;
            if (tempShootingDuration >= currentWeapon.weaponSpeed)
            {
                tempShootingDuration = 0;
                Ray ray = Camera.main.ScreenPointToRay(screenCenter);
                Vector3 muzzlePos = currentWeaponWorld.GetMuzzlePosition();
                RequestShootServerRpc(ray, muzzlePos, currentWeapon.weaponDamage, "JD");
            }
        }
    }

    internal bool HasWeaponEquipped()
    {
        return currentWeapon != null;
    }

    [ServerRpc]
    private void RequestShootServerRpc(Ray ray, Vector3 muzzlePos, int damage, string sourcePlayerId)
    {
        ShootClientRpc(ray, muzzlePos, damage, sourcePlayerId);
    }

    [ClientRpc]
    private void ShootClientRpc(Ray ray, Vector3 muzzlePos, int damage, string sourcePlayerId)
    {
        Instantiate(PrefabManager.Instance.ParticlesShooting, muzzlePos, Quaternion.LookRotation(ray.direction, Vector3.up));
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, ~LayerMask.GetMask("Player")))
        {
            Transform bulletTrail = Instantiate(PrefabManager.Instance.TrailBullet, muzzlePos, Quaternion.identity).transform;
            bulletTrail.DOMove(hit.point, BULLET_TRAIL_TIME);
            Instantiate(PrefabManager.Instance.ParticlesHit, hit.point, Quaternion.identity);
            if (hit.transform.TryGetComponent(out IHitable hitable))
            {
                hitable.Hit(damage, sourcePlayerId);
            }
        }
        else
        {
            Transform bulletTrail = Instantiate(PrefabManager.Instance.TrailBullet, muzzlePos, Quaternion.identity).transform;
            bulletTrail.DOMove(ray.GetPoint(50f), BULLET_TRAIL_TIME);
        }
    }
}