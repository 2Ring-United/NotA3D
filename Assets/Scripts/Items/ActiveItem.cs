using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class ActiveItem : Item
{
    abstract public void ActivateItem(PlayerInventory eq);
}