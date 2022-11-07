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
    public string nick;
    //���� join, Ȥ�� create�� �� �ҷ����� ����  
    [Header("���⿡ �̵��� ���� �����ÿ�")]
    public string name_SkyScene;
    public string name_UserScene;
    public InputField customName;
    
    string roomName="";
    string sceneName="";

    public bool IsSetWhale;
    // resources �� ����������
    public string userPrefab;
    public string WhalePrepab;
    private void Start()
    {
        //ó������ ��ī�̾��� �ٷ� ������ ��
        roomName = "sky";
        sceneName = name_SkyScene;
        Connect();
    }
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
        if (IsSetWhale)
        {
            SetPlayer(WhalePrepab);
        }
        else
        {
            SetPlayer(userPrefab);
        }
        
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
        GameObject playerObj = PN.Instantiate(prefab, Vector3.zero, Quaternion.identity);
    }

    public void Go_Sky_Scene()
    {
        roomName = "sky";
        sceneName = name_SkyScene;
        //if (photonView.IsMine)
        //{ 
        //    RemoveMyPhotonView(); 
        //}
        PN.LeaveRoom();
    }
    public void Go_User_Scene(string NickName)
    {

        roomName = NickName;
        sceneName = name_UserScene;
        //if (photonView.IsMine)
        //{
        //    RemoveMyPhotonView();
        //}
        PN.LeaveRoom();
    }
    public void Go_User_Custom()
    {
        roomName = customName.text;
        sceneName = name_UserScene;
        print("Set :" + customName.text);
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
    //void RemoveMyPhotonView()
    //{
    //    photonView.RPC("RPCRemoveMyPhotonView", RpcTarget.All);
    //}
    //[PunRPC]
    //void RPCRemoveMyPhotonView()
    //{
    //    Minimap_IconManager.instance.Remove_User_Icon();
    //    print(CHAN_PlayerManger.LocalPlayerInstance.GetComponent<PhotonView>());
    //}

}   
