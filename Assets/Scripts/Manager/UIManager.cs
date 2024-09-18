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
        TogglePanel(inventoryPanel);
    }

    public void ToggleEquipment()
    {
        TogglePanel(equipmentPanel);
    }

    public void ToggleStatus()
    {
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
