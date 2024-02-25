using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PrefabManager : MonoBehaviour
{
    [Header("Particles")]
    public GameObject ParticlesShooting;

    public GameObject ParticlesHit;
    public GameObject ParticlesTacticalMarker;

    [Header("Trails")]
    public GameObject TrailBullet;

    [Header("GUI Elements")]
    public GameObject GuiWeaponInformationPanel;

    [Header("Materials")]
    public Material MaterialSkyboxNormal;

    public Material MaterialSkyboxOutsideZone;
    public static PrefabManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
}