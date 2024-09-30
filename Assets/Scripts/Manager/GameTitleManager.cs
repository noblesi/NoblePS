using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameTitleManager : MonoBehaviour
{
    public Button startGameButton;
    public Button loadGameButton;
    public Button settingsButton;
    public Button exitGameButton;

    public GameObject confirmationPopup;
    public Text confirmationMessage;
    public Button confirmYesButton;
    public Button confirmNoButton;

    public GameObject settingsPopup;
    public Button settingsSaveButton;

    public Toggle fullscreenToggle;
    public Toggle windowedToggle;

    private DataManager dataManager;

    private void Start()
    {
        dataManager = DataManager.Instance;

        loadGameButton.interactable = IsSaveDataAvailable();

        startGameButton.onClick.AddListener(OnStartGameButtonClicked);
        loadGameButton.onClick.AddListener(OnLoadGameButtonClicked);
        settingsButton.onClick.AddListener(OnSettingsButtonClicked);
        exitGameButton.onClick.AddListener(OnExitGameButtonClicked);

        confirmYesButton.onClick.AddListener(OnConfirmYesClicked);
        confirmNoButton.onClick.AddListener(OnConfirmNoClicked);

        settingsSaveButton.onClick.AddListener(OnSettingsSaveClicked);

        confirmationPopup.SetActive(false);
        settingsPopup.SetActive(false);

        LoadCurrentSettings();
    }

    private bool IsSaveDataAvailable()
    {
        return dataManager.HasSaveData();
    }

    private void OnStartGameButtonClicked()
    {
        if(IsSaveDataAvailable())
        {
            ShowConfirmationPopup("������ ���� �����͸� �ʱ�ȭ�ϰ� �� ������ �����Ͻðڽ��ϱ�?");
        }
        else
        {
            StartNewGame();
        }
    }

    private void OnLoadGameButtonClicked()
    {
        if (IsSaveDataAvailable())
        {
            LoadGame();
        }
    }

    private void OnSettingsButtonClicked()
    {
        OpenSettingsPopup();
    }

    private void OnExitGameButtonClicked()
    {
        Application.Quit();
    }

    private void ShowConfirmationPopup(string message)
    {
        confirmationMessage.text = message;
        confirmationPopup.SetActive(true);
    }

    private void OnConfirmYesClicked()
    {
        StartNewGame();
        confirmationPopup.SetActive(false);
    }

    private void OnConfirmNoClicked()
    {
        confirmationPopup.SetActive(false);
    }

    private void StartNewGame()
    {
        dataManager.ResetAllData();

        SceneManager.LoadScene("MainGameScene");

        Debug.Log("�� ������ ���۵Ǿ����ϴ�.");
    }

    private void LoadGame()
    {
        dataManager.LoadAllData();

        SceneManager.LoadScene("MainGameScene");

        Debug.Log("����� �����͸� �ҷ��� ������ �����մϴ�.");
    }

    private void OpenSettingsPopup()
    {
        settingsPopup.SetActive(true);
        LoadCurrentSettings();
    }

    private void OnSettingsSaveClicked()
    {
        SaveSettings();
        settingsPopup.SetActive(false);
    }

    private void SaveSettings()
    {
        if (fullscreenToggle.isOn)
        {
            // ��üȭ�� ��� ����
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
            PlayerPrefs.SetInt("ScreenMode", 1); // 1: Fullscreen
        }
        else if (windowedToggle.isOn)
        {
            // ������ ��� ����
            Screen.fullScreenMode = FullScreenMode.Windowed;
            PlayerPrefs.SetInt("ScreenMode", 0); // 0: Windowed
        }

        PlayerPrefs.Save();
        Debug.Log("������ ����Ǿ����ϴ�.");
    }

    private void LoadCurrentSettings()
    {
        int screenMode = PlayerPrefs.GetInt("ScreenMode", 1); // �⺻���� ��üȭ��

        if (screenMode == 1)
        {
            fullscreenToggle.isOn = true;
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        }
        else
        {
            windowedToggle.isOn = true;
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }
    }

    
}
