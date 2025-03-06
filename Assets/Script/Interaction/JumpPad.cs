using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [SerializeField] float power;
    [SerializeField] LayerMask playerLayer;

    private void OnCollisionEnter(Collision collision)
    {
        PlayerController playerController;

        if(collision.gameObject.TryGetComponent(out playerController))
        {
            Debug.Log("JumpPad Active");
            playerController.Jump(power);
            //UI �۵� �׽�Ʈ
            GameManager.Instance.stat.AddOrSubtract(StatType.Health, -10f);
        }
    }
}
