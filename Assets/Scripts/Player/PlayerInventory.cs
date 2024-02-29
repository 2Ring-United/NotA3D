using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{

    [SerializeField] Weapon _weapon;

    [SerializeField] ActiveItem _activeItem;

    [SerializeField] PassiveItem _passiveItem;

    [SerializeField] int gold;

    private void Start()
    {
        
    }

    void Update()
    {
        if(_activeItem != null)
        {
            _activeItem.UpdateItem(this);
        }
        if (_passiveItem != null)
        {
            _passiveItem.UpdateItem(this);
        }
    }

    public void EquipWeapon(Weapon wep)
    {
        if (_weapon != null)
        {
            _weapon.Drop();
        }
        _weapon = wep;
    }
    public void EquipActiveItem(Item item)
    {
        if (_activeItem != null)
        {
            _activeItem.Drop();
        }
        _activeItem = (ActiveItem)item;
    }

    public void EquipPassiveItem(Item item)
    {
        if (_passiveItem != null)
        {
            _passiveItem.Drop();
        }
        _passiveItem = (PassiveItem)item;
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

    public Weapon GetWeapon()
    {
        return _weapon;
    }

    public ActiveItem GetActiveItem()
    {
        return _activeItem;
    }

    public PassiveItem GetPassiveItem()
    {
        return _passiveItem;
    }



}
