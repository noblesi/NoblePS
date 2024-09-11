using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Item item;  // ���Կ� �ִ� ������
    public Image icon;  // ������ ������
    public Text quantityText;  // ������ ���� ǥ��
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

    // ���� ������Ʈ (�����ܰ� ���� ǥ��)
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
            HandleRightClick();
        }
    }

    private void HandleRightClick()
    {
        if(item is Equipment)
        {
            EquipItem();
        }
        else if(item is Consumable)
        {
            UseConsumable();
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

    private void UseConsumable()
    {
        Consumable consumableItem = item as Consumable;
        if(consumableItem != null)
        {
            ApplyConsumableEffects(consumableItem);

            item.Quantity--;
            if(item.Quantity <= 0)
            {
                ClearSlot();
            }
            else
            {
                UpdateSlot();
            }
        }
    }

    private void ApplyConsumableEffects(Consumable consumable)
    {
        Debug.Log($"Used {consumable.ItemName}: Restored {consumable.HealthRestore} HP and {consumable.ManaRestore} MP");
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

    

    // ������ �߰�
    public void AddItem(Item newItem)
    {
        item = newItem;
        UpdateSlot();
    }

    // ������ ����
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
