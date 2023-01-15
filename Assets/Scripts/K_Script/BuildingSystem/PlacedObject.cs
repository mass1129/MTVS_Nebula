using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;


public class PlacedObject : MonoBehaviourPun
{
    public PlacedObjectTypeSO placedObjectTypeSO;
    public Vector2Int origin;
    public PlacedObjectTypeSO.Dir dir;
    //인스턴스 유무에 관계없이 실행할수있는 정적 메소드
    //네트워크 객체로 모든플레이어에게 동일하게 생성한다.
    //네트워크 객체로 생성하지 않을려면 건물 속성들을 전부 동기화 시켜서 각 클라이언트에서 만들어야 한다.
    //네트워크 객체로 생성하려면 각 건물에 photonview가 붙어야 한다. 각 클라이언트는 보유할수있는 viewiD가 제한이 있기때문에 건물이 많아질때는 좋은 방법은 아니다.
    public static PlacedObject Create(Vector3 worldPosition, Vector2Int origin, PlacedObjectTypeSO.Dir dir, PlacedObjectTypeSO placedObjectTypeSO)
    {
        
        GameObject placedObjectTransform = PhotonNetwork.InstantiateRoomObject(Path.Combine(placedObjectTypeSO.bundleFolderName, placedObjectTypeSO.prefab.name), worldPosition, Quaternion.Euler(0, placedObjectTypeSO.GetRotationAngle(dir), 0));

        PlacedObject placedObject = placedObjectTransform.transform.GetComponent<PlacedObject>();
        placedObject.placedObjectTypeSO = placedObjectTypeSO;
        placedObject.origin = origin;
        placedObject.dir = dir;

        placedObject.Setup();
        return placedObject;
    }
    //유저월드 매니저에 건물 리스트 추가 OR 제거
    [PunRPC]
    public void RPCPlaceObjListUpdate(bool toAdd)
    {
        if (toAdd)
            K_UserWorldMgr.instance.loadObjectList.Add(this);
        else
            K_UserWorldMgr.instance.loadObjectList.Remove(this);
    }
    void Setup() {
        photonView.RPC("RPCPlaceObjListUpdate", RpcTarget.AllBuffered, true);
    }
    public void DestroySelf()
    {


        photonView.RPC("RPCPlaceObjListUpdate", RpcTarget.AllBuffered, false);
        PhotonNetwork.Destroy(photonView);

    }
    


    public List<Vector2Int> GetGridPositionList() {
        return placedObjectTypeSO.GetGridPositionList(origin, dir);
    }

   
    //직렬화 데이터 생성후 반환
    public SaveObject GetSaveObject()
    {
        return new SaveObject
        {
            placedObjectTypeSOName = placedObjectTypeSO.name,
            origin = origin,
            dir = dir,

        };
    }



    [System.Serializable]
    public class SaveObject
    {

        public string placedObjectTypeSOName;
        public Vector2Int origin;
        public PlacedObjectTypeSO.Dir dir;
    }

}
