using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, IInteractable
{
    [SerializeField] ItemData data;
    public string Name { get { return data.Name;  } }
    public string Description { get { return data.Description; } }

    public event Action<GameObject> OnInteracted;

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
        OnInteracted?.Invoke(this.gameObject);
    }

}
