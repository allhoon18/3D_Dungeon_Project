using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStat : MonoBehaviour
{
    [SerializeField] Image healthBar;

    public PlayerStat stat;

    public void Init()
    {
        stat.OnStatChanged += UpdateUI;
    }

    void UpdateUI(float amount)
    {
        healthBar.fillAmount = amount;
    }
}
