using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    [SerializeField] private Slider hpBar;
    [SerializeField] private Slider mpBar;
    [SerializeField] private Text hpText;
    [SerializeField] private Text mpText;
    [SerializeField] private RawImage miniMapImage;

    private PlayerData playerData;

    public void Initialize(PlayerData playerData)
    {
        this.playerData = playerData;
        UpdateHUD();
    }

    private void Update()
    {
        if(playerData != null)
        {
            UpdateHUD();
        }
    }

    private void UpdateHUD()
    {
        hpBar.value = (float)playerData.Status.CurrentHP / playerData.Status.MaxHP;
        mpBar.value = (float)playerData.Status.CurrentMP / playerData.Status.MaxMP;

        hpText.text = $"{playerData.Status.CurrentHP} / {playerData.Status.MaxHP}";
        mpText.text = $"{playerData.Status.CurrentMP} / {playerData.Status.MaxMP}";
    }
}
