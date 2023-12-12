using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : Pickable
{

    public abstract void SetupItem(PlayerInventory eq);


    public abstract void UpdateItem(PlayerInventory eq);

}
