using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallStone : MonoBehaviour
{
    [SerializeField]
    private Item item;
    public Item TakeItem()
    {
        Destroy(gameObject);
        return item;
    }
}
