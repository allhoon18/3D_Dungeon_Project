using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatType
{
    Health,
    Speed,
    JumpPower
}

public class PlayerStat : MonoBehaviour
{
    [Header("Stat")]
    public float health;
    public float speed;
    public float jumpPower;

    private float maxHealth;

    public Action<float> OnStatChanged;

    Dictionary<StatType, Action<float>> ChangeStat;

    public void Init()
    {
        maxHealth = health;

        ChangeStat = new Dictionary<StatType, Action<float>>
        {
            { StatType.Health, ChangeHealth },
            { StatType.Speed, ChangeSpeed },
            { StatType.JumpPower, ChangeJumpPower }
        };
    }

    void ChangeHealth(float value)
    {
        if (value > 0)
            health = Mathf.Min(health + value, maxHealth);
        else
            health = Mathf.Max(0, health + value);

        Debug.Log("Health Changed");
        OnStatChanged?.Invoke(health / maxHealth);
    }

    void ChangeSpeed(float value)
    {
        speed = Mathf.Max(0, speed + value);
    }

    void ChangeJumpPower(float value)
    {
        jumpPower = Mathf.Max(0, jumpPower + value);
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
