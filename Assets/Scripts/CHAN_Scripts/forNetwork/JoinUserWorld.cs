using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class JoinUserWorld : MonoBehaviourPunCallbacks
{

    public string RoomName;
    int num=0;
    // Ư�� ��ư�� ������ Ư�� �뿡 ������ ���� ���� 
    public void Btn_JoinRoom1()
    {
        //�κ�� �������´�.
        PhotonNetwork.LeaveRoom();
        num = 1;
        //�濡 �����Ѵ�. 
        //���� �̹� ������� �ִ��� �˻��Ѵ�.
        //������ join, ������ �����Ѵ�.
        //�濡 ������ �� ���� �̸��� ������ �ϴ� ����� �г������� �����.  
    }
    public void Btn_JoinRoom2()
    {
        //�κ�� �������´�.
        PhotonNetwork.LeaveRoom();
        num = 2;
        //�濡 �����Ѵ�. 
        //���� �̹� ������� �ִ��� �˻��Ѵ�.
        //������ join, ������ �����Ѵ�.
        //�濡 ������ �� ���� �̸��� ������ �ϴ� ����� �г������� �����.  
    }
    public void Btn_JoinRoom3()
    {
        //�κ�� �������´�.
        PhotonNetwork.LeaveRoom();
        num = 3;
        //�濡 �����Ѵ�. 
        //���� �̹� ������� �ִ��� �˻��Ѵ�.
        //������ join, ������ �����Ѵ�.
        //�濡 ������ �� ���� �̸��� ������ �ϴ� ����� �г������� �����.  
    }
    //�� ���� Ŀ���� ����
    RoomOptions SetRoomOption()
    {
        RoomOptions roomOptions = new RoomOptions();
        //���� �ִ��ο� ����
        roomOptions.MaxPlayers = 10;
        // �κ񿡼� �ش� �뿡 ������ �� �䱸�Ǵ� �������� ����
        // ������ �ؽ����̺��� ����
        //ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
        //hash[roomName] =// ���⿡ ���� ���� �̸��� ���� ��
        //roomOptions.CustomRoomProperties = hash;

        return roomOptions;
    }


    public override void OnLeftRoom()
    {
        print("������");
        base.OnLeftRoom();
        
    }
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        print("������");
        PhotonNetwork.JoinOrCreateRoom(num.ToString(), SetRoomOption(), TypedLobby.Default);
    }
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        print("�� �����");
        
    }
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        print($"Room{num} ����");
        PhotonNetwork.LoadLevel(RoomName);

    }
}
