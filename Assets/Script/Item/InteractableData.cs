using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Iteractable", menuName = "New Interactable")]

public class InteractableData : ScriptableObject
{
    [Header("Interactable")]
    public string Name;
    public string Description;
}
