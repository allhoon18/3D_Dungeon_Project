using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [HideInInspector] public PlayerStat stat;
    private Rigidbody rigidbody;

    public Camera camera;
    private Vector2 prevMouseDelta;

    [SerializeField] float lookSentitivity;

    InputHandler inputHandler;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.controller = this;

        rigidbody = GetComponent<Rigidbody>();
        camera = Camera.main;

        inputHandler = GetComponent<InputHandler>();

        inputHandler.OnJump += OnJump; // Jump 이벤트에 메서드 연결

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

    void Move()
    {
        if (stat == null) return;

        Vector3 dir = transform.forward * inputHandler.movementInput.y + transform.right * inputHandler.movementInput.x;
        dir *= stat.speed;
        dir.y = rigidbody.velocity.y;

        rigidbody.AddForce(dir, ForceMode.Acceleration);
    }

    public void OnJump()
    {
        if(stat != null)
        {
            Jump(stat.jumpPower);
        }
        else
        {
            Debug.LogWarning("PlayerStat is missing");
            return;
        }
    }

    public void Jump(float power)
    {
        rigidbody.AddForce(Vector3.up * power, ForceMode.Impulse);
    }

    void CameraLook()
    {
        if (inputHandler.mouseDelta != prevMouseDelta)
        {
            transform.eulerAngles += new Vector3(0, inputHandler.mouseDelta.x * lookSentitivity, 0);
            camera.transform.eulerAngles += new Vector3(inputHandler.mouseDelta.y * lookSentitivity * -1, 0, 0);
            prevMouseDelta = inputHandler.mouseDelta;
        }
    }


}
