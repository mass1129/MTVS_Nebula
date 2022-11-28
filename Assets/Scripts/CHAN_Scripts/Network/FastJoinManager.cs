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
    [Header("�̵��ϰ��� �ϴ� �� �̸� �Է� (�ش� ���� ���� ���ÿ� �־�� ��)")]
    public string Room;
    private void Awake()
    {
        // �� ������Ʈ�� ������� �ȵȴ�. 
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
    #region �����ͼ��� -> �κ� ���� �Լ�
    //1. ������ ���� ���� �õ�
    public void OnClickConnect()
    {
        // setting inspector ���� �ص� �� 
        PhotonNetwork.GameVersion = "1";
        //NameServer ����,(AppID, GameVersion, ����)
        PhotonNetwork.ConnectUsingSettings();
        print("���� ����");

    }
    //2. ������ ���� ���� ����
    public override void OnConnected()
    {
        base.OnConnected();
        print("���� ����");
    }
    //3. ������ ������ ���� ����

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        print("������ ���� ���� ����");
        print("�κ� ����");
        PhotonNetwork.JoinLobby();

    }
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        print("�κ� ���� ����");
        //PhotonNetwork.LoadLevel("");
        CreateRoom();


    }
    #endregion
    //���� "�� �ϴü����� ���ư��� ��ư�� ������ ������ �����ϵ��� �����. 
    string roomName = "1";
    public void CreateRoom()
    {
        // ����
        //�� ���� ����
        RoomOptions roomOptions = new RoomOptions();
        //�ִ��ο�
        //where '0' means "no limit"
        roomOptions.MaxPlayers = 4;//byte.Parse(totalNum.text);
        // ���� �����.
        PhotonNetwork.CreateRoom(roomName, roomOptions, TypedLobby.Default);
    }
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        print("�� ���� �Ϸ�");
        //GetBtn();

    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        print("�� ���� ����");
    }
    public override void OnJoinedRoom()
    {
        print("�� ���� �Ϸ�");
        base.OnJoinedRoom();
        //PhotonNetwork.LoadLevel(Room);
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        print("�� ���� ����");
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
