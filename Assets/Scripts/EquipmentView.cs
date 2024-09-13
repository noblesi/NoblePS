using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentView : MonoBehaviour
{
    public EquipmentSlot weaponSlot;
    public EquipmentSlot armorSlot;
    public EquipmentSlot helmetSlot;
    public EquipmentSlot bootsSlot;

    public Text itemDescriptionText;
    private EquipmentPresenter presenter;

    private Dictionary<EquipmentType, EquipmentSlot> slotDictionary = new Dictionary<EquipmentType, EquipmentSlot>();

    public void Initialize(EquipmentPresenter equipmentPresenter)
    {
        presenter = equipmentPresenter;

        slotDictionary[EquipmentType.Weapon] = weaponSlot;
        slotDictionary[EquipmentType.Armor] = armorSlot;
        slotDictionary[EquipmentType.Helmet] = helmetSlot;
        slotDictionary[EquipmentType.Boots] = bootsSlot;

        foreach(var slot in slotDictionary.Values)
        {
            slot.Initialize(equipmentPresenter, slot.equipmentType);
        }
    }

    public void DisplayEquipment(Dictionary<EquipmentType, Equipment> equippedItems)
    {
        foreach(var itemPair in equippedItems)
        {
            UpdateSlot(itemPair.Key, itemPair.Value);
        }
    }

    public void UpdateSlot(EquipmentType equipmentType, Equipment equipment)
    {
        if(slotDictionary.TryGetValue(equipmentType, out EquipmentSlot slot))
        {
            slot.UpdateSlot(equipment);
        }
    }

    public void ShowItemDescription(string description)
    {
        itemDescriptionText.text = description;
        itemDescriptionText.gameObject.SetActive(true);
    }

    public void HideItemDescription()
    {
        itemDescriptionText.gameObject.SetActive(false);
    }
}
