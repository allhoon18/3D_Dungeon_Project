using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    //InputAction을 통한 키 입력을 처리
    private PlayerInput input;
    private InputActionMap playerActionMap;
    public Vector2 movementInput { get; private set; }
    public Vector2 mouseDelta { get; private set; }
    public bool isRun { get; private set; }
    public bool isUse { get; private set; }
    public bool cameraChange { get; private set; }

    public event Action OnJump;

    void Start()
    {
        input = GetComponent<PlayerInput>();
        playerActionMap = input.actions.FindActionMap("Player");

        InitInputActions();
    }

    void InitInputActions()
    {
        InputAction moveAction = playerActionMap.FindAction("Move");
        moveAction.performed += context => movementInput = context.ReadValue<Vector2>();
        moveAction.canceled += context => movementInput = Vector2.zero;

        InputAction jumpAction = playerActionMap.FindAction("Jump");
        jumpAction.started += context => OnJump?.Invoke();

        InputAction lookAction = playerActionMap.FindAction("Mouse");
        lookAction.performed += context => mouseDelta = context.ReadValue<Vector2>();

        InputAction runAction = playerActionMap.FindAction("Run");
        runAction.performed += context => isRun = true;
        runAction.canceled += context => isRun = false;

        InputAction useAction = playerActionMap.FindAction("Use");
        useAction.performed += context => isUse = true;
        useAction.canceled += context => isUse = false;

        InputAction cameraAction = playerActionMap.FindAction("CameraChange");
        cameraAction.started += context => cameraChange = !cameraChange;
    }
}
