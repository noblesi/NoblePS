using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterHpBar : MonoBehaviour
{
    private Slider hpSlider;
    private Camera mainCamera;

    private void Awake()
    {
        hpSlider = GetComponentInChildren<Slider>();
        mainCamera = Camera.main;
    }

    public void Initialize(MonsterFSM monsterFSM)
    {
        monsterFSM.OnHealthChanged += UpdateHpBar;
    }

    private void Update()
    {
        if(mainCamera != null)
        {
            transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward,
                mainCamera.transform.rotation * Vector3.up);
        }
    }

    private void UpdateHpBar(int currentHP, int maxHP)
    {
        if(hpSlider != null)
        {
            hpSlider.maxValue = maxHP;
            hpSlider.value = currentHP;
        }
    }
}
