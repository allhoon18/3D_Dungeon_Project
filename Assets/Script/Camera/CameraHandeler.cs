using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandeler : MonoBehaviour
{
    [SerializeField] Transform playerTransform;
    [SerializeField] InputHandler inputHandler;

    public bool isFPSMode;
    [SerializeField] Vector3 cameraOffset;

    //ī�޶� ȸ�� ����
    [Header("FPS Info")]
    [SerializeField] float lookSentitivity;
    [SerializeField] float maxRotVertical;
    [SerializeField] float minRotVertical;

    [Header("TPS Info")]
    [SerializeField] float TPSCameraDistance;
    

    private Vector2 prevMouseDelta;
    
    private Vector2 cameraCurRot;

    private Vector3 camStartPos;


    private void Start()
    {
        camStartPos = transform.position;
    }

    private void FixedUpdate()
    {
        if (isFPSMode)
            transform.position = playerTransform.position + cameraOffset;
    }

    private void LateUpdate()
    {
        isFPSMode = inputHandler.cameraChange;

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
        playerTransform.localEulerAngles = new Vector3(0, cameraCurRot.x, 0);

        cameraCurRot.y = Mathf.Clamp(cameraCurRot.y, minRotVertical, maxRotVertical);

        transform.localEulerAngles = new Vector3(-cameraCurRot.y, cameraCurRot.x, 0);
    }

    void TPSCameraLook()
    {
        SetPositionTPSCamera();
        RotateTPSCamera();
        playerTransform.eulerAngles = new Vector3(0, cameraCurRot.x, 0);
    }

    void SetPositionTPSCamera()
    {
        //���� ī�޶� �ٶ󺸴� ����� �ݴ��� ������ �ٶ�
        Vector2 tpsCameraDir = new Vector2(-cameraCurRot.x + 180, cameraCurRot.y + 180);
        float tpsCameraPosX = Mathf.Cos(tpsCameraDir.x * Mathf.Deg2Rad);
        float tpsCameraPosZ = Mathf.Sin(tpsCameraDir.x * Mathf.Deg2Rad);
        float tpsCameraPosY = Mathf.Sin(tpsCameraDir.y * Mathf.Deg2Rad);

        Vector3 tpsCameraPos = new Vector3(tpsCameraPosX, tpsCameraPosY, tpsCameraPosZ).normalized * TPSCameraDistance;

        transform.position = playerTransform.position + tpsCameraPos + cameraOffset;
    }

    void RotateTPSCamera()
    {
        Vector3 targetPos = playerTransform.position + cameraOffset;
        Vector3 playerDir = targetPos - transform.position;

        playerDir.Normalize();

        //ī�޶��� y�� ȸ��
        float tpsCameraRotY = Mathf.Atan2(playerDir.x, playerDir.z) * Mathf.Rad2Deg;

        //ī�޶��� x�� ȸ��
        Vector2 horizontalDirection = new Vector2(playerDir.x, playerDir.z);

        float tpsCameraRotX = Mathf.Atan(playerDir.y / horizontalDirection.magnitude) * Mathf.Rad2Deg;

        transform.eulerAngles = new Vector3(-tpsCameraRotX, tpsCameraRotY, 0);
    }
}
