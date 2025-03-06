using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [SerializeField] float power;
    [SerializeField] LayerMask playerLayer;

    private void OnCollisionEnter(Collision collision)
    {
        //if ((playerLayer & (1 << collision.gameObject.layer)) != 0)
        //    return;

        PlayerController playerController;

        if(collision.gameObject.TryGetComponent(out playerController))
        {
            Debug.Log("JumpPad Active");
            playerController.Jump(power);
        }
    }
}
