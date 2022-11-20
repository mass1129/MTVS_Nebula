using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundItem : MonoBehaviourPun, IPunObservable
{
    public ItemObject item;

    public ItemDatabaseObject database;

    public void SetItem(int i)
    {
        photonView.RPC("RPCSetItem", RpcTarget.AllBuffered, i);
    }
    [PunRPC]
    public void RPCSetItem(int i)
    {
        item = database.ItemObjects[i];
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
    }
}
