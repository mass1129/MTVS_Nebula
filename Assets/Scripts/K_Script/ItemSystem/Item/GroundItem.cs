using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundItem : MonoBehaviour
{
    public ItemObject item;

    public ItemDatabaseObject database;
    //private void Start()
    //{
        
    //}
    //public void SetItem(int id, int uniqueId)
    //{
    //    photonView.RPC("RPCSetItem", RpcTarget.AllBuffered, id, uniqueId);
    //}
    //[PunRPC]
    //public void RPCSetItem(int id, int uniqueId)
    //{
    //    item = database.ItemObjects[id];
    //    item.data.uniqueId = uniqueId;
    //}
    //public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    //{
        
    //}
}
