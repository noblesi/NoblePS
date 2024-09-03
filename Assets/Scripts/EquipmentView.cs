using UnityEngine;
using UnityEngine.UI;

public class EquipmentView : MonoBehaviour, IEquipmentView
{
    public Image weaponIcon;
    public Image armorIcon;
    public Image helmetIcon;
    public Image bootsIcon;

    private EquipmentPresenter presenter;

    public void Initialize(EquipmentPresenter equipmentPresenter)
    {
        presenter = equipmentPresenter;
        presenter.Initialize();
    }

    public void DisplayEquipment(EquipmentModel equipment)
    {
        weaponIcon.sprite = equipment.Weapon != null ? equipment.Weapon.Icon : null;
        armorIcon.sprite = equipment.Armor != null ? equipment.Armor.Icon : null;
        helmetIcon.sprite = equipment.Helmet != null ? equipment.Helmet.Icon : null;
        bootsIcon.sprite = equipment.Boots != null ? equipment.Boots.Icon : null;
    }

    public void OnEquipButtonClicked(Item item)
    {
        presenter.EquipItem(item);  // 버튼 클릭 시 아이템 장착
    }

    public void OnUnequipButtonClicked(ItemType itemType)
    {
        presenter.UnequipItem(itemType);  // 버튼 클릭 시 아이템 해제
    }
}
