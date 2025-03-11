using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandeler : MonoBehaviour
{
    Transform playerTransform;
    InputHandler inputHandler;

    public bool isFPSMode;

    //카메라 회전 정보
    [Header("FPS Info")]
    [SerializeField] float lookSentitivity;
    [SerializeField] float maxRotVertical;
    [SerializeField] float minRotVertical;

    [Header("TPS Info")]
    [SerializeField] float TPSCameraDistance;
    [SerializeField] Vector3 TPSOffset;

    private Vector2 prevMouseDelta;
    
    private Vector2 cameraCurRot;

    private Vector3 camStartPos;


    private void Start()
    {
        playerTransform = transform.parent;
        inputHandler = GetComponentInParent<InputHandler>();
        camStartPos = transform.localPosition;
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
        transform.localPosition = camStartPos;

        playerTransform.localEulerAngles = new Vector3(0, cameraCurRot.x, 0);

        cameraCurRot.y = Mathf.Clamp(cameraCurRot.y, minRotVertical, maxRotVertical);

        transform.localEulerAngles = new Vector3(-cameraCurRot.y, 0, 0);
    }

    void TPSCameraLook()
    {
        SetPositionTPSCamera();
        RotateTPSCamera();
        playerTransform.eulerAngles = new Vector3(0, cameraCurRot.x, 0);
    }

    void SetPositionTPSCamera()
    {
        //현재 카메라가 바라보는 방향과 반대인 각도를 바라봄
        Vector2 tpsCameraDir = new Vector2(-cameraCurRot.x + 180, cameraCurRot.y + 180);
        float tpsCameraPosX = Mathf.Cos(tpsCameraDir.x * Mathf.Deg2Rad);
        float tpsCameraPosZ = Mathf.Sin(tpsCameraDir.x * Mathf.Deg2Rad);
        float tpsCameraPosY = Mathf.Sin(tpsCameraDir.y * Mathf.Deg2Rad);

        Vector3 tpsCameraPos = new Vector3(tpsCameraPosX, tpsCameraPosY, tpsCameraPosZ).normalized * TPSCameraDistance;

        transform.localPosition = tpsCameraPos + TPSOffset;
    }

    void RotateTPSCamera()
    {
        Vector3 targetPos = playerTransform.position + TPSOffset;
        Vector3 playerDir = targetPos - transform.position;

        playerDir.Normalize();

        //카메라의 y축 회전
        float tpsCameraRotY = Mathf.Atan2(playerDir.x, playerDir.z) * Mathf.Rad2Deg;

        //카메라의 x축 회전
        Vector2 horizontalDirection = new Vector2(playerDir.x, playerDir.z);

        float tpsCameraRotX = Mathf.Atan(playerDir.y / horizontalDirection.magnitude) * Mathf.Rad2Deg;

        transform.eulerAngles = new Vector3(-tpsCameraRotX, tpsCameraRotY, 0);
    }
}
