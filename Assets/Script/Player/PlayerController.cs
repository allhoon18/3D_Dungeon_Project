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
    private InputAction.CallbackContext context;
    Vector3 movementInput;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.controller = this;

        stat = GetComponent<PlayerStat>();
        rigidbody = GetComponent<Rigidbody>();
        InitInput();
    }

    private void FixedUpdate()
    {
        Move();
    }

    void InitInput()
    {
        input = GetComponent<PlayerInput>();
        playerActionMap = input.actions.FindActionMap("Player");

        InitMove();
        InitJump();
        
    }

    void InitMove()
    {
        moveAction = playerActionMap.FindAction("Move");

        moveAction.performed += context =>
        {
            Vector2 dir = context.ReadValue<Vector2>();
            Vector3 moveDir = new Vector3(dir.x, 0, dir.y);
            movementInput = moveDir;
        };

        moveAction.canceled += context =>
        {
            movementInput = Vector3.zero;
            Vector3 currentVelocity = rigidbody.velocity;
            Debug.Log(currentVelocity.magnitude * 0.01f.RoundToDecimalPlaces(2));
            Vector3 newVelocity = Vector3.SmoothDamp(rigidbody.velocity, Vector3.zero, ref currentVelocity, currentVelocity.magnitude * 0.01f.RoundToDecimalPlaces(2));
            rigidbody.velocity = newVelocity;
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

    void Move()
    {
        Vector3 dir = movementInput.normalized;
        dir *= stat.speed;
        dir.y = rigidbody.velocity.y;

        rigidbody.AddForce(dir, ForceMode.Acceleration);
    }

    void Jump()
    {
        rigidbody.AddForce(Vector3.up * stat.jumpPower, ForceMode.Impulse);
    }


}
