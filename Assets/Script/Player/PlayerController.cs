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
        rigidbody = GetComponent<Rigidbody>();

        inputHandler = GetComponent<InputHandler>();

        animationHandler = GetComponent<AnimationHandler>();

        inputHandler.OnJump += OnJump; // Jump 이벤트에 메서드 연결

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        //점프 상태일 때는 낙하 애니메이션을 재생하지 않음
        if (isJumping) return;

        if (!isGroud)
            animationHandler.ActiveAnimation(AnimationStatus.Fall);
        else
            animationHandler.ActiveAnimation(AnimationStatus.Land);
    }

    private void FixedUpdate()
    {
        //지면에 있는지 여부를 확인
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

        //달리고 있을 때 스태미나를 사용함
        if (moveSpeed == stat.runSpeed)
            stat.AddOrSubtractStat(StatType.Stamina, stat.staminaUsageForRunning);


        animationHandler.ActiveAnimation(AnimationStatus.Walk, dir.magnitude);
        animationHandler.ActiveAnimation(AnimationStatus.Run, moveSpeed);

        //움직임을 적용
        rigidbody.AddForce(dir * moveSpeed, ForceMode.Force);

    }

    public void OnJump()
    {
        if (stat == null && stat.stamina < stat.staminaUsageForJump) return;
        //키 입력을 통해 점프시 스탯의 jumpPower를 사용함
        Jump(stat.jumpPower);
    }

    //외부에서 점프를 실행할 때는 힘을 입력할 수 있게함(ex: 점프대)
    public void Jump(float power)
    {
        //지면에 있을 때만 점프를 실행
        if (!isGroud) return;

        rigidbody.velocity = new Vector3(rigidbody.velocity.x, 0, rigidbody.velocity.z);

        rigidbody.AddForce(transform.up * power, ForceMode.Impulse);

        animationHandler.ActiveAnimation(AnimationStatus.Jump);

        isJumping = true;

        stat.AddOrSubtractStat(StatType.Stamina, stat.staminaUsageForJump);
        //지면에 닿았는지 여부와 관계없이 일정 시간이 지나면 점프 상태를 해제
        StartCoroutine(EndJump());
    }

    IEnumerator EndJump()
    {
        yield return new WaitForSeconds(jumpDuration);

        if(isJumping)
            isJumping = false;
    }

    //입력에 따라 걷기 또는 달리기 속도를 적용
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
