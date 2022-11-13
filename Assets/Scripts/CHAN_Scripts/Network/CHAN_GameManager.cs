using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    //�ش� �Ŵ����� �̱������� �����.
    //���� join, Ȥ�� create�� �� �ҷ����� ����  
    [Header("���⿡ �̵��� ���� �����ÿ�")]
    public string name_SkyScene;
    public string name_UserScene;
    public InputField customName;
    
    string roomName="";
    string sceneName="";
    // resources �� ����������
    [HideInInspector]
    public string prefab;
    public string userPrefab;
    public string WhalePrepab;

    GameObject player;
    public GameObject LoadingObject;
    //public GameObject Chat;
    private void Start()
    {
        //�ε� ����
        LoadingObject.SetActive(true);
        //ó������ ��ī�̾��� �ٷ� ������ ��
        roomName = "sky";
        sceneName = name_SkyScene;
        prefab = WhalePrepab;
        Connect();
    }
    public void Connect()
    {
        // ������ ������ �����Ѵ�.
        PN.ConnectUsingSettings();
        PN.NickName = PlayerPrefs.GetString("AvatarName");
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
        SetPlayer(prefab);
        PN.LoadLevel(sceneName);
        //if (sceneName == name_UserScene)
        //{
        //    PN.Instantiate("Chatting",Vector3.zero,Quaternion.identity);
        //}
    }
    public override void OnLeftRoom()
    {
        base.OnLeftRoom();

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
    void SetPlayer(string prefab)
    {
        player= PN.Instantiate(prefab, new Vector3(51, 0, 47), Quaternion.identity);
    }

    public void Go_Sky_Scene()
    {
        Destroy(player);
        roomName = "sky";
        sceneName = name_SkyScene;
        prefab = WhalePrepab;
        print("Join : " + roomName+"Scene");
        PN.LeaveRoom();
    }
    public void Go_User_Scene(string NickName)
    {
        Destroy(player);
        roomName = NickName;
        sceneName = name_UserScene;
        prefab = userPrefab;
        print("Join : " + roomName);
        PN.LeaveRoom();
    }
    public void Go_User_Custom()
    {
        Destroy(player);
        roomName = customName.text;
        sceneName = name_UserScene;
        prefab = userPrefab;
        print("Join : " + customName.text);
        PN.LeaveRoom();
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        print("�÷��̾� ����");
    }

}   
