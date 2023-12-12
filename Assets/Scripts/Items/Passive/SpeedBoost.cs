using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Pass_", menuName = "United/Pickups/PassiveItem/SpeedBoost")]
public class SpeedBoost : PassiveItem
{
    float speedBoost = 1f;

    public override void PickUp(PlayerInventory eq)
    {
        eq.EquipPassiveItem(this);
    }
    public override void SetupItem(PlayerInventory eq)
    {
        GameManager.Instance.PlayerController.Speed += speedBoost;  
    }
    public override void UpdateItem(PlayerInventory eq)
    {
      
    }
    public override void Drop()
    {
        GameManager.Instance.PlayerController.Speed -= speedBoost;
        PickupContainer spawnedPickup = Instantiate(P_PickupContainer, GameManager.Instance.PlayerController.transform);
        spawnedPickup.pickable = this;
        spawnedPickup.transform.parent = null;
    }
}
