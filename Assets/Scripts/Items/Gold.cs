using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "United/Pickups/Gold")]
public class Gold : Pickable
{
    public int amount = 50;
    public override void PickUp(PlayerInventory eq)
    {
        eq.CollectGold(amount);
    }

    public override void Drop()
    {
        PickupContainer spawnedPickup = Instantiate(P_PickupContainer, GameManager.Instance.PlayerController.transform);
        spawnedPickup.pickable = this;
        spawnedPickup.transform.parent = null;
    }
}
