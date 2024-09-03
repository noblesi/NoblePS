using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    public Text tooltipText;
    public GameObject tooltipPanel;

    private void Start()
    {
        HideTooltip();
    }

    public void ShowTooltip(string text)
    {
        tooltipPanel.SetActive(false);
        tooltipText.text = text;
    }

    public void HideTooltip()
    {
        tooltipPanel.SetActive(false);
    }
}
