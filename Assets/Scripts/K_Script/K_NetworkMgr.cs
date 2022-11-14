using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class K_NetworkMgr : MonoBehaviourPunCallbacks
{
    public GridBuildingSystem3D builder;
    private void Awake()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster() => PhotonNetwork.JoinOrCreateRoom("ø‰»Ò", new RoomOptions { MaxPlayers = 6 }, null);

    public override void OnJoinedRoom()
    {   
        if(builder!=null)
        {
         
            PhotonNetwork.Destroy(builder.gameObject);
        }
       
        PhotonNetwork.Instantiate("Player1 1", new Vector3(51, 0, 47), Quaternion.identity);
    }
}
