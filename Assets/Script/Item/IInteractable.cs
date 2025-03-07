using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    InteractableData interactableData { get; set; }

    public string Name { get; }
    public string Description { get; }

    public event Action<GameObject> OnItemInteracted; //��ȣ�ۿ� ���� �̺�Ʈ
    public event Action OnItemInteractionEnded; // ��ȣ�ۿ� ���� �̺�Ʈ

    void Interact();
    void EndInteraction();
}
