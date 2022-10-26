using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSystemAssets : MonoBehaviour
{
    public static BuildingSystemAssets Instance { get; private set; }


    private void Awake()
    {
        Instance = this;
    }


    public PlacedObjectTypeSO[] placedObjectTypeSOArray;
    public PlacedObjectTypeSO GetPlacedObjectTypeSOFromName(string placedObjectTypeSOName)
    {
        foreach (PlacedObjectTypeSO placedObjectTypeSO in placedObjectTypeSOArray)
        {
            if (placedObjectTypeSO.name == placedObjectTypeSOName)
            {
                return placedObjectTypeSO;
            }
        }
        return null;
    }

}
