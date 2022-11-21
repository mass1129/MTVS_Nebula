using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;



public class K_TestGameMgr : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab;
    private void Start()
    {
        if (playerPrefab == null)
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
        }
        else
        {
            //if (PlayerEquipment.LocalPlayerInstance == null)
           // {
                //Debug.LogFormat("We are Instantiating LocalPlayer from {0}", Application.loadedLevelName);
                // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
                PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(51, 0, 47), Quaternion.identity);
           // }
           
        }
        
    }
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }

    public void LeaveRoom()
    {
        
        //PhotonNetwork.Destroy(PlayerEquipment.LocalPlayerInstance);
        PhotonNetwork.LeaveRoom();
    }

    //void LoadArena()
    //{
    //    if (!PhotonNetwork.IsMasterClient)
    //    {
    //        Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
    //        return;
    //    }
    //    Debug.LogFormat("PhotonNetwork : Loading Level : {0}", PhotonNetwork.CurrentRoom.PlayerCount);
    //    PhotonNetwork.LoadLevel(1);
    //}
    //public override void OnPlayerEnteredRoom(Player other)
    //{
    //    Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting


    //    if (PhotonNetwork.IsMasterClient)
    //    {
    //        Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom


    //        LoadArena();
    //    }
    //}


    //public override void OnPlayerLeftRoom(Player other)
    //{
    //    Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects


    //    if (PhotonNetwork.IsMasterClient)
    //    {
    //        Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom


    //        LoadArena();
    //    }
    //}

}