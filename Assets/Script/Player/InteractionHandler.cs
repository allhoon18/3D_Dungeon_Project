using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionHandler : MonoBehaviour
{
    [SerializeField] LayerMask targetLayer;
    [SerializeField] float interactRange;
    [SerializeField] Transform interactPoint;

    GameObject curInteraction;

    public Camera camera;

    InputHandler input;

    private void Start()
    {
        input = GetComponent<InputHandler>();
    }

    private void Update()
    {
        CheckObject();

        if (input.isUse && curInteraction != null)
        {
            Item curItem;

            if(curInteraction.TryGetComponent<Item>( out curItem ))
            {
                curItem.targetPlayer = GetComponent<PlayerStat>();
                curItem.ActiveItemEffect();
            }
        }
    }

    void CheckObject()
    {
        if (camera == null) return;

        Ray ray = new Ray(interactPoint.position, camera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactRange, targetLayer))
        {
            if (hit.collider.gameObject != curInteraction)
            {
                curInteraction = hit.collider.gameObject;

                ItemInteract(hit.collider.gameObject);
            }
        }
        else
        {
            if(curInteraction != null)
                EndItemInteact(curInteraction);
            curInteraction = null;
        }
    }

    void ItemInteract(GameObject hitObject)
    {
        IInteractable interactableObj;

        if(hitObject.TryGetComponent(out interactableObj))
        {
            interactableObj.Interact();
        }
    }

    void EndItemInteact(GameObject hitObject)
    {
        IInteractable interactableObj;

        if (hitObject.TryGetComponent(out interactableObj))
        {
            interactableObj.EndInteraction();
        }
    }



    private void OnDrawGizmos()
    {
        if (camera == null) return;

        Gizmos.color = Color.green;
        //Gizmos.DrawLine(transform.position, transform.forward * interactRange);
        Gizmos.DrawLine(interactPoint.position, interactPoint.position + camera.transform.forward.normalized * interactRange);
    }

}
