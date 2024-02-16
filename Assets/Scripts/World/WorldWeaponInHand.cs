using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldWeaponInHand : MonoBehaviour
{
    private WeaponSO weaponSO;
    private Transform muzzleTransform;

    private void Awake()
    {
        WorldWeapon worldWeapon = GetComponent<WorldWeapon>();
        weaponSO = worldWeapon.GetWeaponInformation();
        muzzleTransform = worldWeapon.GetMuzzleTransform();
        Destroy(worldWeapon);
        Destroy(GetComponent<BoxCollider>());
    }

    public WeaponSO GetWeaponInformation()
    { return weaponSO; }

    public Vector3 GetMuzzlePosition()
    {
        return muzzleTransform.position;
    }
}