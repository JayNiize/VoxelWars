using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera aimCamera;
    [SerializeField] private Camera minimapCamera;
    [SerializeField] private CinemachineVirtualCamera thirdPersonCamera;

    private float defaultFOV;
    private float defaultMinimapCameraY;
    public static CameraManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        defaultFOV = thirdPersonCamera.m_Lens.FieldOfView;
        aimCamera.m_Lens.FieldOfView = defaultFOV;
        defaultMinimapCameraY = minimapCamera.transform.position.y;
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

    internal void UpdateMinimapCamera(Vector3 pos)
    {
        minimapCamera.transform.position = new Vector3(pos.x, defaultMinimapCameraY, pos.z);
    }

    public void RegisterPlayer(Transform target)
    {
        aimCamera.Follow = target;
        thirdPersonCamera.Follow = target;
    }
}