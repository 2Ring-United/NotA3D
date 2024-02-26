using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Activ_", menuName = "United/Pickups/ActiveItem/MagicShield")]
public class MagicShield : ActiveItem
{
    public float InvincibilityTime = 3f;
    public float CooldownTimer = 10f;
    private float _invincibility = 0f;
    private float _cooldown = 0f;
    private bool isOn = false;
    private bool canActivate = true;
    public override void PickUp(PlayerInventory eq)
    {
        eq.EquipActiveItem(this);
    }

    public override void SetupItem(PlayerInventory eq)
    {
       
    }

    public override void UpdateItem(PlayerInventory eq)
    {
        if (isOn)
        {
            if(_invincibility <= InvincibilityTime)
            {
                _invincibility += Time.deltaTime;
            }
            else
            {
                _invincibility = 0f;
                isOn = false;
                GameManager.Instance.PlayerController.Collision.SetCollsionEnabled(true);

            }
        }

        if (!canActivate)
        {
            if (_cooldown <= CooldownTimer)
            {
                _cooldown += Time.deltaTime;
            }
            else
            {
                _cooldown = 0f;
                canActivate = true;
                GameManager.Instance.PlayerController.Collision.SetCollsionEnabled(true);

            }
        }
    }

    public override void ActivateItem(PlayerInventory eq)
    {
        if (canActivate)
        {
            canActivate = false;
            isOn = true;
            GameManager.Instance.PlayerController.Collision.SetCollsionEnabled(false);
        }
        
    }

    public override void Drop()
    {
        PickupContainer spawnedPickup = Instantiate(P_PickupContainer, GameManager.Instance.PlayerController.transform);
        spawnedPickup.pickable = this;
        //spawnedPickup.transform.position = spawnedPickup.transform.parent.position;
        spawnedPickup.transform.parent = null;
    }
}

