using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class JoinUserWorld : MonoBehaviourPunCallbacks
{

    public string RoomName;
    int num=0;
    // 특정 버튼을 누르면 특정 룸에 들어가도록 만들 것임 
    public void Btn_JoinRoom1()
    {
        //로비로 빠져나온다.
        PhotonNetwork.LeaveRoom();
        num = 1;
        //방에 입장한다. 
        //방이 이미 만들어져 있는지 검사한다.
        //있으면 join, 없으면 생성한다.
        //방에 입장할 때 방의 이름은 들어가고자 하는 사람의 닉네임으로 만든다.  
    }
    public void Btn_JoinRoom2()
    {
        //로비로 빠져나온다.
        PhotonNetwork.LeaveRoom();
        num = 2;
        //방에 입장한다. 
        //방이 이미 만들어져 있는지 검사한다.
        //있으면 join, 없으면 생성한다.
        //방에 입장할 때 방의 이름은 들어가고자 하는 사람의 닉네임으로 만든다.  
    }
    public void Btn_JoinRoom3()
    {
        //로비로 빠져나온다.
        PhotonNetwork.LeaveRoom();
        num = 3;
        //방에 입장한다. 
        //방이 이미 만들어져 있는지 검사한다.
        //있으면 join, 없으면 생성한다.
        //방에 입장할 때 방의 이름은 들어가고자 하는 사람의 닉네임으로 만든다.  
    }
    //방 생성 커스텀 정보
    RoomOptions SetRoomOption()
    {
        RoomOptions roomOptions = new RoomOptions();
        //룸의 최대인원 정의
        roomOptions.MaxPlayers = 10;
        // 로비에서 해당 룸에 입장할 때 요구되는 정보들을 정의
        // 포톤의 해쉬테이블을 생성
        //ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
        //hash[roomName] =// 여기에 유저 월드 이름이 들어가야 함
        //roomOptions.CustomRoomProperties = hash;

        return roomOptions;
    }


    public override void OnLeftRoom()
    {
        print("나가짐");
        base.OnLeftRoom();
        
    }
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        print("마스터");
        PhotonNetwork.JoinOrCreateRoom(num.ToString(), SetRoomOption(), TypedLobby.Default);
    }
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        print("방 재생성");
        
    }
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        print($"Room{num} 입장");
        PhotonNetwork.LoadLevel(RoomName);

    }
}
