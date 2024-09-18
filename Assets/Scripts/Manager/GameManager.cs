using System.Collections.Generic;

public class GameManager : Singleton<GameManager>
{
    public PlayerFSM playerFSM;
    public StatusView statusView;
    public InventoryView inventoryView;
    public EquipmentView equipmentView;

    private PlayerData playerData;
    private StatusPresenter statusPresenter;
    private InventoryPresenter inventoryPresenter;
    private EquipmentPresenter equipmentPresenter;

    private void Awake()
    {
        playerData = new PlayerData();

        // Status
        statusPresenter = new StatusPresenter(statusView, playerData.Status);

        // Inventory
        List<InventorySlot> inventorySlots = inventoryView.GetSlots();
        inventoryPresenter = new InventoryPresenter(inventoryView, playerData.Inventory, inventorySlots);

        // Equipment
        equipmentPresenter = new EquipmentPresenter(equipmentView, playerData.Equipment, inventoryPresenter, statusPresenter);

        playerFSM.SetInventoryPresenter(inventoryPresenter);

        inventoryPresenter.SetEquipmentPresenter(equipmentPresenter);

        // PlayerFSM
        playerFSM.SetPlayerData(playerData); // PlayerFSM¿¡ PlayerData Àü´Þ

        InitializeInventory();
        InitializeEquipment();
        InitializeStatus();
    }

    public void InitializeInventory()
    {
        inventoryView.Initialize(inventoryPresenter);
    }

    public void InitializeEquipment()
    {
        equipmentView.Initialize(equipmentPresenter);
    }

    public void InitializeStatus()
    {
        statusView.Initialize(statusPresenter);
    }
}
