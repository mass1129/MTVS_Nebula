using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class JoinUserWorld : MonoBehaviourPunCallbacks
{
    //해당 스크립트는 싱글톤으로 만든다.
    public static JoinUserWorld instance;
<<<<<<< Updated upstream
    
    
=======
    public string UserWorld;

>>>>>>> Stashed changes
    private void Awake()
    {
        instance = this;
    }
    // 특정 버튼을 누르면 특정 룸에 들어가도록 만들 것임 
<<<<<<< Updated upstream
    public void Btn_JoinRoom()
    {
        //로비로 빠져나온다.
        PhotonNetwork.LeaveRoom();
        //방에 입장한다. 
        //방이 이미 만들어져 있는지 검사한다.
        //있으면 join, 없으면 생성한다.
        //방에 입장할 때 방의 이름은 들어가고자 하는 사람의 닉네임으로 만든다. 
        PhotonNetwork.JoinOrCreateRoom("1", SetRoomOption("1"), TypedLobby.Default);
    }
    RoomOptions SetRoomOption(string roomName)
    {
        RoomOptions roomOptions = new RoomOptions();
        //룸의 최대인원 정의
        roomOptions.MaxPlayers = 10;
        // 로비에서 해당 룸에 입장할 때 요구되는 정보들을 정의
        string[] LobbyOptions = { "map" };
        // 포톤의 해쉬테이블을 생성
        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
        hash[roomName] =// 여기에 유저 월드 이름이 들어가야 함
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
        print("나감");
        PhotonNetwork.LeaveRoom();
    }
    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        print("나갔음");
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
        print("마스터 재입장");
        base.OnConnectedToMaster();
        PhotonNetwork.JoinOrCreateRoom("1", SetRoomOption("1"), TypedLobby.Default);
    }
    public override void OnJoinedRoom()
    {
        print("룸 재입장");
        base.OnJoinedRoom();
        PhotonNetwork.LoadLevel(UserWorld);
    }
    public override void OnJoinedLobby()
    {
        //룸 입장
        print("로비 재입장");
        base.OnJoinedLobby();
       
    }
>>>>>>> Stashed changes
}
