using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public string Name { get; }
    public string Description { get; }

    public event Action<GameObject> OnInteracted;

    void Interact();
}
