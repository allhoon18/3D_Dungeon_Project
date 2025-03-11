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

    public void AddOrSubtractStat(StatType type, float amount)
    {
        Action<float> statChange;

        if(ChangeStat.TryGetValue(type, out statChange))
        {
            statChange(amount);
        }
    }

    public void ChangeStatDuringDuration(StatType type, float amount, float duration)
    {
        AddOrSubtractStat(type, amount);
        StartCoroutine(ResetStat(type, amount, duration));
    }

    IEnumerator ResetStat(StatType type, float amount, float duration)
    {
        yield return new WaitForSeconds(duration);
        AddOrSubtractStat(type, -amount);
    }
}
