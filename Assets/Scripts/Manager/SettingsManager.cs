using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public Slider volumeSlider;
    public Toggle windowedModeToggle;
    public Toggle fullscreenModeToggle;
    public Text resolutionText;

    private string settingsFilePath;
    private Resolution[] resolutions;
    private int currentResolutionIndex;

    private void Start()
    {
        settingsFilePath = Path.Combine(Application.persistentDataPath, "settings.json");

        resolutions = Screen.resolutions;
        LoadSettings();

        InitializeUI();
    }

    private void InitializeUI()
    {
        windowedModeToggle.onValueChanged.AddListener(OnWindowedModeToggleChanged);
        fullscreenModeToggle.onValueChanged.AddListener(OnFullscreenModeToggleChanged);
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);

        resolutionText.text = $"{resolutions[currentResolutionIndex].width} x {resolutions[currentResolutionIndex].height}";

        windowedModeToggle.isOn = !Screen.fullScreen;
        fullscreenModeToggle.isOn = Screen.fullScreen;
    }

    public void OnVolumeChanged(float volume)
    {
        AudioListener.volume = volume;
        SaveSettings();
    }

    private void OnWindowedModeToggleChanged(bool isWindowed)
    {
        if (isWindowed)
        {
            fullscreenModeToggle.isOn = false;
            Screen.SetResolution(resolutions[currentResolutionIndex].width, resolutions[currentResolutionIndex].height, false);
            SaveSettings();
        }
    }

    private void OnFullscreenModeToggleChanged(bool isFullscreen)
    {
        if(isFullscreen)
        {
            windowedModeToggle.isOn = false;
            Screen.SetResolution(resolutions[currentResolutionIndex].width, resolutions[currentResolutionIndex].height, true);
            SaveSettings();
        }
    }

    public void ChangeResolution(int direction)
    {
        currentResolutionIndex += direction;
        if (currentResolutionIndex >= resolutions.Length)
        {
            currentResolutionIndex = 0;
        }
        else if(currentResolutionIndex < 0)
        {
            currentResolutionIndex = resolutions.Length - 1;
        }

        Screen.SetResolution(resolutions[currentResolutionIndex].width, resolutions[currentResolutionIndex].height, fullscreenModeToggle.isOn);
        resolutionText.text = $"{resolutions[currentResolutionIndex].width} x {resolutions[currentResolutionIndex].height}";
        SaveSettings();
    }

    private void LoadSettings()
    {
        if (File.Exists(settingsFilePath))
        {
            string json = File.ReadAllText(settingsFilePath);
            SettingsData settings = JsonUtility.FromJson<SettingsData>(json);

            volumeSlider.value = settings.volume;
            fullscreenModeToggle.isOn = settings.isFullscreen;
            windowedModeToggle.isOn = !settings.isFullscreen;
            currentResolutionIndex = settings.resolutionIndex;

            AudioListener.volume = settings.volume;
            Screen.fullScreen = settings.isFullscreen;
            Screen.SetResolution(resolutions[currentResolutionIndex].width, resolutions[currentResolutionIndex].height, settings.isFullscreen);
            resolutionText.text = $"{resolutions[currentResolutionIndex].width} x {resolutions[currentResolutionIndex].height}";
        }
        else
        {
            volumeSlider.value = 1.0f;
            windowedModeToggle.isOn = true; // 기본값은 창 모드
            fullscreenModeToggle.isOn = false;
            currentResolutionIndex = resolutions.Length - 1; // 최대 해상도로 설정
            resolutionText.text = $"{resolutions[currentResolutionIndex].width} x {resolutions[currentResolutionIndex].height}";
        }
    }

    private void SaveSettings()
    {
        SettingsData settings = new SettingsData(volumeSlider.value, fullscreenModeToggle.isOn, currentResolutionIndex);
        string json = JsonUtility.ToJson(settings, true);
        File.WriteAllText(settingsFilePath, json);
    }
}

[System.Serializable]
public class SettingsData
{
    public float volume;
    public bool isFullscreen;
    public int resolutionIndex;

    public SettingsData(float volume, bool isFullscreen, int resolutionIndex)
    {
        this.volume = volume;
        this.isFullscreen = isFullscreen;
        this.resolutionIndex = resolutionIndex;
    }
}
