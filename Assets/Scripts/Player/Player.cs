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

    private NetworkObject networkObject;

    public static Player Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        CurrentHealth = MaxHealth;
        networkObject = GetComponent<NetworkObject>();
    }

    public override void OnNetworkSpawn()
    {
        gameObject.layer = IsOwner ? 3 : 7;
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
            networkObject.Despawn();
        }
    }

    public void Hit(int damage, string sourcePlayerId)
    {
        RemoveHealth(damage);
    }
}