using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    [SerializeField] LayerMask targetLayer;
    [SerializeField] float interactRange;
    [SerializeField] Camera camera;

    GameObject curInteraction;

    private void Update()
    {
        Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactRange, targetLayer))
        {
            if (hit.collider.gameObject != curInteraction)
            {
                Debug.Log("Check");
                curInteraction = hit.collider.gameObject;
            }
        }
        else
        {
            curInteraction = null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(camera.transform.position, camera.transform.position + camera.transform.forward * interactRange);
    }

}
