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

        strengthButton.onClick.RemoveAllListeners();
        strengthButton.onClick.AddListener(OnStrengthIncrease);

        dexterityButton.onClick.RemoveAllListeners();
        dexterityButton.onClick.AddListener(OnDexterityIncrease);

        intelligenceButton.onClick.RemoveAllListeners();
        intelligenceButton.onClick.AddListener(OnIntelligenceIncrease);
    }

    public void DisplayStatus(StatusModel status)
    {
        levelText.text = "Level : " + status.Level;
        hpText.text = "HP : " + status.HP;
        mpText.text = "MP : " + status.MP;

        strengthText.text = FormatStat("STR", status.BaseStrength, status.GetStrengthBonus(), status.Strength);
        dexterityText.text = FormatStat("DEX", status.BaseDexterity, status.GetDexterityBonus(), status.Dexterity);
        intelligenceText.text = FormatStat("INT", status.BaseIntelligence, status.GetIntelligenceBonus(), status.Intelligence);

        expText.text = $"EXP : {status.GetCurrentExp()} / {status.GetExpToNextLevel()}";

        statPointsText.text = "Stat Points : " + status.GetStatPoints();

        bool hasStatPoints = status.GetStatPoints() > 0;
        strengthButton.interactable = hasStatPoints;
        dexterityButton.interactable = hasStatPoints;
        intelligenceButton.interactable = hasStatPoints;
    }

    private string FormatStat(string statName, int baseValue, int bonusValue, int totalValue)
    {
        string formattedStat = $"{statName} : <color=#FFFFFF>{totalValue}</color> ";  // ���� ���

        formattedStat += $"(<color=#FFFFFF>{baseValue}</color>";  // ���̽� ���� ���
        if (bonusValue > 0)
        {
            formattedStat += $" + <color=#00FF00>{bonusValue}</color>)";  // ��� ���ʽ� �ʷϻ�
        }
        else if (bonusValue < 0)
        {
            formattedStat += $" - <color=#FF0000>{Mathf.Abs(bonusValue)}</color>)";  // ���� ���ʽ� ������
        }
        else
        {
            formattedStat += ")";  // ���ʽ��� ���� ��
        }

        return formattedStat;
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

    //������� ���� ������Ʈ��
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            presenter.GainExperience(50);
        }
    }
}
