using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameTitleManager : MonoBehaviour
{
    public Button newGameButton;
    public Button continueButton;
    public Button settingButton;
    public Button quitButton;

    public GameObject settingPanel;
    public GameObject playerNamePanel;
    public InputField playerNameInputField;
    public Button confirmNameButton;

    private void Start()
    {
        settingPanel.SetActive(false);
        playerNamePanel.SetActive(false);
        CheckSaveData();
    }

    private void CheckSaveData()
    {
        if (SaveDataManager.SaveDataExists())
        {
            continueButton.interactable = true;
        }
        else
        {
            continueButton.interactable= false;
        }
    }

    public void OnNewGame()
    {
        if(SaveDataManager.SaveDataExists())
        {
            ShowOverwriteWarning();
        }
        else
        {
            StartNewGame();
        }
    }

    private void ShowOverwriteWarning()
    {

    }

    public void StartNewGame()
    {
        SaveDataManager.ClearSaveData();
        ShowPlayerNameInput();
    }

    private void ShowPlayerNameInput()
    {
        playerNamePanel.SetActive(true);
    }

    public void OnConfirmPlayerName()
    {
        string playerName = playerNameInputField.text.Trim();

        if (string.IsNullOrEmpty(playerName))
        {
            playerName = "ฟ๋ป็";
        }

        PlayerData playerData = new PlayerData();
        playerData.PlayerName = playerName;
        SaveDataManager.SavePlayerData(playerData);

        SceneManager.LoadScene("MainGame");
    }

    public void OnContinue()
    {
        if (SaveDataManager.SaveDataExists())
        {
            SceneManager.LoadScene("MainGame");
        }
    }

    public void OnSetting()
    {
        settingPanel.SetActive(true);
    }

    public void OnApplicationQuit()
    {
        Application.Quit();
    }

    public void OnCloseSettings()
    {
        settingPanel.SetActive(false);
    }
}
