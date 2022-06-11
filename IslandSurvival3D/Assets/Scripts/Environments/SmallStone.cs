using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallStone : DropItem,ITakeable
{
    public Item TakeItem()
    {
        Destroy(gameObject);
        return item;
    }
}
