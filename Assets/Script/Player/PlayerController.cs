using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public PlayerStat stat;
    private Rigidbody rigidbody;

    public Camera camera;
    private Vector2 prevMouseDelta;

    [SerializeField] float lookSentitivity;
    [SerializeField] float maxRotVertical;
    [SerializeField] float minRotVertical;

    float cameraCurRot;

    InputHandler inputHandler;

    AnimationHandeler animationHandeler;

    float moveSpeed;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        camera = Camera.main;

        inputHandler = GetComponent<InputHandler>();

        animationHandeler = GetComponent<AnimationHandeler>();

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

        moveSpeed = SetSpeed();

        dir *= moveSpeed;
        dir.y = rigidbody.velocity.y;

        rigidbody.AddForce(dir, ForceMode.Acceleration);

        animationHandeler.ActiveAnimation(AnimationStatus.Walk, dir.magnitude);
        animationHandeler.ActiveAnimation(AnimationStatus.Run, moveSpeed);
    }

    public void OnJump()
    {
        if (stat == null) return;
        
        Jump(stat.jumpPower);
    }

    public void Jump(float power)
    {
        rigidbody.AddForce(Vector3.up * power, ForceMode.Impulse);

        animationHandeler.ActiveAnimation(AnimationStatus.Jump);
    }

    public float SetSpeed()
    {
        if (inputHandler.isRun)
        {
            return stat.runSpeed;
        }  
        else
            return stat.walkSpeed;
    }

    void CameraLook()
    {
        if (inputHandler.mouseDelta != prevMouseDelta)
        {
            transform.localEulerAngles += new Vector3(0, inputHandler.mouseDelta.x * lookSentitivity, 0);

            cameraCurRot += inputHandler.mouseDelta.y * lookSentitivity;

            cameraCurRot = Mathf.Clamp(cameraCurRot, minRotVertical, maxRotVertical);

            camera.transform.localEulerAngles = new Vector3(-cameraCurRot, 0, 0);

            prevMouseDelta = inputHandler.mouseDelta;
        }
    }


}
