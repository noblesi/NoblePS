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
        statusView.DisplayStatus(statusModel);
    }

    public void GainExperience(int exp)
    {
        statusModel.GainExp(exp);
        statusView.DisplayStatus(statusModel);
    }

    public void ApplyItemBonus(Item item, bool equip)
    {
        statusModel.ApplyItemBonus(item, equip);
        statusView.DisplayStatus(statusModel);
    }

    public void AllocateStatPoint(string statType)
    {
        statusModel.AllocateStatPoint(statType);
        statusView.DisplayStatus(statusModel);
    }
}
