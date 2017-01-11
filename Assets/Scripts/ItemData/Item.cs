using UnityEngine;
using System.Collections;

public class Item {
    public int id;
    public string itemName;
    public int ability;
    public string description;
    public int cost;
    public int stackSize;
    public int remainingUses;
    public int throwAbility;
    public int throwFlatPower;
    public float throwModPower;
    public int throwSplashRange;

    public Item clone()
    {
        return (Item)this.MemberwiseClone();
    }
}
