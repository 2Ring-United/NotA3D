using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Activ_", menuName = "United/Pickups/ActiveItem")]
public class MagicShield : ActiveItem
{
    public float InvincibilityTime = 3f;
    public override void PickUp(PlayerInventory eq)
    {
        eq.EquipActiveItem(this);
    }

    public override void SetupItem(PlayerInventory eq)
    {
       
    }

    public override void UpdateItem(PlayerInventory eq)
    {
       
    }

    public override void ActivateItem(PlayerInventory eq)
    {
        
    }

    public override void Drop()
    {
        PickupContainer spawnedPickup = Instantiate(P_PickupContainer, GameManager.Instance.PlayerController.transform);
        spawnedPickup.pickable = this;
        //spawnedPickup.transform.position = spawnedPickup.transform.parent.position;
        spawnedPickup.transform.parent = null;
    }
}

