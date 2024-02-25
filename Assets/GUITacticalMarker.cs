using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUITacticalMarker : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI labelDistance;
    [SerializeField] private Image background;
    [SerializeField] private new ParticleSystem particleSystem;
    private ParticleSystem.MainModule ps;

    private void Awake()
    {
        ps = particleSystem.main;
        Color color = Color.red;
        ps.startColor = color;
        background.color = color;
        gameObject.SetActive(false);
    }

    private void Update()
    {
        int distance = (int)(Vector3.Distance(transform.position, Player.Instance.transform.position));
        labelDistance.text = distance.ToString() + "m";
        background.transform.position = CameraManager.Instance.GetMainCamera().WorldToScreenPoint(transform.position);
    }
}