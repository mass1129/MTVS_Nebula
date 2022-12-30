using UnityEngine;

[CreateAssetMenu(fileName = "NewBuildingSystemAssetsDatabase", menuName = "Inventory System/Items/Building/Database")]
public class BuildingSystemAssetsDatabase : ScriptableObject
{

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
