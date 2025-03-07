using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    [SerializeField] LayerMask targetLayer;
    [SerializeField] float interactRange;

    GameObject _curInteraction;

    [HideInInspector] public Camera camera;

    private void Start()
    {
        GameManager.Instance.interaction = this;
    }

    private void Update()
    {
        CheckObject();
    }

    void CheckObject()
    {
        Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactRange, targetLayer))
        {
            if (hit.collider.gameObject != _curInteraction)
            {
                _curInteraction = hit.collider.gameObject;

                ItemInteract(hit.collider.gameObject);
            }
        }
        else
        {
            if(_curInteraction != null)
                EndItemInteact(_curInteraction);
            _curInteraction = null;
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
        Gizmos.DrawLine(camera.transform.position, camera.transform.position + camera.transform.forward * interactRange);
    }

}
