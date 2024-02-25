using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour, IHealth, IHitable
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
            Debug.Log("you deeead boiii :(");
        }
    }

    public void Hit(int damage, Transform hitSource)
    {
        throw new System.NotImplementedException();
    }
}