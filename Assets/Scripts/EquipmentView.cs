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

    public void Initialize(EquipmentPresenter equipmentPresenter)
    {
        presenter = equipmentPresenter;

        weaponSlot.Initialize(presenter, EquipmentType.Weapon);
        armorSlot.Initialize(presenter, EquipmentType.Armor);
        helmetSlot.Initialize(presenter, EquipmentType.Helmet);
        bootsSlot.Initialize(presenter, EquipmentType.Boots);
    }

    public void DisplayEquipment(EquipmentModel equipment)
    {
        weaponSlot.UpdateSlot();
        armorSlot.UpdateSlot();
        helmetSlot.UpdateSlot();
        bootsSlot.UpdateSlot();
    }

    public void UpdateSlot(EquipmentType equipmentType)
    {
        switch (equipmentType)
        {
            case EquipmentType.Weapon:
                weaponSlot.UpdateSlot();
                break;
            case EquipmentType.Armor:
                armorSlot.UpdateSlot();
                break;
            case EquipmentType.Helmet:
                helmetSlot.UpdateSlot();
                break;
            case EquipmentType.Boots:
                bootsSlot.UpdateSlot();
                break;
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
