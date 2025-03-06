using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStat : MonoBehaviour
{
    [SerializeField] Image healthBar;

    private void Start()
    {
        GameManager.Instance.uiStat = this;
    }

    public void UpdateUI(float amount)
    {
        healthBar.fillAmount = amount;
    }
}
