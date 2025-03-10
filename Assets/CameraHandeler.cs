using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandeler : MonoBehaviour
{
    public Transform playerTransform;
    public InputHandler inputHandler;
    public Camera camera;

    public bool isFPSMode;

    //ī�޶� ȸ�� ����
    [Header("FPS Info")]
    [SerializeField] float lookSentitivity;
    [SerializeField] float maxRotVertical;
    [SerializeField] float minRotVertical;

    [Header("TPS Info")]
    [SerializeField] float TPSCameraDistance;

    private Vector2 prevMouseDelta;
    
    private Vector2 cameraCurRot;

    public Vector3 camStartPos;


    private void Start()
    {
        playerTransform = transform.parent;
        inputHandler = GetComponentInParent<InputHandler>();
        camStartPos = transform.position;
    }

    private void LateUpdate()
    {
        if (inputHandler.mouseDelta != prevMouseDelta)
        {
            cameraCurRot.x += inputHandler.mouseDelta.x * lookSentitivity;
            cameraCurRot.y += inputHandler.mouseDelta.y * lookSentitivity;

            if (isFPSMode)
                FPSCameraLook();
            else
                TPSCameraLook();

            prevMouseDelta = inputHandler.mouseDelta;
        }

        
    }

    void FPSCameraLook()
    {
        transform.localEulerAngles = camStartPos;

        playerTransform.localEulerAngles = new Vector3(0, cameraCurRot.x, 0);

        cameraCurRot.y = Mathf.Clamp(cameraCurRot.y, minRotVertical, maxRotVertical);

        transform.localEulerAngles = new Vector3(-cameraCurRot.y, 0, 0);
    }

    void TPSCameraLook()
    {
        //playerTransform.localEulerAngles = new Vector3(0, cameraCurRot.x, 0);

        SetPositionTPSCamera();
        RotateTPSCamera();
    }

    void SetPositionTPSCamera()
    {
        //���� ī�޶� �ٶ󺸴� ����� �ݴ��� ������ �ٶ�
        Vector2 tpsCameraDir = new Vector2(cameraCurRot.x + 180f, cameraCurRot.y + 180f);
        float tpsCameraPosX = Mathf.Cos(tpsCameraDir.x * Mathf.Deg2Rad) * TPSCameraDistance;
        float tpsCameraPosZ = Mathf.Sin(tpsCameraDir.x * Mathf.Deg2Rad) * TPSCameraDistance;
        float tpsCameraPosY = Mathf.Sin(tpsCameraDir.y * Mathf.Deg2Rad) * TPSCameraDistance;

        transform.localPosition = new Vector3(tpsCameraPosX, tpsCameraPosY, tpsCameraPosZ) + camStartPos;
    }

    void RotateTPSCamera()
    {
        Vector3 targetPos = playerTransform.position + camStartPos;

        Vector3 playerDir = targetPos - transform.position;

        playerDir.Normalize();

        //ī�޶��� y�� ȸ��
        float tpsCameraRotY = Mathf.Atan2(playerDir.x, playerDir.z) * Mathf.Rad2Deg;

        //ī�޶��� x�� ȸ��
        Vector2 horizontalDirection = new Vector2(playerDir.x, playerDir.z);

        float tpsCameraRotX = Mathf.Atan(playerDir.y / horizontalDirection.magnitude) * Mathf.Rad2Deg;

        transform.localEulerAngles = new Vector3(-tpsCameraRotX, tpsCameraRotY, 0);
    }
}
