using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using PN = Photon.Pun.PhotonNetwork;
using Agora_RTC_Plugin.API_Example.Examples.Advanced.ScreenShare;

public class Item_TVManager_Agora : MonoBehaviourPun
{
    public static Item_TVManager_Agora instance;
    public Transform prefab_Wall;
    //public Transform window;
    public GameObject[] prefab_TVScreem;

    ScreenShare ss;
    Vector3 InitialPos;
    public bool moving;
    public bool hasControl;


    private void Awake()
    {
        instance = this;
    }
    public Text introduceText;
    //TV가 켜졌는지 꺼졌는지 판별
    public bool isTurn;
    void Start()
    {
        InitialPos = prefab_Wall.position;
        ss = GetComponent<ScreenShare>();
    }

    #region 화면공유 준비
    public void ReadyToShare()
    {
        photonView.RPC("RPCReadyToShare", RpcTarget.All);
        Debug.LogWarning("공유준비 완료");
    }
    [PunRPC]
    void RPCReadyToShare()
    {
        ss.Initialize();
    }
    #endregion
    #region 화면 송출 종료
    public void EndShare()
    {
        photonView.RPC("RPCEndShare", RpcTarget.All);

    }
    [PunRPC]
    void RPCEndShare()
    {
        ss.EndShare();
        Debug.LogWarning("화면공유 비활성화 시작");
    }
    #endregion
    #region 벽을 움직이게 하는 함수 모음
    public void Move(bool isTurn)
    {
        photonView.RPC("RpcMove", RpcTarget.All, isTurn);
    }
    [PunRPC]
    void RpcMove(bool isTurn)
    { StartCoroutine(MoveTV(isTurn)); }
    IEnumerator MoveTV(bool b)
    {
        moving = true;
        // TV가 땅에서 나온다.
        // 초기에 스크린은 땅에 위치하도록(y= -5에 위치)
        //InsertScreenObject(false);
        Vector3 SetPos = b == true ? InitialPos + Vector3.up * 10 : InitialPos;
        float distance = Vector3.Distance(prefab_Wall.position, SetPos);
        while (distance > 0.1f)
        {
            prefab_Wall.position = Vector3.Lerp(prefab_Wall.position, SetPos, Time.deltaTime * 2);
            distance = Vector3.Distance(prefab_Wall.position, SetPos);
            //window.position = prefab_Wall.position;
            yield return null;
        }
        prefab_Wall.position = SetPos;
        //window.position -= window.transform.forward * 0.51f;
        //버튼을 누르면 y=10 만큼 좌표 lerp하게 이동 
        moving = false;

    }
    #endregion
    #region 유저들에게 화면 송출 시작 
    public void SendScreen(string id)
    {
        photonView.RPC("RPCSendScreen", RpcTarget.All, id);
        ss.TurnOnMyScreen();
        Debug.Log(id);
    }
    [PunRPC]
    void RPCSendScreen(string id)
    {
        ss.TurnOnScreen(id);
        Debug.LogWarning(id + " 보내짐");
    }
    #endregion
    #region 플레이어에게 TV 제어권한 주기 위한 함수
    public void TurnControl()
    { photonView.RPC("RPCTurnControl", RpcTarget.All); }
    [PunRPC]
    void RPCTurnControl()
    {
        hasControl = !hasControl;
    }
    #endregion
}

