using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChattingSet : MonoBehaviourPunCallbacks
{
    //인풋 필드의 값을 가져온다.
    public InputField chats;
    public GameObject texts;
    public Transform area_Text;
    //유저의 닉네임을 가져온다. 

    void Start()
    {
        //엔터키를 누르면 바로 입력이 넣어지도록 한다. 
        if (photonView.IsMine)
        { 
            chats.onSubmit.AddListener(PostText);
        }
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.NickName = "user"+Random.RandomRange(0,100).ToString();
    }

    // 여기서 글자가 입력된다. 
    public void PostText(string s)
    {
        photonView.RPC("RPCPostText", RpcTarget.All, PhotonNetwork.NickName, s);
        chats.text = "";
        chats.ActivateInputField();
    }
    [PunRPC]
    void RPCPostText(string nickName, string text)
    {
        GameObject text_Obj = Instantiate(texts, area_Text);
        text_Obj.SetActive(true);
        text_Obj.GetComponent<Text>().text = nickName+": "+text;
        
    }
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        
        PhotonNetwork.JoinOrCreateRoom("00", new RoomOptions(), TypedLobby.Default);
    }
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        print("입장");

    }
}
