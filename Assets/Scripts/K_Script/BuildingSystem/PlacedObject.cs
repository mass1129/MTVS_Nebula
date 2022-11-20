using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class PlacedObject : MonoBehaviourPun
{

    public static PlacedObject Create(Vector3 worldPosition, Vector2Int origin, PlacedObjectTypeSO.Dir dir, PlacedObjectTypeSO placedObjectTypeSO)
    {
        
        GameObject placedObjectTransform = PhotonNetwork.InstantiateRoomObject(Path.Combine(placedObjectTypeSO.bundleFolderName, placedObjectTypeSO.prefab.name), worldPosition, Quaternion.Euler(0, placedObjectTypeSO.GetRotationAngle(dir), 0));

        PlacedObject placedObject = placedObjectTransform.transform.GetComponent<PlacedObject>();
        placedObject.placedObjectTypeSO = placedObjectTypeSO;
        placedObject.origin = origin;
        placedObject.dir = dir;

        placedObject.Setup();
        return placedObject;
        //TestCreate(worldPosition, origin, dir, placedObjectTypeSO);
    }
   

    private PlacedObjectTypeSO placedObjectTypeSO;
    private Vector2Int origin;
    private PlacedObjectTypeSO.Dir dir;

    protected virtual void Setup() {
        //Debug.Log("PlacedObject.Setup() " + transform);
    }

    public virtual void GridSetupDone() {
        //Debug.Log("PlacedObject.GridSetupDone() " + transform);
    }

    protected virtual void TriggerGridObjectChanged() {
        foreach (Vector2Int gridPosition in GetGridPositionList()) {
            GridBuildingSystem3D.Instance.GetGridObject(gridPosition).TriggerGridObjectChanged();
        }
    }

    public Vector2Int GetGridPosition() {
        return origin;
    }

    public List<Vector2Int> GetGridPositionList() {
        return placedObjectTypeSO.GetGridPositionList(origin, dir);
    }

    public virtual void DestroySelf() {
        PhotonNetwork.Destroy(gameObject);
    }

    public override string ToString() {
        return placedObjectTypeSO.nameString;
    }


    public PlacedObject(PlacedObjectTypeSO placedObjectTypeSO, Vector2Int origin, PlacedObjectTypeSO.Dir dir)
    {
        this.placedObjectTypeSO = placedObjectTypeSO;
        this.origin = origin;
        this.dir = dir;
    }

    public SaveObject GetSaveObject() 
    {
        return new SaveObject {
            placedObjectTypeSOName = placedObjectTypeSO.name,
            origin = origin,
            dir = dir,
            
        };
    }

 

    [System.Serializable]
    public class SaveObject {

        public string placedObjectTypeSOName;
        public Vector2Int origin;
        public PlacedObjectTypeSO.Dir dir;
    }

}
