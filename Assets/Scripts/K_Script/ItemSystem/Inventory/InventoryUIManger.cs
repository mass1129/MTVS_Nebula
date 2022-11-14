using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUIManger : MonoBehaviour
{
    public K_UserInterface[] inventoryArray;
    bool isShowed = false;
    public GameObject tabButton;
    public K_PlayerItemSystem playerItem;
    public void ShowSelectWindow(int i)
    {   
        for(int j = 0; j < inventoryArray.Length; j++)
        {
            if(j==i)
            {
                inventoryArray[j].inventoryWindow.SetActive(true);
            }
            else
                inventoryArray[j].inventoryWindow.SetActive(false);
        }
    }

    public void ShowStart()
    {
        isShowed = !isShowed;
        string avatarName = PlayerPrefs.GetString("AvatarName");
        if (isShowed)
        {
            //playerItem.ItemLoad(avatarName);
           tabButton.SetActive(true);
            for (int j = 0; j < inventoryArray.Length; j++)
            {
                if (j == 1)
                {
                    inventoryArray[j].inventoryWindow.SetActive(true);
                }
                else
                    inventoryArray[j].inventoryWindow.SetActive(false);
            }
        }
        else
        {
            for (int j = 0; j < inventoryArray.Length; j++)
            {

                inventoryArray[j].inventoryWindow.SetActive(false);
            }
            //playerItem.ItemSave(avatarName);
            tabButton.SetActive(false);
        }

    }

    
}
