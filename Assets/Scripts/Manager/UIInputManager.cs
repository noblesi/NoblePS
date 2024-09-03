using UnityEngine;

public class UIInputManager : MonoBehaviour
{
    private void Update()
    {
        HandleUIInput();
    }

    private void HandleUIInput()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            UIManager.Instance.ToggleInventory();
        }

        if(Input.GetKeyDown(KeyCode.E))
        {
            UIManager.Instance.ToggleEquipment();
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            UIManager.Instance.ToggleStatus();
        }
    }
}
