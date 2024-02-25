using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldTreasureChest : MonoBehaviour, IActionable
{
    [SerializeField] private float openRotation = -140f;
    [SerializeField] private float openDuration = 1f;
    [SerializeField] private Transform treasureTop;

    private int minItemsToDrop = 2;
    private int maxItemsToDrop = 5;
    private bool isOpen = false;

    public void ExecuteAction()
    {
        if (isOpen)
        {
            return;
        }
        isOpen = true;
        treasureTop.DOLocalRotate(new Vector3(0, 0, openRotation), openDuration).SetEase(Ease.OutBounce);

        for (int i = 0; i < Random.Range(minItemsToDrop, maxItemsToDrop - 1); i++)
        {
            WeaponRarity weaponRarity = LootManager.Instance.CalculateWeaponRarity();
            List<WeaponSO> possibleWeapons = ItemManager.Instance.GetAllWeaponsByRarity(weaponRarity);
            WeaponSO weaponToSpawn = possibleWeapons[Random.Range(0, possibleWeapons.Count - 1)];

            WorldWeapon worldWeapon = Instantiate(weaponToSpawn.weaponPrefab, transform.position + Vector3.left, Quaternion.identity).GetComponent<WorldWeapon>();
            worldWeapon.SetWeaponInformation(weaponToSpawn);
        }
    }

    public void HideActionInfo()
    {
    }

    public void ShowActionInfo()
    {
    }
}