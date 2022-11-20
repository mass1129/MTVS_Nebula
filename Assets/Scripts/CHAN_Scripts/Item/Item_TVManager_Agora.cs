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
    //TV�� �������� �������� �Ǻ�
    public bool isTurn;
    void Start()
    {
        InitialPos = prefab_Wall.position;
        ss = GetComponent<ScreenShare>();
    }

    #region ȭ����� �غ�
    public void ReadyToShare()
    {
        photonView.RPC("RPCReadyToShare", RpcTarget.All);
        Debug.LogWarning("�����غ� �Ϸ�");
    }
    [PunRPC]
    void RPCReadyToShare()
    {
        ss.Initialize();
    }
    #endregion
    #region ȭ�� ���� ����
    public void EndShare()
    {
        photonView.RPC("RPCEndShare", RpcTarget.All);

    }
    [PunRPC]
    void RPCEndShare()
    {
        ss.EndShare();
        Debug.LogWarning("ȭ����� ��Ȱ��ȭ ����");
    }
    #endregion
    #region ���� �����̰� �ϴ� �Լ� ����
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
        // TV�� ������ ���´�.
        // �ʱ⿡ ��ũ���� ���� ��ġ�ϵ���(y= -5�� ��ġ)
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
        //��ư�� ������ y=10 ��ŭ ��ǥ lerp�ϰ� �̵� 
        moving = false;

    }
    #endregion
    #region �����鿡�� ȭ�� ���� ���� 
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
        Debug.LogWarning(id + " ������");
    }
    #endregion
    #region �÷��̾�� TV ������� �ֱ� ���� �Լ�
    public void TurnControl()
    { photonView.RPC("RPCTurnControl", RpcTarget.All); }
    [PunRPC]
    void RPCTurnControl()
    {
        hasControl = !hasControl;
    }
    #endregion
}

