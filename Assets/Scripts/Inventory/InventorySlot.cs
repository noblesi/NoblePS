using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Item item;  // 슬롯에 있는 아이템
    public Image icon;  // 아이템 아이콘
    public Text quantityText;  // 아이템 수량 표시
    public InventoryPresenter inventoryPresenter;
    public Tooltip tooltip;

    public void Initialize(InventoryPresenter presenter, Tooltip tooltip)
    {
        inventoryPresenter = presenter;
        this.tooltip = tooltip;
        UpdateSlot();
    }

    public bool IsEmpty()
    {
        return item == null;
    }

    // 슬롯 업데이트 (아이콘과 수량 표시)
    public void UpdateSlot()
    {
        if(item != null)
        {
            icon.sprite = item.GetIcon();
            icon.gameObject.SetActive(true);

            quantityText.text = item.Quantity > 1 ? item.Quantity.ToString() : "";
            quantityText.enabled = item.Quantity > 1;
        }
        else
        {
            icon.gameObject.SetActive(false);
            quantityText.enabled = false;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right && item != null)
        {
            EquipItemIfPossible();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowTooltipIfNeeded();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HideTooltipIfNeeded();
    }

    private void ShowTooltipIfNeeded()
    {
        if (item != null && !string.IsNullOrEmpty(item.Description) && tooltip != null)
        {
            tooltip.ShowTooltip(item);
        }
    }

    private void HideTooltipIfNeeded()
    {
        if (tooltip != null)
        {
            tooltip.HideTooltip();
        }
    }

    private void EquipItemIfPossible()
    {
        if (item == null) return;

        if (inventoryPresenter.CanEquipItem(item))
        {
            inventoryPresenter.EquipItem(item);  // 장비 착용
            ClearSlot();
        }
        else
        {
            Debug.LogWarning("Cannot equip this item.");
        }
    }

    // 아이템 추가
    public void AddItem(Item newItem)
    {
        item = newItem;
        UpdateSlot();
    }

    // 아이템 제거
    public void RemoveItem()
    {
        item = null;
        UpdateSlot();
    }

    public void ClearSlot()
    {
        item = null;
        icon.sprite = null;
        icon.gameObject.SetActive(false);
        quantityText.text = "";
        quantityText.enabled = false;
    }
}
