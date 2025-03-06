using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] ItemData data;

    private void OnTriggerEnter(Collider other)
    {
        PlayerStat playerStat;
        if(other.gameObject.TryGetComponent(out playerStat))
        {
            playerStat.AddOrSubtract(data.type, data.value);
            Destroy(gameObject);
        }
    }

}
