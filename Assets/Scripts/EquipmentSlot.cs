using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image icon;
    public Image slotBackground;
    public EquipmentType equipmentType;

    private EquipmentPresenter equipmentPresenter;
    private string itemDescription;

    public void Initialize(EquipmentPresenter presenter, EquipmentType type)
    {
        equipmentPresenter = presenter;
        equipmentType = type;
        UpdateSlot();
    }

    public void UpdateSlot()
    {
        Equipment equippedItem = equipmentPresenter.GetItemInSlot(equipmentType) as Equipment;
        
        if (equippedItem != null && !string.IsNullOrEmpty(equippedItem.IconPath))
        {
            icon.sprite = equippedItem.GetIcon();
            icon.gameObject.SetActive(true);
            itemDescription = equippedItem.Description;
        }
        else
        {
            icon.sprite = null;
            icon.gameObject.SetActive(false);
            itemDescription = "";
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Equipment equippedItem = equipmentPresenter.GetItemInSlot(equipmentType) as Equipment;

        if(equippedItem != null)
        {
            equipmentPresenter.ShowItemDescription(equippedItem);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        equipmentPresenter.HideItemDescription();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            equipmentPresenter.UnequipItem(equipmentType);
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        InventorySlot draggedItemSlot = eventData.pointerDrag.GetComponent<InventorySlot>();
        if(draggedItemSlot != null && draggedItemSlot.item != null)
        {
            equipmentPresenter.EquipItem(draggedItemSlot.item);
        }
    }
}
