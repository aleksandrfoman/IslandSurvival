using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [SerializeField]
    private Color defaultColor;
    [SerializeField]
    private Color selectedColor;
    [SerializeField]
    private Image backImage;
    [SerializeField]
    private Image iconSlot;
    [SerializeField]
    private TMP_Text amountText;
    [SerializeField]
    private TMP_Text selectedNumberText;
    [SerializeField]
    private Item item;
    public Item Item => item;

    public void SetItem(Item item)
    {
        this.item = item;
    }

    public void ClearItem()
    {
        item = null;
    }
    public void SelectedBackImage()
    {
        backImage.color = selectedColor;
        
    }
    public void DefalutBackImage()
    {
        backImage.color = defaultColor;
    }
    public void UpdateInformation()
    {
        if (item != null)
        {
            iconSlot.gameObject.SetActive(true);
            amountText.gameObject.SetActive(true);
            iconSlot.sprite = item.IconItem;
            amountText.text = item.CurrentAmount.ToString() + "/" + item.MaxAmount.ToString();
        }
        else
        {
            iconSlot.gameObject.SetActive(false);
            amountText.gameObject.SetActive(false);
        }
        selectedNumberText.gameObject.SetActive(false);
    }
    public void UpdateInformation(int indexQuickPanel)
    {
        selectedNumberText.gameObject.SetActive(true);
        selectedNumberText.text = indexQuickPanel.ToString();

        if (item != null)
        {
            iconSlot.gameObject.SetActive(true);
            amountText.gameObject.SetActive(true);
            iconSlot.sprite = item.IconItem;
            amountText.text = item.CurrentAmount.ToString() + "/" + item.MaxAmount.ToString();
        }
        else
        {
            iconSlot.gameObject.SetActive(false);
            amountText.gameObject.SetActive(false);
        }
    }

    
}
