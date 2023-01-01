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
    // 지금은 시작하자마자 서버에 접속하는 것으로 만듬
    // 추 후 게임 시작 후, 접속하는 부분에서 해당 기능을 넣을 예정
    //해당 매니저는 싱글톤으로 만든다.
    //방을 join, 혹은 create할 때 불러오는 변수  
    [Header("여기에 이동할 씬을 넣으시오")]
    public string name_SkyScene;
    public string name_UserScene;
    public InputField customName;
    
    string roomName="";
    string sceneName="";
    // resources 의 프리팹정보
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
        //로딩 시작
        LoadingObject.SetActive(true);
        //처음에는 스카이씬에 바로 들어가도록 함
        InitialLoadScene("요희", name_UserScene, userPrefab);
        // 서버접속 시작
        Connect();
    }
    public void Connect()
    {
        //유저 아이디를 커스텀으로 설정
        AuthenticationValues authValues = new AuthenticationValues(avatarName);
        PN.AuthValues = authValues;
        PN.NickName = avatarName;
        // 마스터 서버에 접속한다.
        PN.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster()
    {
        // 서버에 접속하면 Room으로 바로 입장한다. 
        base.OnConnectedToMaster();
        //유저 정보 조회는 마스터 서버시점부터 시작된다.
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
        //유저 방은 10명으로 제한
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
        print("플레이어 퇴장");
    }
    // 씬에서 유저 프로필 씬으로 이동하는 함수
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
