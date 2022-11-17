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
        roomName = PlayerPrefs.GetString("AvatarName");
    }

    public override void OnConnectedToMaster() => PhotonNetwork.JoinOrCreateRoom("ø‰»Ò", new RoomOptions { MaxPlayers = 6 }, null);

    public string playerPrefabName;
    string roomName;
    public List<GameObject> players;
    public override void OnJoinedRoom()
    {   
       
        
        var player=PhotonNetwork.Instantiate(playerPrefabName, new Vector3(51, 0, 47), Quaternion.identity);
        //players.Add(player);
        //for(int i=0; i<players.Count; i++)
      // players[i].GetComponent<K_PlayerItemSystem>().ItemLoad();

       
    }
}
