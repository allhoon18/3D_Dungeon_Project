using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public PlayerStat stat;
    private Rigidbody rigidbody;

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

    bool isMoveOnPlatform;
    Transform platformTransform;
    Vector3 prevPlatformPosition;

    float moveSpeed;

    void Start()
    {
        //Rigidbody
        rigidbody = GetComponent<Rigidbody>();

        inputHandler = GetComponent<InputHandler>();

        animationHandler = GetComponent<AnimationHandler>();

        inputHandler.OnJump += OnJump; // Jump 이벤트에 메서드 연결

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
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

    void Move()
    {
        if (stat == null) return;

        Vector3 dir = transform.forward * inputHandler.movementInput.y + transform.right * inputHandler.movementInput.x;

        if (isMoveOnPlatform && prevPlatformPosition != null)
        {
            // 플랫폼의 위치를 기준으로 움직임 처리
            transform.position += platformTransform.position - prevPlatformPosition;
            prevPlatformPosition = platformTransform.position;
        }

        //걷기 or 달리기 속도 적용
        moveSpeed = SetSpeed();
        dir.y = rigidbody.velocity.y;

        if (moveSpeed == stat.runSpeed)
            stat.AddOrSubtractStat(StatType.Stamina, stat.staminaUsageForRunning);


        animationHandler.ActiveAnimation(AnimationStatus.Walk, dir.magnitude);
        animationHandler.ActiveAnimation(AnimationStatus.Run, moveSpeed);

        rigidbody.AddForce(dir * moveSpeed, ForceMode.Force);

    }

    public void OnJump()
    {
        if (stat == null && stat.stamina < stat.staminaUsageForJump) return;
        Debug.Log("Jump");
        Jump(stat.jumpPower);
    }

    public void Jump(float power)
    {
        if (!isGroud) return;

        rigidbody.velocity = new Vector3(rigidbody.velocity.x, 0, rigidbody.velocity.z);

        rigidbody.AddForce(transform.up * power, ForceMode.Impulse);

        animationHandler.ActiveAnimation(AnimationStatus.Jump);

        isJumping = true;

        stat.AddOrSubtractStat(StatType.Stamina, stat.staminaUsageForJump);

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
        if (inputHandler.isRun && stat.stamina > stat.staminaUsageForRunning)
        {
            return stat.runSpeed;
        }  
        else
            return stat.walkSpeed;
    }

    bool IsGround()
    {
        Ray ray = new Ray(transform.position + Vector3.up * 0.1f, Vector3.down);

        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, distanceToGround, groundLayer))
        {
            if(hit.collider.gameObject.layer == LayerMask.NameToLayer("Platform"))
            {
                isMoveOnPlatform = true;
                platformTransform = hit.transform;
            }
            else
            {
                isMoveOnPlatform = false;
            }

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
