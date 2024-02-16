using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera aimCamera;
    [SerializeField] private CinemachineVirtualCamera thirdPersonCamera;

    private float defaultFOV;
    public static CameraManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        defaultFOV = thirdPersonCamera.m_Lens.FieldOfView;
        aimCamera.m_Lens.FieldOfView = defaultFOV;
    }

    public void SetAimCamera(bool isActive, float weaponScope)
    {
        if (weaponScope > 0)
        {
            aimCamera.m_Lens.FieldOfView = defaultFOV / weaponScope;
        }

        aimCamera.gameObject.SetActive(isActive);
        thirdPersonCamera.gameObject.SetActive(!isActive);
    }

    public Camera GetMainCamera()
    {
        return Camera.main;
    }
}