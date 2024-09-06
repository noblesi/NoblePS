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
    public Text expText;
    public Text statPointsText;

    public Button strengthButton;
    public Button dexterityButton;
    public Button intelligenceButton;

    private StatusPresenter presenter;

    public void Initialize(StatusPresenter statusPresenter)
    {
        presenter = statusPresenter;
        presenter.Initialize();

        strengthButton.onClick.AddListener(OnStrengthIncrease);
        dexterityButton.onClick.AddListener(OnDexterityIncrease);
        intelligenceButton.onClick.AddListener(OnIntelligenceIncrease);
    }

    public void DisplayStatus(StatusModel status)
    {
        levelText.text = "Level : " + status.Level;
        hpText.text = "HP : " + status.HP;
        mpText.text = "MP : " + status.MP;

        strengthText.text = $"STR : <color=#FFFFFF>{status.BaseStrength}</color> <color=#00FF00>(+{status.GetStrengthBonus()})</color>";
        dexterityText.text = $"DEX : <color=#FFFFFF>{status.BaseDexterity}</color> <color=#00FF00>(+{status.GetDexterityBonus()})</color>";
        intelligenceText.text = $"INT : <color=#FFFFFF>{status.BaseIntelligence}</color> <color=#00FF00>(+{status.GetIntelligenceBonus()})</color>";

        expText.text = $"EXP : {status.GetCurrentExp()} / {status.GetExpToNextLevel()}";

        statPointsText.text = "Stat Points : " + status.GetStatPoints();

        bool hasStatPoints = status.GetStatPoints() > 0;
        strengthButton.interactable = hasStatPoints;
        dexterityButton.interactable = hasStatPoints;
        intelligenceButton.interactable = hasStatPoints;
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
