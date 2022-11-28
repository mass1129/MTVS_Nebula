using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.Threading.Tasks;

public class FastJoinManager : MonoBehaviourPunCallbacks
{
    Button goMyWorld;
    [Header("이동하고자 하는 씬 이름 입력 (해당 씬이 빌드 세팅에 있어야 함)")]
    public string Room;
    private void Awake()
    {
        // 이 오브젝트는 사라지면 안된다. 
        DontDestroyOnLoad(this);

    }
    private void Start()
    {
        testKey();
    }
    [ContextMenu("go")]
    public void testKey()
    { 
        OnClickConnect();
        
    }
    #region 마스터서버 -> 로비 입장 함수
    //1. 마스터 서버 접속 시도
    public void OnClickConnect()
    {
        // setting inspector 에서 해도 됨 
        PhotonNetwork.GameVersion = "1";
        //NameServer 접속,(AppID, GameVersion, 지역)
        PhotonNetwork.ConnectUsingSettings();
        print("접속 시작");

    }
    //2. 마스터 서버 접속 성공
    public override void OnConnected()
    {
        base.OnConnected();
        print("접속 성공");
    }
    //3. 마스터 서버에 접속 성공

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        print("마스터 서버 접속 성공");
        print("로비 입장");
        PhotonNetwork.JoinLobby();

    }
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        print("로비 접속 성공");
        //PhotonNetwork.LoadLevel("");
        CreateRoom();


    }
    #endregion
    //만약 "내 하늘섬으로 돌아가기 버튼을 누르면 룸으로 입장하도록 만든다. 
    string roomName = "1";
    public void CreateRoom()
    {
        // 갈축
        //방 정보 셋팅
        RoomOptions roomOptions = new RoomOptions();
        //최대인원
        //where '0' means "no limit"
        roomOptions.MaxPlayers = 4;//byte.Parse(totalNum.text);
        // 방을 만든다.
        PhotonNetwork.CreateRoom(roomName, roomOptions, TypedLobby.Default);
    }
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        print("방 생성 완료");
        //GetBtn();

    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        print("방 생성 실패");
    }
    public override void OnJoinedRoom()
    {
        print("방 입장 완료");
        base.OnJoinedRoom();
        //PhotonNetwork.LoadLevel(Room);
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        print("방 입장 실패");
        base.OnJoinRoomFailed(returnCode, message);
    }
    //public async void GetBtn()
    //{
    //    while (!GameObject.Find("Btn_MyWorld"))
    //    {
    //        await Task.Yield();
    //    }
    //    goMyWorld = GameObject.Find("Btn_MyWorld").GetComponent<Button>();
    //    goMyWorld.onClick.AddListener(CreateRoom);

    //}

}
