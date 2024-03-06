using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public const string MOUSE_SPEED = "mouseSpeed";

    [SerializeField] private Slider mouseSensitivitySlider;

    public static SettingsManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        mouseSensitivitySlider.value = PlayerPrefs.GetFloat(MOUSE_SPEED, 0f);
    }

    public void SetMouseSpeed(float value)
    {
        PlayerPrefs.SetFloat(MOUSE_SPEED, value);
        Debug.Log($"Set Speed to {value}");
    }

    public void GetMouseSpeed()
    {
        PlayerPrefs.GetFloat(MOUSE_SPEED);
    }
}