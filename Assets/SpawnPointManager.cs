using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SpawnPointManager : NetworkBehaviour
{
    private List<Vector3> possibleSpawnPoints = new List<Vector3>();

    public static SpawnPointManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        for (int i = 0; i < transform.childCount - 1; i++)
        {
            possibleSpawnPoints.Add(transform.GetChild(i).position);
        }
    }

    public Vector3 GetSpawnPosition()
    {
        int index = Random.Range(0, possibleSpawnPoints.Count);
        Vector3 pos = possibleSpawnPoints[index];
        possibleSpawnPoints.RemoveAt(index);
        return pos;
    }
}