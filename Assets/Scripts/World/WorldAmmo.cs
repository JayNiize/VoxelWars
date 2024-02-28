using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldAmmo : MonoBehaviour, IPickupable
{
    [Header("Weapon")]
    [SerializeField] private AmmoSO ammoSO;

    [SerializeField] private int ammoAmount;

    private GUIWorldWeaponPanel infoPanel;

    public AmmoSO GeAmmoInformation()
    { return ammoSO; }

    private void Start()
    {
        transform.DORotate(new Vector3(0, 360, 0), 5, RotateMode.FastBeyond360).SetLoops(-1);
        transform.DOLocalMoveY(0.25f, 5f).SetLoops(-1, LoopType.Yoyo);
    }

    public void ShowActionInfo()
    {
        infoPanel = Instantiate(PrefabManager.Instance.GuiWeaponInformationPanel, Vector3.up, Quaternion.identity, transform).GetComponent<GUIWorldWeaponPanel>();
        infoPanel.Setup(ammoSO, ammoAmount);
    }

    public void HideActionInfo()
    {
        infoPanel.Hide();
    }

    public void ExecuteAction()
    {
        Player.Instance.InventoryController.AddToInventory(ammoSO, ammoAmount);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        transform.DOKill();
    }
}