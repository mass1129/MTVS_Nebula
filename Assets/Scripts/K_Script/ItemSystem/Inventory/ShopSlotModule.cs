using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopSlotModule : MonoBehaviour
{
    public TextMeshProUGUI itemCostTxt;
    public TextMeshProUGUI itemNameTxt;
    public Button buyButton;
    public TextMeshProUGUI toolTipTitle;
    public TextMeshProUGUI toolTipDiscription;
    public Image slopImg;


    public void SetShopModule(InventorySlot slot, InventoryObject inventory)
    {
        ItemObject itemObj = slot.GetShopItemObject(inventory);
        if (!itemObj)
        {
            itemNameTxt.SetText("구매완료 번들");
            toolTipTitle.SetText("");
            toolTipDiscription.SetText("");
            slopImg.sprite = null;

        }
        else
        {
            itemNameTxt.SetText(itemObj.name.ToString());
            toolTipTitle.SetText(itemObj.name.ToString());
            toolTipDiscription.SetText(itemObj.description);
            slopImg.sprite = itemObj.uiDisplay;
           
        }
    }
}
