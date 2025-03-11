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

    public PlayerStat targetPlayer;

    public void ActiveItemEffect()
    {
        if (targetPlayer == null) return;

        //������ ������ ���ӽð� �� ������ ���� �ٸ��� ����
        if (data.duration == 0)
            targetPlayer.AddOrSubtractStat(data.type, data.value);
        else
            targetPlayer.ChangeStatDuringDuration(data.type, data.value, data.duration);

        EndInteraction();
        Destroy(gameObject);
    }

    //Interaction�� ���� �����Ǿ��� �� ȣ��->�������� UI�� Ȱ��ȭ
    public void Interact()
    {
        OnItemInteracted?.Invoke(this.gameObject);
    }
    //�������� UI�� ����
    public void EndInteraction()
    {
        OnItemInteractionEnded?.Invoke();
    }

}
