using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName= "Pickup_", menuName = "United/Pickups/Armor")]
public class ArmorPickup : Pickable
{
    public float amount = 1;
    public override void PickUp(PlayerInventory eq)
    {
        GameManager.Instance.PlayerController.AddArmor(amount);
    }

    public override void Drop()
    {
        PickupContainer spawnedPickup = Instantiate(P_PickupContainer, GameManager.Instance.PlayerController.transform);
        spawnedPickup.pickable = this;
        spawnedPickup.transform.parent = null;
    }
}
