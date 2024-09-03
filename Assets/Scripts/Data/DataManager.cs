using UnityEngine;

public class DataManager : MonoBehaviour
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

    private InventoryModel LoadInventoryData()
    {
        // �κ��丮 �����͸� �ε��ϴ� ����
        // JSON, XML, DB ��� �����͸� �ҷ��� InventoryModel�� �����ϰ� ��ȯ
    }

    private void SaveInventoryData(InventoryModel model)
    {
        // �κ��丮 �����͸� �����ϴ� ����
        // JSON, XML, DB ������ �����͸� ����
    }

    private EquipmentModel LoadEquipmentData()
    {
        // ��� �����͸� �ε��ϴ� ����
        // JSON, XML, DB ��� �����͸� �ҷ��� EquipmentModel�� �����ϰ� ��ȯ
    }

    private void SaveEquipmentData(EquipmentModel model)
    {
        // ��� �����͸� �����ϴ� ����
        // JSON, XML, DB ������ �����͸� ����
    }
}
