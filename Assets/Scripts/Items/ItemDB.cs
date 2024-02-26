using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct ItemDBEntry
{
    public int ID;
    public Item Object;
}

[CreateAssetMenu]
public class ItemDB : ScriptableObject
{
    public List<ItemDBEntry> Items = new List<ItemDBEntry>();
}
