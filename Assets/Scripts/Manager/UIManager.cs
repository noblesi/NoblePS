using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    public GameObject inventoryPanel;
    public GameObject equipmentPanel;
    public GameObject statusPanel;

    public void ToggleInventory()
    {
        if(!inventoryPanel.activeSelf)
        {
            GameManager.Instance.InitializeInventory();
        }
        TogglePanel(inventoryPanel);
    }

    public void ToggleEquipment()
    {
        if (!equipmentPanel.activeSelf)
        {
            GameManager.Instance.InitializeEquipment();
        }
        TogglePanel(equipmentPanel);
    }

    public void ToggleStatus()
    {
        if (!statusPanel.activeSelf)
        {
            GameManager.Instance.InitializeStatus();
        }
        TogglePanel(statusPanel);
    }

    private void TogglePanel(GameObject panel)
    {
        if(panel != null)
        {
            panel.SetActive(!panel.activeSelf);
        }
    }
}
