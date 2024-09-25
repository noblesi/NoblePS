using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private Collider weaponCollider;

    private void Awake()
    {
        weaponCollider = GetComponent<Collider>();
        if(weaponCollider != null)
        {
            weaponCollider.isTrigger = true;
            weaponCollider.enabled = false;
        }
    }

    public void OnWeaponEnable()
    {
        if(weaponCollider != null)
        {
            weaponCollider.enabled = true;
        }
    }

    public void OnWeaponDisable()
    {
        if(weaponCollider != null)
        {
            weaponCollider.enabled = false;
        }
    }
}
