using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldEntity : MonoBehaviour, IHitable, IHealth
{
    [SerializeField] private GameObject destroyedPrefab;
    [SerializeField] private int maxHealth;

    public int MaxHealth
    { get { return maxHealth; } set { maxHealth = value; } }

    private int currHealth;

    public int CurrentHealth
    { get { return currHealth; } set { currHealth = value; } }

    private void Awake()
    {
        CurrentHealth = MaxHealth;
    }

    public void Hit(int damage, string sourcePlayerId)
    {
        RemoveHealth(damage);
    }

    public void AddHealth(int health)
    {
        throw new System.NotImplementedException();
    }

    public void RemoveHealth(int health)
    {
        CurrentHealth -= health;
        if (CurrentHealth <= 0)
        {
            if (destroyedPrefab != null)
                Instantiate(destroyedPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}