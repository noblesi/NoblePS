using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    public Text hpText;
    public Text mpText;
    public Text expText;

    private StatusModel status;

    public void Initialize(StatusModel status)
    {
        this.status = status;

        status.OnHealthChanged += UpdateHealth;
        status.OnManaChanged += UpdateMana;
        status.OnExperienceChanged += UpdateExperience;

        UpdateHealth();
        UpdateMana();
        UpdateExperience();
    }

    private void OnDestroy()
    {
        if(status != null)
        {
            status.OnHealthChanged -= UpdateHealth;
            status.OnManaChanged -= UpdateMana;
            status.OnExperienceChanged -= UpdateExperience;
        }
    }

    private void UpdateHealth()
    {
        hpText.text = $"HP: {status.HP} / {status.MaxHP}";
    }
    private void UpdateMana()
    {
        mpText.text = $"MP: {status.MP} / {status.MaxMP}";
    }

    private void UpdateExperience()
    {
        expText.text = $"EXP: {status.GetCurrentExp()} / {status.GetExpToNextLevel()}";
    }
}
