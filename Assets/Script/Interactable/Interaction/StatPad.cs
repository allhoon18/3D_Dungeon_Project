using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatPad : MonoBehaviour, IInteractable
{
    [SerializeField] StatType type;
    [SerializeField] float value;
    [SerializeField] InteractableData data;
    [SerializeField] bool resetOnEnd;

    public InteractableData interactableData
    {
        get => data;
        set => value = data;
    }

    public string Name { get { return interactableData.Name; } }
    public string Description { get { return interactableData.Description; } }

    public event Action<GameObject> OnItemInteracted;
    public event Action OnItemInteractionEnded;

    PlayerStat playerStat;

    private void OnCollisionEnter(Collision collision)
    {
        

        if (collision.gameObject.TryGetComponent(out playerStat))
        {
            playerStat.AddOrSubtractStat(type, value);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if(playerStat != null && resetOnEnd)
            playerStat.AddOrSubtractStat(type, -value);

        playerStat = null;
    }

    public void Interact()
    {
        OnItemInteracted?.Invoke(this.gameObject);
    }

    public void EndInteraction()
    {
        OnItemInteractionEnded?.Invoke();
    }
}
