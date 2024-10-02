using System.Collections.Generic;
using UnityEngine;

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
        playerData = DataManager.Instance.GetPlayerData();

        InitializeGameSystems();
    }

    private void InitializeGameSystems()
    {
        if(playerData != null)
        {
            // Status
            statusPresenter = new StatusPresenter(statusView, playerData.Status);
            statusView.Initialize(statusPresenter);

            // Inventory
            List<InventorySlot> inventorySlots = inventoryView.GetSlots();
            inventoryPresenter = new InventoryPresenter(inventoryView, playerData.Inventory, inventorySlots);
            inventoryView.Initialize(inventoryPresenter);
            inventoryPresenter.Initialize();

            // Equipment
            equipmentPresenter = new EquipmentPresenter(equipmentView, playerData.Equipment, inventoryPresenter, statusPresenter);
            equipmentView.Initialize(equipmentPresenter);
            equipmentPresenter.Initialize();

            playerFSM.SetInventoryPresenter(inventoryPresenter);
            inventoryPresenter.SetEquipmentPresenter(equipmentPresenter);
                
            // PlayerFSM
            playerFSM.SetPlayerData(playerData); // PlayerFSM에 PlayerData 전달
    
            InitializeInventory();
            InitializeEquipment();
            InitializeStatus();
        }
        else
        {
            Debug.LogError("PlayerData가 초기화되지 않았습니다.");
        }
    }

    public PlayerData GetPlayerData()
    {
        return playerData;
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
