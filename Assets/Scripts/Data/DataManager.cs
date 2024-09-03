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

    private InventoryModel LoadInventoryData()
    {
        // 인벤토리 데이터를 로드하는 로직
        // JSON, XML, DB 등에서 데이터를 불러와 InventoryModel을 생성하고 반환
    }

    private void SaveInventoryData(InventoryModel model)
    {
        // 인벤토리 데이터를 저장하는 로직
        // JSON, XML, DB 등으로 데이터를 저장
    }

    private EquipmentModel LoadEquipmentData()
    {
        // 장비 데이터를 로드하는 로직
        // JSON, XML, DB 등에서 데이터를 불러와 EquipmentModel을 생성하고 반환
    }

    private void SaveEquipmentData(EquipmentModel model)
    {
        // 장비 데이터를 저장하는 로직
        // JSON, XML, DB 등으로 데이터를 저장
    }
}
