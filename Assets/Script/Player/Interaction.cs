using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    [SerializeField] LayerMask targetLayer;
    [SerializeField] float interactRange;

    GameManager gameManager;

    GameObject _curInteraction;

    Camera _camera;

    private void Start()
    {
        gameManager = GameManager.Instance;
        _camera = gameManager.controller.camera;
    }

    private void Update()
    {
        CheckObject();
    }

    void CheckObject()
    {
        Ray ray = _camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactRange, targetLayer))
        {
            if (hit.collider.gameObject != _curInteraction)
            {
                Debug.Log("Check");
                _curInteraction = hit.collider.gameObject;
            }
        }
        else
        {
            _curInteraction = null;
        }
    }

    private void OnDrawGizmos()
    {
        if (_camera == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawLine(_camera.transform.position, _camera.transform.position + _camera.transform.forward * interactRange);
    }

}
