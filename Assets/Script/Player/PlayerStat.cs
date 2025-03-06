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

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.stat = this;
        maxHealth = health;
    }

    public void AddOrSubtract(StatType type, float value)
    {
        switch(type)
        {
            case StatType.Health:
                if (value > 0)
                    health = Mathf.Min(health + value, maxHealth);
                else
                    health = Mathf.Max(0, health + value);
                
                GameManager.Instance.uiStat.UpdateUI(health/maxHealth);
                break;
            case StatType.Speed:
                speed += value;
                break;
            case StatType.JumpPower:
                jumpPower += value;
                break;
        }
    }
}
