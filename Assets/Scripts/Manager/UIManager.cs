using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : Singleton<UIManager>
{
    public GameObject inventoryPanel;
    public GameObject equipmentPanel;
    public GameObject statusPanel;
    public GameObject quitPanel;

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

    public void ToggleQuit()
    {
        TogglePanel(quitPanel);
    }

    public void ReturnGameTitle()
    {
        quitPanel.SetActive(false);
        DataManager.Instance.SaveAllData();
        SceneManager.LoadScene("GameTitleScene");
    }

    public void ReturnGame()
    {
        quitPanel.SetActive(false);
    }

    private void TogglePanel(GameObject panel)
    {
        if(panel != null)
        {
            panel.SetActive(!panel.activeSelf);
        }
    }
}
