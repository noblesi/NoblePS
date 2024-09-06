using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PlayerFSM playerFSM;
    public StatusView statusView;
    public InventoryView inventoryView;
    public EquipmentView equipmentView;

    private PlayerData playerData;
    private StatusPresenter statusPresenter;
    private InventoryPresenter inventoryPresenter;
    private EquipmentPresenter equipmentPresenter;

    private void Start()
    {
        playerData = new PlayerData();

        // Status
        statusPresenter = new StatusPresenter(statusView, playerData.Status);
        statusView.Initialize(statusPresenter);

        // Inventory
        inventoryPresenter = new InventoryPresenter(inventoryView, playerData.Inventory, equipmentPresenter, playerData);
        inventoryView.Initialize(inventoryPresenter);

        // Equipment
        equipmentPresenter = new EquipmentPresenter(equipmentView, playerData.Equipment, inventoryPresenter, statusPresenter);
        equipmentView.Initialize(equipmentPresenter);

        inventoryPresenter.SetEquipmentPresenter(equipmentPresenter);

        // PlayerFSM
        playerFSM.SetPlayerData(playerData); // PlayerFSM¿¡ PlayerData Àü´Þ
    }
}
