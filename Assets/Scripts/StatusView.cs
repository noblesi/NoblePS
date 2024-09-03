using UnityEngine;
using UnityEngine.UI;

public class StatusView : MonoBehaviour, IStatusView
{
    public Text levelText;
    public Text hpText;
    public Text mpText;
    public Text strengthText;
    public Text dexterityText;
    public Text intelligenceText;
    public Slider expSlider;
    public Text statPointsText;

    private StatusPresenter presenter;

    public void Initialize(StatusPresenter statusPresenter)
    {
        presenter = statusPresenter;
        presenter.Initialize();
    }

    public void DisplayStatus(StatusModel status)
    {
        levelText.text = "Level: " + status.Level;
        hpText.text = "HP: " + status.HP;
        mpText.text = "MP: " + status.MP;
        strengthText.text = "Strength: " + status.Strength;
        dexterityText.text = "Dexterity: " + status.Dexterity;
        intelligenceText.text = "Intelligence: " + status.Intelligence;

        expSlider.maxValue = status.GetExpToNextLevel();
        expSlider.value = status.GetCurrentExp();
        statPointsText.text = "Stat Points: " + status.GetStatPoints();
    }

    public void OnStrengthIncrease()
    {
        presenter.AllocateStatPoint("Strength");
    }

    public void OnDexterityIncrease()
    {
        presenter.AllocateStatPoint("Dexterity");
    }

    public void OnIntelligenceIncrease()
    {
        presenter.AllocateStatPoint("Intelligence");
    }

    //디버깅을 위한 업데이트문
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            presenter.GainExperience(50);
        }
    }
}
