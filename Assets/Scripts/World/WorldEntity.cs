using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class WorldEntity : NetworkBehaviour, IHitable, IHealth
{
    [SerializeField] private GameObject destroyedPrefab;
    [SerializeField] private int maxHealth;

    private MeshFilter tempMesh;

    public int MaxHealth
    { get { return maxHealth; } set { maxHealth = value; } }

    private int currHealth;

    public int CurrentHealth
    { get { return currHealth; } set { currHealth = value; } }

    private void Awake()
    {
        CurrentHealth = MaxHealth;

        tempMesh = GetComponentInChildren<MeshFilter>();
    }

    public void Hit(int damage, Transform hitSource)
    {
        RemoveHealth(damage);
    }

    public void AddHealth(int health)
    {
        throw new System.NotImplementedException();
    }

    public void RemoveHealth(int health)
    {
        RemoveHealthClientRpc(health);
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void RemoveHealthClientRpc(int health)
    {
        CurrentHealth -= health;
        if (CurrentHealth <= 0)
        {
            if (destroyedPrefab != null)
                Instantiate(destroyedPrefab, transform.position, Quaternion.identity);
            CameraShake.Instance.Shake(0.25f, 0.1f);
            Destroy(gameObject);
        }
    }
}