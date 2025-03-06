using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    GameManager gameManager;

    [SerializeField] float power;

    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    private void OnCollisionEnter(Collision collision)
    {
        gameManager.controller.Jump(power);
    }
}
