using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ammo", menuName = "Custom/Ammo")]
public class AmmoSO : ScriptableObject
{
    public string ammoName;
    public Sprite ammoSprite;
    public GameObject ammoPrefab;
}