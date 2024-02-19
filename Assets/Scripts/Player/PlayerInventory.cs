using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{

    [SerializeField] Weapon weapon;

    [SerializeField] ActiveItem activeItem;

    [SerializeField] PassiveItem passiveItem;

    [SerializeField] int gold;

    public Weapon CurrentWeapon => weapon;

    public void EquipWeapon(Weapon wep)
    {
        weapon = wep;
    }
    public void EquipActiveItem(Item item)
    {
        if (activeItem != null)
        {
            activeItem.Drop();
        }
        activeItem = (ActiveItem)item;
    }

    public void EquipPassiveItem(Item item)
    {
        if (passiveItem != null)
        {
            passiveItem.Drop();
        }
        passiveItem = (PassiveItem)item;
    }

    public void CollectGold(int amount)
    {
        gold += amount;
    }

    public bool RemoveGold(int amount)
    {
        int temp = gold;
        if(temp - amount < 0)
        {
            return false;
        }
        gold -= amount;
        return true;
    }
}
