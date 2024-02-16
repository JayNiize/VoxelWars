using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldZone : MonoBehaviour
{
    [Header("Zone Information")]
    [SerializeField] private List<ZoneSO> zoneSteps = new List<ZoneSO>();

    [SerializeField] private int zoneDamageSpeed = 2;

    private bool canZoneShrink = true;

    private float tempZoneDamageSpeed;
    private float tempZoneRestingTimer;

    private int currentZoneStep;
    private bool isPlayerInZone = true;
    private bool isZoneResting = false;

    private void Awake()
    {
        if (zoneSteps.Count == 0)
        {
            Debug.Log("NO ZONE STEP PROVIDED");
            return;
        }
        currentZoneStep = 0;
        transform.localScale = Vector3.one * zoneSteps[currentZoneStep].radius;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            isPlayerInZone = true;
            RenderSettings.skybox = PrefabManager.Instance.MaterialSkyboxNormal;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            isPlayerInZone = false;
            RenderSettings.skybox = PrefabManager.Instance.MaterialSkyboxOutsideZone;
        }
    }

    private void FixedUpdate()
    {
        if (!isPlayerInZone)
        {
            tempZoneDamageSpeed += Time.fixedDeltaTime;
            if (tempZoneDamageSpeed >= zoneDamageSpeed)
            {
                Player.Instance.RemoveHealth(zoneSteps[currentZoneStep].damage);
                tempZoneDamageSpeed = 0;
            }
        }
    }

    private void Update()
    {
        if (canZoneShrink)
        {
            if (!isZoneResting)
            {
                transform.localScale -= transform.localScale * (zoneSteps[currentZoneStep].shrinkSpeed / 10000f);

                if (zoneSteps[currentZoneStep] != zoneSteps[^1])
                {
                    float nextRadius = zoneSteps[currentZoneStep + 1].radius;
                    if (transform.localScale.magnitude <= (Vector3.one * nextRadius).magnitude)
                    {
                        isZoneResting = true;
                    }
                }
                if (transform.localScale.magnitude <= (Vector3.one * zoneSteps[^1].radius).magnitude)
                {
                    canZoneShrink = false;
                }
            }
        }

        if (isZoneResting)
        {
            tempZoneRestingTimer += Time.deltaTime;
            if (tempZoneRestingTimer >= zoneSteps[currentZoneStep].restingTimeInSeconds)
            {
                tempZoneRestingTimer = 0;
                currentZoneStep++;
                isZoneResting = false;
            }
        }
    }
}