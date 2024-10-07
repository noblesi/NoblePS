using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
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
    }

    public void UpdateSlot(Equipment equipment)
    {
        if (equipment != null)
        {
            icon.gameObject.SetActive(true);
            icon.sprite = equipment.GetIcon();
            itemDescription = equipment.Description;
        }
        else
        {
            ClearSlot();
        }
    }

    public void ClearSlot()
    {
        icon.sprite = null;
        icon.gameObject.SetActive(false);
        itemDescription = "";
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
}
