using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChattingSet : MonoBehaviourPunCallbacks
{
    //��ǲ �ʵ��� ���� �����´�.
    public InputField chats;
    public GameObject texts;
    public Transform area_Text;
    //������ �г����� �����´�. 

    void Start()
    {
        //����Ű�� ������ �ٷ� �Է��� �־������� �Ѵ�. 
        if (photonView.IsMine)
        { 
            chats.onSubmit.AddListener(PostText);
        }
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.NickName = "user";
    }

    public void PostText(string s)
    {
        photonView.RPC("RPCPostText", RpcTarget.All, s);
        chats.text = "";
        chats.ActivateInputField();
    }
    [PunRPC]
    void RPCPostText(string text)
    {
        GameObject text_Obj = Instantiate(texts, area_Text);
        text_Obj.SetActive(true);
        text_Obj.GetComponent<Text>().text = text;
        
    }
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        
        PhotonNetwork.JoinOrCreateRoom("00", new RoomOptions(), TypedLobby.Default);
    }
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        print("����");

    }
}
