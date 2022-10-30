using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Database", menuName = "Inventory System/Items/Database")]
public class ItemDatabaseObject : ScriptableObject
{
    public ItemObject[] ItemObjects;

    [ContextMenu("Update ID's")]
    public void UpdateID()
    {
        for (int i = 0; i < ItemObjects.Length; i++)
        {
            if (ItemObjects[i].data.Id != i)
                ItemObjects[i].data.Id = i;
        }
    }
    public void OnValidate()
    {
        for (int i = 0; i < ItemObjects.Length; i++)
        {
            ItemObjects[i].data.Id = i;
        }
    }

}
