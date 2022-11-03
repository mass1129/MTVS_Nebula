using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class JoinUserWorld : MonoBehaviourPunCallbacks
{
    //�ش� ��ũ��Ʈ�� �̱������� �����.
    public static JoinUserWorld instance;
<<<<<<< Updated upstream
    
    
=======
    public string UserWorld;

>>>>>>> Stashed changes
    private void Awake()
    {
        instance = this;
    }
    // Ư�� ��ư�� ������ Ư�� �뿡 ������ ���� ���� 
<<<<<<< Updated upstream
    public void Btn_JoinRoom()
    {
        //�κ�� �������´�.
        PhotonNetwork.LeaveRoom();
        //�濡 �����Ѵ�. 
        //���� �̹� ������� �ִ��� �˻��Ѵ�.
        //������ join, ������ �����Ѵ�.
        //�濡 ������ �� ���� �̸��� ������ �ϴ� ����� �г������� �����. 
        PhotonNetwork.JoinOrCreateRoom("1", SetRoomOption("1"), TypedLobby.Default);
    }
    RoomOptions SetRoomOption(string roomName)
    {
        RoomOptions roomOptions = new RoomOptions();
        //���� �ִ��ο� ����
        roomOptions.MaxPlayers = 10;
        // �κ񿡼� �ش� �뿡 ������ �� �䱸�Ǵ� �������� ����
        string[] LobbyOptions = { "map" };
        // ������ �ؽ����̺��� ����
        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
        hash[roomName] =// ���⿡ ���� ���� �̸��� ���� ��
        roomOptions.CustomRoomProperties = hash;
        roomOptions.CustomRoomPropertiesForLobby = LobbyOptions;

        return roomOptions;

    }
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        PhotonNetwork.LoadLevel(UserWorld);
    }
=======
    public void JoinFriendRoom()
    {
        print("����");
        PhotonNetwork.LeaveRoom();
    }
    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        print("������");
        PhotonNetwork.JoinLobby();
        
        
        
    }
    //public override void OnCreatedRoom()
    //{
    //    base.OnCreatedRoom();
    //}
    RoomOptions SetRoomOption(string name)
    {
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 10;
        //string[] properties = { "name" };
        //options.CustomRoomPropertiesForLobby = properties;
        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
        hash["name"] = name;
        options.CustomRoomProperties = hash;
        return options;
    }
    public override void OnConnectedToMaster()
    {
        print("������ ������");
        base.OnConnectedToMaster();
        PhotonNetwork.JoinOrCreateRoom("1", SetRoomOption("1"), TypedLobby.Default);
    }
    public override void OnJoinedRoom()
    {
        print("�� ������");
        base.OnJoinedRoom();
        PhotonNetwork.LoadLevel(UserWorld);
    }
    public override void OnJoinedLobby()
    {
        //�� ����
        print("�κ� ������");
        base.OnJoinedLobby();
       
    }
>>>>>>> Stashed changes
}
