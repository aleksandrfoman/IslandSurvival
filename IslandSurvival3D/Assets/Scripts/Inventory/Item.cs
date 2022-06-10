using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item",menuName ="ScriptableObjects/Items/NewItem")]
public class Item : ScriptableObject
{
    [SerializeField]
    private string itemName;
    public string ItemName => itemName;
    [SerializeField]
    private Sprite iconItem;
    public Sprite IconItem => iconItem;
    [SerializeField]
    private int currentAmount;
    public int CurrentAmount => currentAmount;

    [SerializeField]
    private int maxAmount;
    public int MaxAmount => maxAmount;

    private bool isFullAmount = false;
    public bool IsFullAmount => isFullAmount;

    public void SetAmount(int amount)
    {
        currentAmount = amount;
    }
    public int AddItemAmount(Item addItem)
    {
        int edgeAmount = 0;
        currentAmount += addItem.currentAmount;
        if (currentAmount >= maxAmount)
        {
            isFullAmount = true;
            edgeAmount = currentAmount - maxAmount;
            currentAmount = maxAmount;
        }
        return edgeAmount;
    }

    public Item Clone()
    {
        var clone = Instantiate(this);
        return clone;
    }
}
