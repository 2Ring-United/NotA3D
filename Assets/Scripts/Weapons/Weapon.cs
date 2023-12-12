using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Weap_",menuName = "United/Pickups/Weapon")]
public class Weapon : Pickable
{
    public float Damage = 10f;
    public float AttackCooldownTime = 1f;
    public float KnockbackForce = 0.5f;
    public override void PickUp(PlayerInventory eq)
    {
        eq.EquipWeapon(this);
    }

    public override void Drop()
    {
        PickupContainer spawnedPickup = Instantiate(P_PickupContainer, GameManager.Instance.PlayerController.transform);
        spawnedPickup.pickable = this;
        spawnedPickup.transform.parent = null;
    }

}
