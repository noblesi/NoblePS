using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    public Slider hpSlider;
    public Slider mpSlider;
    public Slider expSlider;
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
        if(hpSlider != null)
        {
            hpSlider.maxValue = status.MaxHP;
            hpSlider.value = status.HP;
        }

        hpText.text = $"HP: {status.HP} / {status.MaxHP}";
    }
    private void UpdateMana()
    {
        if(mpSlider != null)
        {
            mpSlider.maxValue = status.MaxMP;
            mpSlider.value = status.MP;
        }

        mpText.text = $"MP: {status.MP} / {status.MaxMP}";
    }

    private void UpdateExperience()
    {
        if(expSlider != null)
        {
            expSlider.maxValue = status.GetExpToNextLevel();
            expSlider.value = status.GetCurrentExp();
        }
        expText.text = $"EXP: {status.GetCurrentExp()} / {status.GetExpToNextLevel()}";
    }
}
