using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatPad : MonoBehaviour, IInteractable
{
    //기존 함정 클래스를 개편해 체력 값에만 변화를 주는 것이 아닌, 스탯에 변화를 주는 포괄적인 역할로 수정
    [SerializeField] StatType type;
    [SerializeField] float value;
    [SerializeField] InteractableData data;
    [SerializeField] bool resetOnEnd;

    public InteractableData interactableData
    {
        get => data;
        set => value = data;
    }

    public string Name { get { return interactableData.Name; } }
    public string Description { get { return interactableData.Description; } }

    public event Action<GameObject> OnItemInteracted;
    public event Action OnItemInteractionEnded;

    PlayerStat playerStat;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out playerStat))
        {
            playerStat.AddOrSubtractStat(type, value);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if(playerStat != null && resetOnEnd)
            playerStat.AddOrSubtractStat(type, -value);

        playerStat = null;
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
