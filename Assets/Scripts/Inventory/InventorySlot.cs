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

    public int slotIndex;

    public void Initialize(InventoryPresenter presenter, Tooltip tooltip, int index)
    {
        inventoryPresenter = presenter;
        this.tooltip = tooltip;
        this.slotIndex = index;
    }

    public bool IsEmpty()
    {
        return item == null;
    }

    public void UpdateSlot(Item newItem)
    {
        item = newItem;
        if (item != null)
        {
            icon.sprite = item.GetIcon();
            icon.gameObject.SetActive(true);
            quantityText.text = item.Quantity > 1 ? item.Quantity.ToString() : "";
            quantityText.enabled = item.Quantity > 1;
        }
        else
        {
            ClearSlot();
        }
    }

    public void ClearSlot()
    {
        item = null;
        icon.sprite = null;
        icon.gameObject.SetActive(false);
        quantityText.text = "";
        quantityText.enabled = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right && item != null)
        {
            HandleRightClick();
        }
    }

    private void HandleRightClick()
    {
        if(item is Equipment)
        {
            EquipItem();
        }
        else if(item is Consumable consumableItem)
        {
            UseConsumable(consumableItem);
        }
    }

    private void EquipItem()
    {
        if (inventoryPresenter.CanEquipItem(item))
        {
            Equipment equipmentItem = item as Equipment;
            if(equipmentItem != null)
            {
                inventoryPresenter.EquipItem(equipmentItem);
                ClearSlot();
            }
        }
        else
        {
            Debug.LogWarning("Cannot equip this item.");
        }
    }

    private void UseConsumable(Consumable consumableItem)
    {
        PlayerFSM player = FindObjectOfType<PlayerFSM>();
        if(player != null)
        {
            if(consumableItem.HealthRestore > 0)
            {
                player.HealPlayer(consumableItem.HealthRestore);
            }

            if(consumableItem.ManaRestore > 0)
            {
                player.RestoreMana(consumableItem.ManaRestore);
            }

            item.Quantity--;

            if(item.Quantity <= 0)
            {
                inventoryPresenter.RemoveItem(slotIndex);
            }
            else
            {
                UpdateSlot(item);
            }
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
        if (item != null && tooltip != null)
        {
            tooltip.ShowTooltip(item);
        }
        else
        {
            tooltip.HideTooltip();
        }
    }

    private void HideTooltipIfNeeded()
    {
        if (tooltip != null)
        {
            tooltip.HideTooltip();
        }
    }

    // 아이템 추가
    public void AddItem(Item newItem)
    {
        UpdateSlot(newItem);
    }
}
