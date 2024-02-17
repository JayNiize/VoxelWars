using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class WorldWeapon : NetworkBehaviour, IPickupable
{
    [Header("Weapon")]
    [SerializeField] private WeaponSO weaponSO;

    [SerializeField] private Transform muzzleTransform;
    [SerializeField] private Transform meshTransform;

    private GUIWorldWeaponPanel infoPanel;

    public WeaponSO GetWeaponInformation()
    { return weaponSO; }

    private void Start()
    {
        meshTransform.DORotate(new Vector3(0, 360, 0), 5, RotateMode.FastBeyond360).SetLoops(-1);
        meshTransform.DOLocalMoveY(0.25f, 5f).SetLoops(-1, LoopType.Yoyo);
    }

    public Vector3 GetMuzzlePosition()
    {
        return muzzleTransform.position;
    }

    public Transform GetMuzzleTransform()
    {
        return muzzleTransform;
    }

    public void ShowPickupInfo()
    {
        infoPanel = Instantiate(PrefabManager.Instance.GuiWeaponInformationPanel, Vector3.up, Quaternion.identity, transform).GetComponent<GUIWorldWeaponPanel>();
        infoPanel.Setup(weaponSO);
    }

    public void HidePickupInfo()
    {
        infoPanel.Hide();
    }
}