using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class Player : NetworkBehaviour, IHealth, IHitable
{
    private int maxHealth = 100;

    public int MaxHealth
    { get { return maxHealth; } set { maxHealth = value; } }

    private int currHealth;

    public int CurrentHealth
    { get { return currHealth; } set { currHealth = value; OnCurrentHealthChange.Invoke(currHealth, MaxHealth); } }

    public UnityEvent<int, int> OnCurrentHealthChange = new UnityEvent<int, int>();

    private InventoryController inventoryController;

    public InventoryController InventoryController
    { get { return inventoryController; } }

    private WeaponController weaponController;

    public WeaponController WeaponController
    { get { return weaponController; } }

    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private BoxCollider col;

    public static Player Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        CurrentHealth = MaxHealth;
        inventoryController = GetComponent<InventoryController>();
        weaponController = GetComponent<WeaponController>();
    }

    public override void OnNetworkSpawn()
    {
        gameObject.layer = IsOwner ? 3 : 8;
        if (!IsOwner)
        {
            this.enabled = false;
        }
    }

    private void Start()
    {
        GUIManager.Instance.RegisterPlayer(this);
    }

    public void AddHealth(int health)
    {
        throw new System.NotImplementedException();
    }

    public void RemoveHealth(int health)
    {
        Debug.Log($"Removed {health} health");
        CurrentHealth -= health;
        if (CurrentHealth <= 0)
        {
            CurrentHealth = MaxHealth;
            inventoryController.RemoveEverything();
            StartCoroutine(SavePlayerAfterDeath());
        }
    }

    private IEnumerator SavePlayerAfterDeath()
    {
        meshRenderer.enabled = false;
        col.enabled = false;
        yield return new WaitForSeconds(3);
        transform.position = SpawnPointManager.Instance.GetSpawnPosition(false);
        meshRenderer.enabled = true;
        col.enabled = true;
    }

    public void Hit(int damage, Transform hitSource)
    {
        HitClientRpc(damage);
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void HitClientRpc(int damage)
    {
        RemoveHealth(damage);
    }
}