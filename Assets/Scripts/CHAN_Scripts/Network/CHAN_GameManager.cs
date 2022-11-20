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
    private void Start()
    {
        //�ε� ����
        //LoadingObject.SetActive(true);
        //ó������ ��ī�̾��� �ٷ� ������ ��
        InitialLoadScene(PlayerPrefs.GetString("AvatarName"), name_UserScene, userPrefab);
        // �������� ����
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
        print("�÷��̾� ����");
    }
    // ������ ���� ������ ������ �̵��ϴ� �Լ�
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
