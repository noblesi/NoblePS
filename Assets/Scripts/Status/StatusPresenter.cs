public class StatusPresenter
{
    private IStatusView statusView;
    private StatusModel statusModel;

    public StatusPresenter(IStatusView view, StatusModel model)
    {
        statusView = view;
        statusModel = model;
    }

    public void Initialize()
    {
        statusModel.OnHealthChanged += () => statusView.DisplayStatus(statusModel);
        statusModel.OnManaChanged += () => statusView.DisplayStatus(statusModel);
        statusModel.OnExperienceChanged += () => statusView.DisplayStatus(statusModel);
        statusModel.OnStatPointsChanged += () => statusView.DisplayStatus(statusModel);

        statusView.DisplayStatus(statusModel);
    }

    public void GainExperience(int exp)
    {
        statusModel.GainExp(exp);
        statusView.DisplayStatus(statusModel);
    }

    public void ApplyItemBonus(Equipment item, bool equip)
    {
        statusModel.ApplyItemBonus(item, equip);
        statusView.DisplayStatus(statusModel);
    }

    public void AllocateStatPoint(string statType)
    {
        if (statusModel.GetStatPoints() > 0)
        {
            statusModel.AllocateStatPoint(statType);
            statusView.DisplayStatus(statusModel);
        }
    }
}
