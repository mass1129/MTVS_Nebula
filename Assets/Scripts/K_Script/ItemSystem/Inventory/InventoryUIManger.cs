using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUIManger : MonoBehaviour
{
    public K_UserInterface[] inventoryArray;

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

}
