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
        get => data; // ItemData를 InteractableData로 반환
        set => data = value as ItemData; // ItemData로 캐스팅하여 설정
    }

    public string Name => data.Name;
    public string Description => data.Description;

    public event Action<GameObject> OnItemInteracted;
    public event Action OnItemInteractionEnded;

    public PlayerStat targetPlayer;

    public void ActiveItemEffect()
    {
        if (targetPlayer == null) return;

        //아이템 정보의 지속시간 값 유무에 따라 다르게 적용
        if (data.duration == 0)
            targetPlayer.AddOrSubtractStat(data.type, data.value);
        else
            targetPlayer.ChangeStatDuringDuration(data.type, data.value, data.duration);

        EndInteraction();
        Destroy(gameObject);
    }

    //Interaction에 의해 감지되었을 때 호출->오버레이 UI를 활성화
    public void Interact()
    {
        OnItemInteracted?.Invoke(this.gameObject);
    }
    //오버레이 UI를 제거
    public void EndInteraction()
    {
        OnItemInteractionEnded?.Invoke();
    }

}
