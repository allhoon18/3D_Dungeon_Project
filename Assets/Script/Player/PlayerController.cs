using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public PlayerStat stat;
    private Rigidbody rigidbody;

    //ī�޶� ȸ�� ����
    [Header("Rotate Camera")]
    public Camera camera;
    [SerializeField] float lookSentitivity;
    [SerializeField] float maxRotVertical;
    [SerializeField] float minRotVertical;
    private Vector2 prevMouseDelta;
    private float cameraCurRot;

    //Ű �Է� ó��
    InputHandler inputHandler;
    //�ִϸ��̼� �۵�
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
        //Rigidbody, Camera �Ҵ�
        rigidbody = GetComponent<Rigidbody>();
        camera = Camera.main;

        inputHandler = GetComponent<InputHandler>();

        animationHandler = GetComponent<AnimationHandler>();

        inputHandler.OnJump += OnJump; // Jump �̺�Ʈ�� �޼��� ����

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
        //Ű �Է� ���� ����
        Vector3 dir = transform.forward * inputHandler.movementInput.y + transform.right * inputHandler.movementInput.x;
        //�ȱ� or �޸��� �ӵ� ����
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
