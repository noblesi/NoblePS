using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item item;

    public void Initialize(Item itemData)
    {
        item = itemData;
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
        }
    }
}
