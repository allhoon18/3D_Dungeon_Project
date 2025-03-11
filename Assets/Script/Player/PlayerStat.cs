using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatType
{
    Health,
    Speed,
    JumpPower,
    Stamina
}

public class PlayerStat : MonoBehaviour
{
    [Header("Stat")]
    public float health;
    public float walkSpeed;
    public float runSpeed;
    public float jumpPower;
    public float stamina;

    private float maxHealth;
    private float maxStamina;

    public float staminaRecoverPerSecond;
    public float staminaUsageForRunning;
    public float staminaUsageForJump;

    public Action<StatType, float> OnStatChanged;

    Dictionary<StatType, Action<float>> ChangeStat;

    public void Init()
    {
        maxHealth = health;
        maxStamina = stamina;

        //스탯 타입과 그에 따른 변경 함수를 저장
        ChangeStat = new Dictionary<StatType, Action<float>>
        {
            { StatType.Health, ChangeHealth },
            { StatType.Speed, ChangeSpeed },
            { StatType.JumpPower, ChangeJumpPower },
            { StatType.Stamina, ChangeStamina }
        };
    }

    private void Update()
    {
        if (stamina <= maxStamina)
            InvokeRepeating("RecoverStamina", 0f, 1f);
    }

    void RecoverStamina()
    {
        AddOrSubtractStat(StatType.Stamina, staminaRecoverPerSecond);
    }

    void ChangeHealth(float value)
    {
        health = value > 0 ? Mathf.Min(health + value, maxHealth) : Mathf.Max(0, health + value);

        OnStatChanged?.Invoke(StatType.Health, health / maxHealth);
    }

    void ChangeSpeed(float value)
    {
        walkSpeed = Mathf.Max(0, walkSpeed + value);
        runSpeed = Mathf.Max(0, runSpeed + value);
        OnStatChanged?.Invoke(StatType.Speed, 0);
    }

    void ChangeJumpPower(float value)
    {
        jumpPower = Mathf.Max(0, jumpPower + value);
        OnStatChanged?.Invoke(StatType.JumpPower, 0);
    }

    void ChangeStamina(float value)
    {
        stamina = value > 0 ? Mathf.Min(stamina+value, maxStamina) : Mathf.Max(0, stamina + value);

        OnStatChanged?.Invoke(StatType.Stamina, stamina / maxStamina);
    }

    //외부에서 스탯 변경시에는 이 함수를 호출
    //스탯 타입과 값을 입력해 해당 스탯의 값을 수정
    public void AddOrSubtractStat(StatType type, float amount)
    {
        Action<float> statChange;

        if(ChangeStat.TryGetValue(type, out statChange))
        {
            statChange(amount);
        }
    }
    //일정 기간동안 적용되는 스탯 변경을 적용
    public void ChangeStatDuringDuration(StatType type, float amount, float duration)
    {
        AddOrSubtractStat(type, amount);
        StartCoroutine(ResetStat(type, amount, duration));
    }
    //일정 시간이 지나면 스탯을 복구
    IEnumerator ResetStat(StatType type, float amount, float duration)
    {
        yield return new WaitForSeconds(duration);
        AddOrSubtractStat(type, -amount);
    }
}
