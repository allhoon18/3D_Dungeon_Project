using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public PlayerStat stat;
    private Rigidbody rigidbody;

    //카메라 회전 정보
    [Header("Rotate Camera")]
    public Camera camera;
    [SerializeField] float lookSentitivity;
    [SerializeField] float maxRotVertical;
    [SerializeField] float minRotVertical;
    private Vector2 prevMouseDelta;
    private float cameraCurRot;

    //키 입력 처리
    InputHandler inputHandler;
    //애니메이션 작동
    AnimationHandler animationHandler;

    [Header("Check Ground")]
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float distanceToGround;
    [SerializeField] bool isGroud;

    [Header("Jump")]
    [SerializeField] bool isJumping;
    [SerializeField] float jumpDuration;

    float moveSpeed;

    void Start()
    {
        //Rigidbody, Camera 할당
        rigidbody = GetComponent<Rigidbody>();
        camera = Camera.main;

        inputHandler = GetComponent<InputHandler>();

        animationHandler = GetComponent<AnimationHandler>();

        inputHandler.OnJump += OnJump; // Jump 이벤트에 메서드 연결

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {

        //if(previsGround != isGroud)
        //{
        //    if(isGroud)
        //        animationHandler.ActiveAnimation(AnimationStatus.Land);
        //    else
        //        animationHandler.ActiveAnimation(AnimationStatus.Fall);

        //    previsGround = isGroud;
        //}

        if (isJumping) return;

        if (!isGroud)
            animationHandler.ActiveAnimation(AnimationStatus.Fall);
        else
            animationHandler.ActiveAnimation(AnimationStatus.Land);
    }

    private void FixedUpdate()
    {
        isGroud = IsGround();

        Move();
    }

    private void LateUpdate()
    {
        CameraLook();
    }

    void Move()
    {
        if (stat == null) return;
        //키 입력 값을 적용
        Vector3 dir = transform.forward * inputHandler.movementInput.y + transform.right * inputHandler.movementInput.x;
        //걷기 or 달리기 속도 적용
        moveSpeed = SetSpeed();
        dir *= moveSpeed;
        dir.y = rigidbody.velocity.y;

        rigidbody.AddForce(dir, ForceMode.Acceleration);

        animationHandler.ActiveAnimation(AnimationStatus.Walk, dir.magnitude);
        animationHandler.ActiveAnimation(AnimationStatus.Run, moveSpeed);
    }

    public void OnJump()
    {
        if (stat == null) return;

        Jump(stat.jumpPower);
    }

    public void Jump(float power)
    {
        if (!isGroud) return;

        rigidbody.AddForce(Vector3.up * power, ForceMode.Impulse);

        animationHandler.ActiveAnimation(AnimationStatus.Jump);

        isJumping = true;

        StartCoroutine(EndJump());
    }

    IEnumerator EndJump()
    {
        yield return new WaitForSeconds(jumpDuration);

        if(isJumping)
            isJumping = false;
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

    bool IsGround()
    {
        Ray ray = new Ray(transform.position + Vector3.up * 0.1f, Vector3.down);

        if(Physics.Raycast(ray, distanceToGround, groundLayer))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + Vector3.up * 0.2f,
            transform.position + Vector3.up * 0.2f + Vector3.down * distanceToGround);
    }


}
