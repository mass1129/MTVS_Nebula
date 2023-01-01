using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using PN=Photon.Pun.PhotonNetwork;
using User_Info;
using CUI=User_Info.CHAN_Users_Info;

public class CHAN_GameManager : MonoBehaviourPunCallbacks
{
    public static CHAN_GameManager instance;
    
    private void Awake()
    {
        PN.SendRate = 30;
        PN.SerializationRate = 15;
        PhotonNetwork.AutomaticallySyncScene = true;
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

    public string avatarName;
    public string ownIslandId;
    private void Start()
    {
        //�ε� ����
        LoadingObject.SetActive(true);
        //ó������ ��ī�̾��� �ٷ� ������ ��
        InitialLoadScene("����", name_UserScene, userPrefab);
        // �������� ����
        Connect();
    }
    public void Connect()
    {
        //���� ���̵� Ŀ�������� ����
        AuthenticationValues authValues = new AuthenticationValues(avatarName);
        PN.AuthValues = authValues;
        PN.NickName = avatarName;
        // ������ ������ �����Ѵ�.
        PN.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster()
    {
        // ������ �����ϸ� Room���� �ٷ� �����Ѵ�. 
        base.OnConnectedToMaster();
        //���� ���� ��ȸ�� ������ ������������ ���۵ȴ�.
        PN.FindFriends(CUI.userLists.ToArray());
        PN.JoinOrCreateRoom(roomName, roomOption(), TypedLobby.Default);

    }
    public override void OnFriendListUpdate(List<FriendInfo> friendList)
    {
        if (CUI.onlineUsers.Count > 0) CUI.onlineUsers.Clear();
        foreach (FriendInfo info in friendList)
        {
            if (info.IsOnline)
            {
                CUI.onlineUsers[info.UserId] = info.Room;
            }
        }

    }
    public override void OnJoinedRoom()
    {
       
        if (sceneName == name_SkyScene)
        { 
            BGMPlayer.instance.GetComponent<AudioSource>().clip = BGMPlayer.instance.audioSources[1];
            BGMPlayer.instance.GetComponent<AudioSource>().Play();
            PN.LoadLevel(sceneName);
            SetPlayer(prefab);
        }
        else if (sceneName == name_UserScene)
        {
            BGMPlayer.instance.GetComponent<AudioSource>().clip = BGMPlayer.instance.audioSources[2];
            BGMPlayer.instance.GetComponent<AudioSource>().Play();
            if(PN.CurrentRoom.PlayerCount <= 1)
            PN.LoadLevel(sceneName);
            LoadingObject.SetActive(false);
            
        }
        
        
        

    }


    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
       
        
    }
    RoomOptions roomOption()
    {
        RoomOptions roomOps = new();
        if (roomName == "sky")
        {
            roomOps.MaxPlayers = 20;
        }
        //���� ���� 10������ ����
        else
        {
            roomOps.MaxPlayers = 20;
            
        }
        return roomOps;
    }
    public void SetPlayer(string prefab)
    {
        player = PN.Instantiate(prefab, new Vector3(0, 3, 0), Quaternion.identity);

    }

    public void Go_Sky_Scene()
    {
        roomName = "sky";
        sceneName = name_SkyScene;
        prefab = WhalePrepab;
        print("Join : " + roomName + "Scene");
        LoadingObject.SetActive(true);
        PN.LeaveRoom();
        print("Leave : " + roomName);
        //ExitButton_GoSky();
    }
    public void Go_User_Scene(string NickName)
    {
        PhotonNetwork.Destroy(player);
        roomName = NickName;
        sceneName = name_UserScene;
        prefab = userPrefab;
        print("Join : " + roomName);
        LoadingObject.SetActive(true);
        PN.LeaveRoom();
        print("Leave : " + roomName);
        //ExitButton_GoWorld(NickName);
    }
    public void ExitButton_GoSky()
    {
        if (PhotonNetwork.InRoom)
        {
           
            roomName = "sky";
            sceneName = name_SkyScene;
            prefab = WhalePrepab;
            LoadingObject.SetActive(true);
            print("Leave : " + roomName);
            if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount > 1) MigrateMaster();
            else
            {
                PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.LocalPlayer);
                PhotonNetwork.LeaveRoom();
            }
        }
    }
    public void ExitButton_GoWorld(string NickName)
    {
        if (PhotonNetwork.InRoom)
        {
            
            roomName = NickName;
            sceneName = name_UserScene;
            prefab = userPrefab;
            print("Join : " + roomName);
            LoadingObject.SetActive(true);

            if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount > 1) MigrateMaster();
            else
            {
                PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.LocalPlayer);
                PhotonNetwork.LeaveRoom();
            }
        }
    }
    private void MigrateMaster()
    {
        var dict = PhotonNetwork.CurrentRoom.Players;
        if (PhotonNetwork.SetMasterClient(dict[dict.Count - 1]))
            PhotonNetwork.LeaveRoom();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {

        
    }
    
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        print("�÷��̾� ����");
    }
    // ������ ���� ������ ������ �̵��ϴ� �Լ�
    public void Btn_GoProfile()
    {
        PN.Disconnect();
        SceneManager.LoadScene(1);
        BGMPlayer.instance.GetComponent<AudioSource>().clip = BGMPlayer.instance.audioSources[0];
        BGMPlayer.instance.GetComponent<AudioSource>().Play();
        GameObject Voice = GameObject.Find("VoiceManager");
        Destroy(Voice);
        Destroy(gameObject);
    }
    void InitialLoadScene(string _roomName,string _sceneName,string _prefab)
    {
        roomName = _roomName;
        sceneName = _sceneName;
        prefab = _prefab;
        avatarName = PlayerPrefs.GetString("AvatarName");
        ownIslandId = PlayerPrefs.GetString("Island_ID");
    }
}   
