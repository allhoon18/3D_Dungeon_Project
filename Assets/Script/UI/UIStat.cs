using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStat : MonoBehaviour
{
    [SerializeField] Image healthBar;
    [SerializeField] Image staminaBar;
    [SerializeField] GameObject speedUpUI;
    [SerializeField] GameObject jumpPowerUpUI;

    public PlayerStat stat;

    public void Init()
    {
        stat.OnStatChanged += UpdateUI;
    }

    void UpdateUI(StatType type, float amount = 0)
    {
        switch(type)
        {
            case StatType.Health:
                healthBar.fillAmount = amount;
                break;

            case StatType.Stamina:
                staminaBar.fillAmount = amount;
                break;

            case StatType.Speed:
                speedUpUI.SetActive(!speedUpUI.activeSelf);
                break;

            case StatType.JumpPower:
                jumpPowerUpUI.SetActive(!jumpPowerUpUI.activeSelf);
                break;
        }
        
    }
}
