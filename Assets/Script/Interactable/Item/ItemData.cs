using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "New Item")]

public class ItemData : InteractableData
{
    [Header("Item")]
    public StatType type;
    public float value;
    public float duration;
}
