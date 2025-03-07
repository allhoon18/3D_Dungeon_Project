using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Item : MonoBehaviour, IInteractable
{
    [SerializeField] ItemData data;

    public InteractableData interactableData
    {
        get => data; // ItemData�� InteractableData�� ��ȯ
        set => data = value as ItemData; // ItemData�� ĳ�����Ͽ� ����
    }

    public string Name => data.Name;
    public string Description => data.Description;

    public event Action<GameObject> OnItemInteracted;
    public event Action OnItemInteractionEnded;

    private void OnTriggerEnter(Collider other)
    {
        PlayerStat playerStat;
        if(other.gameObject.TryGetComponent(out playerStat))
        {
            playerStat.ChangeStatDuringDuration(data.type, data.value, 5);
            Destroy(gameObject);
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
