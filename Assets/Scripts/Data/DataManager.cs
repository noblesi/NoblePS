using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    private StatusModel statusModel;
    private InventoryModel inventoryModel;
    private EquipmentModel equipmentModel;

    private void Awake()
    {
        LoadAllData();
    }

    private void LoadAllData()
    {
        inventoryModel = new InventoryModel(); // �κ��丮 ������ �ε�
        inventoryModel.LoadInventoryData();

        statusModel = new StatusModel(); // ���� ������ �ε�
        statusModel.LoadStatusData();

        equipmentModel = new EquipmentModel(); // ��� ������ �ε�
        equipmentModel.LoadEquipmentData();
    }

    public void SaveAllData()
    {
        inventoryModel.SaveInventoryData(); // �κ��丮 ������ ����
        statusModel.SaveStatusData(); // ���� ������ ����
        equipmentModel.SaveEquipmentData(); // ��� ������ ����
    }

    public InventoryModel GetInventoryModel()
    {
        return inventoryModel;
    }

    public StatusModel GetStatusModel()
    {
        return statusModel;
    }

    public EquipmentModel GetEquipmentModel()
    {
        return equipmentModel;
    }
}
