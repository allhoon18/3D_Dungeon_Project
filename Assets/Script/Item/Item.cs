using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, IInteractable
{
    [SerializeField] ItemData data;

    public InteractableData interactableData
    {
        get => data; // ItemData를 InteractableData로 반환
        set => data = value as ItemData; // ItemData로 캐스팅하여 설정
    }

    public string Name { get { return data.name;  } }
    public string Description { get { return data.Description; } }

    public event Action<GameObject> OnItemInteracted;

    private void OnTriggerEnter(Collider other)
    {
        PlayerStat playerStat;
        if(other.gameObject.TryGetComponent(out playerStat))
        {
            playerStat.AddOrSubtract(data.type, data.value);
            Destroy(gameObject);
        }
    }

    public void Interact()
    {
        OnItemInteracted?.Invoke(this.gameObject);
    }

}
