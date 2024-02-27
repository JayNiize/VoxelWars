using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WorldZone : MonoBehaviour
{
    [Header("Zone Information")]
    [SerializeField] private List<ZoneSO> zoneSteps = new List<ZoneSO>();

    [SerializeField] private int zoneDamageSpeed = 2;

    private bool canZoneShrink = true;

    private float tempZoneDamageSpeed;
    private float tempZoneRestingTimer;

    private float TempZoneRestingTimer
    { get { return tempZoneRestingTimer; } set { tempZoneRestingTimer = value; OnChangeTimer.Invoke(tempZoneRestingTimer); } }

    private int currentZoneStep;
    private bool isPlayerInZone = true;

    private bool isZoneResting = false;

    private bool IsZoneResting
    { get { return isZoneResting; } set { isZoneResting = value; ; OnChangeIsResting.Invoke(isZoneResting); } }

    public UnityEvent<bool> OnChangeIsResting;
    public UnityEvent<float> OnChangeTimer;

    private void Awake()
    {
        if (zoneSteps.Count == 0)
        {
            Debug.Log("NO ZONE STEP PROVIDED");
            return;
        }
        currentZoneStep = 0;
        TempZoneRestingTimer = zoneSteps[currentZoneStep].restingTimeInSeconds;
        transform.localScale = Vector3.one * zoneSteps[currentZoneStep].radius;
    }

    private void Start()
    {
        GUIManager.Instance.RegisterWorldZone(this);
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
            if (!IsZoneResting)
            {
                transform.localScale -= transform.localScale * (zoneSteps[currentZoneStep].shrinkSpeed / 10000f);

                if (zoneSteps[currentZoneStep] != zoneSteps[^1])
                {
                    float nextRadius = zoneSteps[currentZoneStep + 1].radius;

                    float t = (transform.localScale - (Vector3.one * nextRadius)).magnitude / ((zoneSteps[currentZoneStep].shrinkSpeed / 10000f) * Time.deltaTime);
                    Debug.Log($"{t} seconds remaining");
                    if (transform.localScale.magnitude <= (Vector3.one * nextRadius).magnitude)
                    {
                        IsZoneResting = true;
                    }
                }
                if (transform.localScale.magnitude <= (Vector3.one * zoneSteps[^1].radius).magnitude)
                {
                    canZoneShrink = false;
                }
            }
        }

        if (IsZoneResting)
        {
            TempZoneRestingTimer -= Time.deltaTime;

            if (TempZoneRestingTimer <= 0)
            {
                TempZoneRestingTimer = zoneSteps[currentZoneStep].restingTimeInSeconds;
                currentZoneStep++;
                IsZoneResting = false;
            }
        }
    }
}