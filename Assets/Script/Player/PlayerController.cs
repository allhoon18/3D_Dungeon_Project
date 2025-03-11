using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public PlayerStat stat;
    private Rigidbody rigidbody;

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

        inputHandler.OnJump += OnJump; // Jump �̺�Ʈ�� �޼��� ����

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
            // �÷����� ��ġ�� �������� ������ ó��
            transform.position += platformTransform.position - prevPlatformPosition;
            prevPlatformPosition = platformTransform.position;
        }

        //�ȱ� or �޸��� �ӵ� ����
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
