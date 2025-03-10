using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandeler : MonoBehaviour
{
    public Transform playerTransform;
    public InputHandler inputHandler;

    //카메라 회전 정보
    [Header("Rotate Camera")]
    public Camera camera;
    [SerializeField] float lookSentitivity;
    [SerializeField] float maxRotVertical;
    [SerializeField] float minRotVertical;
    [SerializeField] float TPSCameraDistance;
    private Vector2 prevMouseDelta;
    
    private Vector2 cameraCurRot;

    private Vector2 tpsCameraCurRot;

    bool isFPSMode;

    public const float offsetY = 1.7f;

    private void Start()
    {
        playerTransform = transform.parent;
        inputHandler = GetComponentInParent<InputHandler>();
    }

    private void LateUpdate()
    {
        if (inputHandler.mouseDelta != prevMouseDelta)
        {
            //FPSCameraLook();
            TPSCameraLook();
            prevMouseDelta = inputHandler.mouseDelta;
        }

        //if (isFPSMode)
        //    FPSCameraLook();
        //else
        //    TPSCameraLook();
    }

    void FPSCameraLook()
    {
        cameraCurRot.x = inputHandler.mouseDelta.x * lookSentitivity;

        playerTransform.localEulerAngles += new Vector3(0, cameraCurRot.x, 0);

        cameraCurRot.y += inputHandler.mouseDelta.y * lookSentitivity;

        cameraCurRot.y = Mathf.Clamp(cameraCurRot.y, minRotVertical, maxRotVertical);

        transform.localEulerAngles = new Vector3(-cameraCurRot.y, 0, 0);
    }

    void TPSCameraLook()
    {
        cameraCurRot.x += inputHandler.mouseDelta.x * lookSentitivity;
        cameraCurRot.y += inputHandler.mouseDelta.y * lookSentitivity;

        TPSCameraSetPosition();
        TPSCameraSetRotation();
    }

    void TPSCameraSetPosition()
    {
        //현재 카메라가 바라보는 방향과 반대인 각도를 바라봄
        Vector2 tpsCameraDir = new Vector2(cameraCurRot.x + 180f, cameraCurRot.y + 180f);
        float tpsCameraPosX = Mathf.Cos(tpsCameraDir.x * Mathf.Deg2Rad) * TPSCameraDistance;
        float tpsCameraPosZ = Mathf.Sin(tpsCameraDir.x * Mathf.Deg2Rad) * TPSCameraDistance;
        float tpsCameraPosY = Mathf.Sin(tpsCameraDir.y * Mathf.Deg2Rad) * TPSCameraDistance;

        transform.localPosition = new Vector3(tpsCameraPosX, tpsCameraPosY + offsetY, tpsCameraPosZ);
    }

    void TPSCameraSetRotation()
    {
        Vector3 targetPos = playerTransform.position + Vector3.up;

        Vector3 playerDir = targetPos - transform.position;
        playerDir.Normalize();
        float tpsCameraRotY = Mathf.Atan(playerDir.x / playerDir.z) * Mathf.Rad2Deg;

        tpsCameraRotY = transform.localPosition.z > 0 ? tpsCameraRotY : tpsCameraRotY;

        //float a = Mathf.Asin(playerDir.y / 1);
        //float b = Mathf.Cos(a);

        //float tpsCameraRotY = Mathf.Atan(a/playerDir.y) * Mathf.Rad2Deg;


        transform.localEulerAngles = new Vector3(0, tpsCameraRotY, 0);
    }
}
