using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item item;

    public void Initialize(Item itemData)
    {
        if(itemData != null)
        {
            item = itemData;
        }
        else
        {
            Debug.LogError("ItemPickup : Item data is null.");
        }

        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerFSM playerFSM = other.GetComponent<PlayerFSM>();
            if(playerFSM != null)
            {
                playerFSM.PickupItem(item);
                Destroy(gameObject);
            }
            else
            {
                Debug.LogError("ItemPickup: Player or Item i null.");
            }
        }
    }
}
