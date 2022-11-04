using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PN=Photon.Pun.PhotonNetwork;

public class CHAN_GameManager : MonoBehaviourPunCallbacks
{
    public static CHAN_GameManager instance;

    private void Awake()
    {
        PN.SendRate = 30;
        PN.SerializationRate = 15;
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }
    // ������ �������ڸ��� ������ �����ϴ� ������ ����
    // �� �� ���� ���� ��, �����ϴ� �κп��� �ش� ����� ���� ����
    private void Start()
    {
        Connect();
    }
    //�ش� �Ŵ����� �̱������� �����.
    public string nick;
    //���� join, Ȥ�� create�� �� �ҷ����� ����  
    string roomName="sky";
    string sceneName= "3.SkyView_Scene";
    [Header("���⿡ �̵��� ���� �����ÿ�")]
    public string name_SkyScene;
    public string name_UserScene;
    public void Connect()
    {
        // ������ ������ �����Ѵ�.
        PN.ConnectUsingSettings();
        PN.NickName = nick;
    }
    public override void OnConnectedToMaster()
    {
        // ������ �����ϸ� Room���� �ٷ� �����Ѵ�. 
        base.OnConnectedToMaster();
        PN.JoinOrCreateRoom(roomName,roomOption(), TypedLobby.Default);
        
    }
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
    }
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        PN.LoadLevel(sceneName);
        SetPlayer();
    }
    RoomOptions roomOption()
    {
        RoomOptions roomOps = new RoomOptions();
        //�ϴþ� ��� �ϴ� 20�� ����
        if (roomName == "sky")
        {
            roomOps.MaxPlayers = 20;
        }
        //���� ���� 10������ ����
        else
        {
            roomOps.MaxPlayers = 10;
        }
        return roomOps;
    }
    void SetPlayer()
    {
        GameObject playerObj = PN.Instantiate("Player_Whale", Vector3.zero, Quaternion.identity);
        print("�� �ҷ���");
    }

    public void Go_Sky_Scene()
    {
        roomName = "sky";
        sceneName = name_SkyScene;
        PN.LeaveRoom();
    }
    public void Go_User_Scene(string NickName)
    {

        roomName = NickName;
        sceneName = name_UserScene;
        PN.LeaveRoom();
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        

    }
}   
