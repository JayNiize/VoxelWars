using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class WorldTreasureChest : NetworkBehaviour, IActionable
{
    [SerializeField] private float openRotation = -140f;
    [SerializeField] private float openDuration = 1f;
    [SerializeField] private Transform treasureTop;

    [SerializeField] private GameObject treasureLight;
    [SerializeField] private GameObject treasureParticles;
    private GUIWorldWeaponPanel infoPanel;

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
        treasureTop.DOLocalRotate(new Vector3(openRotation, 0, 0), openDuration).SetEase(Ease.OutBounce);
        if (infoPanel != null)
        {
            infoPanel.Hide();
        }

        treasureLight.SetActive(false);
        treasureParticles.gameObject.SetActive(false);

        for (int i = 0; i < Random.Range(minItemsToDrop, maxItemsToDrop - 1); i++)
        {
            RarityEnum weaponRarity = LootManager.Instance.CalculateWeaponRarity();
            List<WeaponSO> possibleWeapons = ItemManager.Instance.GetAllWeaponsByRarity(weaponRarity);
            WeaponSO weaponToSpawn = possibleWeapons[Random.Range(0, possibleWeapons.Count)];

            WorldWeapon worldWeapon = Instantiate(weaponToSpawn.Prefab, transform.position + transform.forward + (transform.right * Random.Range(-2.00f, 2.00f)), Quaternion.identity).GetComponent<WorldWeapon>();
            worldWeapon.SetWeaponInformation(weaponToSpawn);
        }
    }

    public void ShowActionInfo()
    {
        if (isOpen)
        {
            return;
        }
        infoPanel = Instantiate(PrefabManager.Instance.GuiWeaponInformationPanel, Vector3.up, Quaternion.identity, transform).GetComponent<GUIWorldWeaponPanel>();
        infoPanel.Setup(UsedColors.TreasureColor, "Open Treasure Chest");
    }

    public void HideActionInfo()
    {
        if (isOpen)
        {
            return;
        }
        infoPanel.Hide();
    }
}