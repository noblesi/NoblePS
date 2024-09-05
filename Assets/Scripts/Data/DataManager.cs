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
        inventoryModel = new InventoryModel(); // 인벤토리 데이터 로드
        inventoryModel.LoadInventoryData();

        statusModel = new StatusModel(); // 스탯 데이터 로드
        statusModel.LoadStatusData();

        equipmentModel = new EquipmentModel(); // 장비 데이터 로드
        equipmentModel.LoadEquipmentData();
    }

    public void SaveAllData()
    {
        inventoryModel.SaveInventoryData(); // 인벤토리 데이터 저장
        statusModel.SaveStatusData(); // 스탯 데이터 저장
        equipmentModel.SaveEquipmentData(); // 장비 데이터 저장
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
