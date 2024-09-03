using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    public Text tooltipText;
    public GameObject tooltipPanel;

    private RectTransform panelRectTransform;

    private void Start()
    {
        panelRectTransform = tooltipPanel.GetComponent<RectTransform>();
        HideTooltip();
    }

    public void ShowTooltip(string text)
    {
        tooltipText.text = text;
        AdjustTooltipSize();
        tooltipPanel.SetActive(true);
    }

    public void HideTooltip()
    {
        tooltipPanel.SetActive(false);
    }

    private void AdjustTooltipSize()
    {
        Vector2 textSize = tooltipText.GetComponent<RectTransform>().sizeDelta;
        panelRectTransform.sizeDelta = new Vector2(panelRectTransform.sizeDelta.x, textSize.y + 20);  // 여유 공간 추가
    }
}
