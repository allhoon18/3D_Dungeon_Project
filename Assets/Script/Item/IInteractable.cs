using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    InteractableData interactableData { get; set; }

    public string Name { get; }
    public string Description { get; }

    public event Action<GameObject> OnItemInteracted;

    void Interact();
}
