using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour, IInteractable
{
    [SerializeField] float damage;
    [SerializeField] InteractableData data;

    public InteractableData interactableData
    {
        get => data;
        set => value = data;
    }

    public string Name { get { return interactableData.Name; } }
    public string Description { get { return interactableData.Description; } }

    public event Action<GameObject> OnItemInteracted;
    public event Action OnItemInteractionEnded;

    private void OnCollisionEnter(Collision collision)
    {
        PlayerStat playerStat;

        if (collision.gameObject.TryGetComponent(out playerStat))
        {
            playerStat.AddOrSubtractStat(StatType.Health, -10f);
        }
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
