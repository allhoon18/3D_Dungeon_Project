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

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.stat = this;
    }

    public void AddOrSubtract(StatType type, float value)
    {
        switch(type)
        {
            case StatType.Health:
                health += value;
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
