using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    public Text tooltipText;
    public RectTransform backgroundRectTransform;

    public RectTransform inventoryPanel;
    private Vector3 offset = new Vector3(-10f, 0, 0);

    private void Awake()
    {
        if(tooltipText == null || backgroundRectTransform == null)
        {
            Debug.LogError("Tooltip : Missing UI Components");
            return;
        }

        HideTooltip();
    }

    public void ShowTooltip(Item item)
    {
        if (tooltipText == null || backgroundRectTransform == null)
        {
            Debug.LogError("Tooltip: UI components are not initialized.");
            return;
        }

        gameObject.SetActive(true);
        
        string tooltipContent = $"<b>{item.ItemName}</b>\n" +
                                $"Type: {item.Type}\n" +
                                $"{item.Description}";

        tooltipText.text = tooltipContent;
    }

    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }
}
