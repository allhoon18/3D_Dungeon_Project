using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour, IInteractable
{
    [SerializeField] float power;
    [SerializeField] InteractableData data;

    public InteractableData interactableData
    {
        get => data;
        set => value = data;
    }

    public string Name { get { return interactableData.Name; } }
    public string Description { get { return interactableData.Description; } }
    
    public event Action<GameObject> OnItemInteracted;

    private void OnCollisionEnter(Collision collision)
    {
        PlayerController playerController;

        if(collision.gameObject.TryGetComponent(out playerController))
        {
            Debug.Log("JumpPad Active");
            playerController.Jump(power);
            //UI 작동 테스트
            GameManager.Instance.stat.AddOrSubtract(StatType.Health, -10f);
        }
    }

    public void Interact()
    {
        OnItemInteracted?.Invoke(this.gameObject);
    }
}
