using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField]
    private List<InventorySlot> inventorySlots = new List<InventorySlot>();
    [SerializeField]
    private int curIndexQuickSlot;
    [SerializeField]
    private Player player;

    private void Start()
    {
        UpdatesSlotsInfo();
    }

    private void Update()
    {
        ChangeQuickSlots();
    }



    public void ChangeQuickSlots()
    {
        if (Input.anyKey)
        {
            for (int i = 0; i <= 9; i++)
            {
                if (Input.GetKeyDown(i.ToString()))
                {
                    if (i == 0)
                    {
                        curIndexQuickSlot = 9;
                    }
                    else
                    {
                        curIndexQuickSlot = i-1;
                    }
                    UpdateQuickSlots(curIndexQuickSlot);
                }
            }
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            curIndexQuickSlot++;
            if (curIndexQuickSlot > 9)
            {
                curIndexQuickSlot = 0;
            }
            UpdateQuickSlots(curIndexQuickSlot);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            curIndexQuickSlot--;
            if (curIndexQuickSlot < 0)
            {
                curIndexQuickSlot = 9;
            }
            UpdateQuickSlots(curIndexQuickSlot);
        }
    }

    private void UpdateQuickSlots(int selectIndex)
    {
        for (int i = 0; i <= 9; i++)
        {
            if (i == selectIndex)
            {
                inventorySlots[i].SelectedBackImage();
                player.ActivateWeapon(inventorySlots[i].Item);
            }
            else
            {
                inventorySlots[i].DefalutBackImage();
            }

        }
    }

    private void UpdatesSlotsInfo()
    {
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            if(i <= 9)
            {
                if (i == 0)
                {
                    inventorySlots[i].UpdateInformation(1);
                }
                else if(i == 9)
                {
                    inventorySlots[i].UpdateInformation(0);
                }
                else
                {
                    inventorySlots[i].UpdateInformation(i+1);
                }
            }
            else
            {
                inventorySlots[i].UpdateInformation();
            }
        }
    }

    public void AddItem(Item addItem)
    {
        int edge = 0;
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            if (inventorySlots[i].Item != null)
            {
                if (!inventorySlots[i].Item.IsFullAmount)
                {
                    if (inventorySlots[i].Item.IdName == addItem.IdName)
                    {
                        edge = inventorySlots[i].Item.AddItemAmount(addItem);
                        if (edge > 0)
                        {
                            continue;
                        }
                        break;
                    }
                }
            }
            else
            {
                if (edge != 0)
                {
                    inventorySlots[i].SetItem(addItem.Clone());
                    inventorySlots[i].Item.SetAmount(edge);
                }
                else
                {
                    inventorySlots[i].SetItem(addItem.Clone());
                }
                break;
            }

        }
        UpdatesSlotsInfo();
        UpdateQuickSlots(curIndexQuickSlot);
    }
}
