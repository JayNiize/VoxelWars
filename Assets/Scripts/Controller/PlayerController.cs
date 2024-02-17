using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerController : NetworkBehaviour
{
    [Header("Player Properties")]
    [SerializeField] private float playerSpeed = 5f;

    [SerializeField] private float playerTurnSmoothness = 0.05f;

    [Header("Player Input")]
    [SerializeField] private InputActionAsset playerInput;

    private InputAction moveAction;
    private InputAction attackAction;
    private InputAction lookAction;
    private InputAction aimAction;
    private InputAction actionAction;

    private Rigidbody rb;
    private Transform cam;
    private WeaponController weaponController;
    private Animator anim;
    [SerializeField] private PlayerPickup playerPickup;

    // STATE HANDLING
    private Vector2 moveInput;

    private bool isShooting;

    private float curVelocity;

    //Cinemachine
    [Header("Cinemachine")]
    [SerializeField] private Transform cinemachineCameraTarget;

    private float _cinemachineTargetYaw;

    private float _cinemachineTargetPitch;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            this.enabled = false;
        }
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main.transform;
        weaponController = GetComponent<WeaponController>();
        anim = GetComponentInChildren<Animator>();

        moveAction = playerInput.FindAction("Move");
        moveAction.started += OnMovementStarted;
        moveAction.canceled += OnMovementStopped;

        attackAction = playerInput.FindAction("Attack");
        attackAction.started += OnAttackStarted;
        attackAction.canceled += OnAttackStopped;

        aimAction = playerInput.FindAction("Aim");
        aimAction.started += OnAimStarted;
        aimAction.canceled += OnAimStopped;

        lookAction = playerInput.FindAction("Look");

        actionAction = playerInput.FindAction("Action");
        actionAction.started += OnActionStarted;
        actionAction.canceled += OnActionStopped;
        Cursor.lockState = CursorLockMode.Locked;
        CameraManager.Instance.RegisterPlayer(cinemachineCameraTarget);
    }

    private void Update()
    {
        HandleAttack();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void LateUpdate()
    {
        HandleCameraRotation();
    }

    private void HandleAttack()
    {
        if (isShooting)
        {
            weaponController.ShootWithWeapon();
        }
    }

    private void HandleMovement()
    {
        moveInput = moveAction.ReadValue<Vector2>();
        if (moveInput != Vector2.zero)
        {
            float targetAngle = CalculatePlayerRotation();
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref curVelocity, playerTurnSmoothness);
            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            rb.Move(rb.position + (moveDirection * playerSpeed * Time.fixedDeltaTime), Quaternion.Euler(0, angle, 0));
        }
        else
        {
            rb.MoveRotation(Quaternion.Euler(0, cam.eulerAngles.y, 0));
        }
    }

    private float CalculatePlayerRotation()
    {
        Vector3 dir = new Vector3(moveInput.x, 0f, moveInput.y).normalized;
        return Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
    }

    private void OnMovementStarted(InputAction.CallbackContext ctx)
    {
        anim.SetBool("IsWalking", true);
    }

    private void OnMovementStopped(InputAction.CallbackContext ctx)
    {
        anim.SetBool("IsWalking", false);
    }

    private void OnAttackStarted(InputAction.CallbackContext ctx)
    {
        isShooting = true;
        anim.SetBool("IsShooting", true);
    }

    private void OnAttackStopped(InputAction.CallbackContext ctx)
    {
        isShooting = false;
        anim.SetBool("IsShooting", false);
    }

    private void OnActionStarted(InputAction.CallbackContext ctx)
    {
        playerPickup.Pickup();
    }

    private void OnActionStopped(InputAction.CallbackContext ctx)
    {
    }

    private void OnAimStarted(InputAction.CallbackContext ctx)
    {
        if (weaponController.HasWeaponEquipped())
        {
            CameraManager.Instance.SetAimCamera(true, weaponController.CurrentWeapon.weaponScopeStrength);
        }
    }

    private void OnAimStopped(InputAction.CallbackContext ctx)
    {
        if (weaponController.HasWeaponEquipped())
        {
            CameraManager.Instance.SetAimCamera(false, 0);
        }
    }

    private void HandleCameraRotation()
    {
        Vector2 _input = lookAction.ReadValue<Vector2>();
        if (_input.sqrMagnitude >= 0)
        {
            //Don't multiply mouse input by Time.deltaTime;
            float deltaTimeMultiplier = 1.0f;

            _cinemachineTargetYaw += _input.x * deltaTimeMultiplier;
            _cinemachineTargetPitch += _input.y * deltaTimeMultiplier;
        }

        // clamp our rotations so our values are limited 360 degrees
        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, -30f, 70f);

        // Cinemachine will follow this target
        cinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch,
            _cinemachineTargetYaw, 0.0f);
    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
}