using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerManager : MonoBehaviour
{
    public Dictionary<MarkerController, GameObject> tacticalMarkers = new Dictionary<MarkerController, GameObject>();
    public static MarkerManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public GameObject RegisterMarkerController(MarkerController controller)
    {
        GameObject marker = Instantiate(PrefabManager.Instance.ParticlesTacticalMarker, transform);
        tacticalMarkers.Add(controller, marker);
        return marker;
    }
}