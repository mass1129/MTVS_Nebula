using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUIManger : MonoBehaviour
{
    public K_UserInterface[] inventoryArray;
    bool isShowed = false;
    public GameObject tabButton;
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
        if(isShowed)
        {
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
            tabButton.SetActive(false);
        }

    }

    
}
