using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerController : MonoBehaviour
{
    private GameObject tacticalMarker;

    private void Start()
    {
        tacticalMarker = MarkerManager.Instance.RegisterMarkerController(this);
    }

    public void ShootWithMarker()
    {
        if (tacticalMarker.activeSelf)
        {
            tacticalMarker.SetActive(false);
            return;
        }
        Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenter);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~LayerMask.GetMask("Player")))
        {
            tacticalMarker.SetActive(true);
            tacticalMarker.transform.position = hit.point;
        }
    }
}