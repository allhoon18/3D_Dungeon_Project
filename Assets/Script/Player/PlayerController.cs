using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerStat stat;
    private Rigidbody rigidbody;
    private PlayerInput input;
    private InputActionMap playerActionMap;
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction lookAction;
    private InputAction.CallbackContext context;
    private Vector3 movementInput;
    
    private Camera camera;
    private Vector2 mouseDelta;
    private Vector2 prevMouseDelta;

    [SerializeField] float lookSentitivity;


    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.controller = this;

        stat = GetComponent<PlayerStat>();
        rigidbody = GetComponent<Rigidbody>();
        camera = Camera.main;
        InitInput();

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void LateUpdate()
    {
        CameraLook();
    }

    void InitInput()
    {
        input = GetComponent<PlayerInput>();
        playerActionMap = input.actions.FindActionMap("Player");

        InitMove();
        InitJump();
        InitLook();
        
    }

    void InitMove()
    {
        moveAction = playerActionMap.FindAction("Move");

        moveAction.performed += context =>
        {
            movementInput = context.ReadValue<Vector2>();
        };

        moveAction.canceled += context =>
        {
            //movementInput = Vector3.zero;
            //Vector3 currentVelocity = rigidbody.velocity;
            //Debug.Log(currentVelocity.magnitude * 0.01f.RoundToDecimalPlaces(2));
            //Vector3 newVelocity = Vector3.SmoothDamp(rigidbody.velocity, Vector3.zero, ref currentVelocity, currentVelocity.magnitude * 0.01f.RoundToDecimalPlaces(2));
            //rigidbody.velocity = newVelocity;

            movementInput = Vector3.zero;
            rigidbody.velocity = Vector3.zero;
        };
    }

    void InitJump()
    {
        jumpAction = playerActionMap.FindAction("Jump");

        jumpAction.started += context =>
        {
            Jump();
        };
    }

    void InitLook()
    {
        lookAction = playerActionMap.FindAction("Mouse");

        lookAction.performed  += context =>
        {
            mouseDelta = context.ReadValue<Vector2>();

        };

    }

    void Move()
    {
        Vector3 dir = transform.forward * movementInput.y + transform.right * movementInput.x;
        dir *= stat.speed;
        dir.y = rigidbody.velocity.y;

        rigidbody.AddForce(dir, ForceMode.Acceleration);
    }

    void Jump()
    {
        rigidbody.AddForce(Vector3.up * stat.jumpPower, ForceMode.Impulse);
    }

    void CameraLook()
    {
        if(mouseDelta != prevMouseDelta)
        {
            transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSentitivity, 0);
            camera.transform.eulerAngles += new Vector3(mouseDelta.y * lookSentitivity * -1, 0, 0);
            prevMouseDelta = mouseDelta;
        }
    }


}
