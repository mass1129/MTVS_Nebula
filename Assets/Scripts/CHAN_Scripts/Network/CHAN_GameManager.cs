using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    private void Start()
    {
        //로딩 시작
        //LoadingObject.SetActive(true);
        //처음에는 스카이씬에 바로 들어가도록 함
        InitialLoadScene(PlayerPrefs.GetString("AvatarName"), name_UserScene, userPrefab);
        // 서버접속 시작
        Connect();
    }
    public void Connect()
    {
        // 마스터 서버에 접속한다.
        PN.ConnectUsingSettings();
        PN.NickName = PlayerPrefs.GetString("AvatarName");
    }
    public override void OnConnectedToMaster()
    {
        // 서버에 접속하면 Room으로 바로 입장한다. 
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
        if (sceneName == name_SkyScene)
        { 
            BGMPlayer.instance.GetComponent<AudioSource>().clip = BGMPlayer.instance.audioSources[1];
            BGMPlayer.instance.GetComponent<AudioSource>().Play();
        }
        else if (sceneName == name_UserScene)
        {
            BGMPlayer.instance.GetComponent<AudioSource>().clip = BGMPlayer.instance.audioSources[2];
            BGMPlayer.instance.GetComponent<AudioSource>().Play();
            //LoadingObject.SetActive(false);
        }
        PN.LoadLevel(sceneName);


    }
    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
    }
    RoomOptions roomOption()
    {
        RoomOptions roomOps = new RoomOptions();
        //하늘씬 뷰는 일단 20명 제한
        if (roomName == "sky")
        {
            roomOps.MaxPlayers = 20;
        }
        //유저 방은 10명으로 제한
        else
        {
            roomOps.MaxPlayers = 10;
        }
        return roomOps;
    }
    void SetPlayer(string prefab)
    {
        if (prefab == WhalePrepab)
        { player= PN.Instantiate(prefab, Vector3.zero, Quaternion.identity); }
        else
        { player = PN.Instantiate(prefab, new Vector3(51, 0, 47), Quaternion.identity); }
    }

    public void Go_Sky_Scene()
    {
        Destroy(player);
        roomName = "sky";
        sceneName = name_SkyScene;
        prefab = WhalePrepab;
        print("Join : " + roomName+"Scene");
        //LoadingObject.SetActive(true);
        PN.LeaveRoom();
    }
    public void Go_User_Scene(string NickName)
    {
        Destroy(player);
        roomName = NickName;
        sceneName = name_UserScene;
        prefab = userPrefab;
        print("Join : " + roomName);
        //LoadingObject.SetActive(true);
        PN.LeaveRoom();
    }
    //public void Go_User_Custom()
    //{
    //    Destroy(player);
    //    roomName = customName.text;
    //    sceneName = name_UserScene;
    //    prefab = userPrefab;
    //    print("Join : " + customName.text);
    //    LoadingObject.SetActive(true);
    //    PN.LeaveRoom();
    //}
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        
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
        GameObject Voice = GameObject.Find("VoiceManager");
        Destroy(Voice);
        Destroy(gameObject);
    }
    void InitialLoadScene(string _roomName,string _sceneName,string _prefab)
    {
        roomName = _roomName;
        sceneName = _sceneName;
        prefab = _prefab;
    }
}   
