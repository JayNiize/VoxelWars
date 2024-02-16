using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Zone Step", menuName = "Custom/Zone")]
public class ZoneSO : ScriptableObject
{
    public int damage;
    public float shrinkSpeed;
    public float radius;
    public float restingTimeInSeconds;
}