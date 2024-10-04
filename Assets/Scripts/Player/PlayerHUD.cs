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

    public Image hpFill;
    public Image mpFill;
    public Image expFill;

    private Color hpColor = new Color(0.8f, 0f, 0f, 1f);
    private Color mpColor = new Color(0f, 0.5f, 1f, 1f);
    private Color expColor = new Color(0.7f, 1f, 0f, 1f);

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

        SetSliderColors();
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

    private void SetSliderColors()
    {
        if (hpFill != null)
        {
            hpFill.color = hpColor;  // 체력바 빨간색
        }

        if (mpFill != null)
        {
            mpFill.color = mpColor;  // 마나바 파란색
        }

        if (expFill != null)
        {
            expFill.color = expColor;  // 경험치바 형광색 (연두색)
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
