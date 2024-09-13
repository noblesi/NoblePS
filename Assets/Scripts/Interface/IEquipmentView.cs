using System.Collections.Generic;

public interface IEquipmentView
{
    void DisplayEquipment(Dictionary<EquipmentType, Equipment> equipment);
    void UpdateSlot(EquipmentType equipmentType, Equipment equipment);
}
