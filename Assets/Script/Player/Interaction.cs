using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    [SerializeField] LayerMask targetLayer;
    [SerializeField] float interactRange;

    Camera camera;
    Transform cameraTransform;

    private void Start()
    {
        camera = Camera.main;
        cameraTransform = camera.transform;
    }

    private void Update()
    {
        Ray ray = camera.ScreenPointToRay(cameraTransform.position);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, interactRange, targetLayer))
        {
            Debug.Log("Check");
        }
    }

}
