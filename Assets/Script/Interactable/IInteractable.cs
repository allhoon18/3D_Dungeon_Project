using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    InteractableData interactableData { get; set; }

    public string Name { get; }
    public string Description { get; }

    public event Action<GameObject> OnItemInteracted; //상호작용 시작 이벤트
    public event Action OnItemInteractionEnded; // 상호작용 종료 이벤트

    void Interact();
    void EndInteraction();
}
