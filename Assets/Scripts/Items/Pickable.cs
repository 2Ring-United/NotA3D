using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pickable : ScriptableObject
{
    public PickupTypes type;
    public Sprite sprite;
    public PickupContainer P_PickupContainer;
    abstract public void PickUp(PlayerInventory eq);
    abstract public void Drop();
}
