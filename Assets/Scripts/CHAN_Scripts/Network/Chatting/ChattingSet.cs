using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChattingSet : MonoBehaviourPun
{
    public static ChattingSet instance;
    //��ǲ �ʵ��� ���� �����´�.
    public InputField chats;
    public GameObject chatItem;
    public RectTransform trContent;

    public Button btn_Enter;
    public Button btn_HideChatMenu;
    public GameObject Area_Scroll;
     
    Color32 idColor;

    private void Awake()
    {
        //if (instance == null)
        //{
        //    instance = this;
        //    DontDestroyOnLoad(this);
        //}
        //else
        //{
        //    Destroy(this);
        //}

    }
    void Start()
    {
        btn_HideChatMenu.onClick.AddListener(HideChat);
        btn_Enter.onClick.AddListener(OnclickedEnter);
        //����Ű�� ������ �ٷ� �Է��� �־������� �Ѵ�. 
        if (photonView.IsMine)
        {
            chats.onSubmit.AddListener(PostText);
        }
        //PhotonNetwork.ConnectUsingSettings();
        //PhotonNetwork.NickName = "user"+Random.RandomRange(0,100).ToString();
        idColor = new Color32(
            (byte)Random.Range(0, 256),
            (byte)Random.Range(0, 256),
            (byte)Random.Range(0, 256),
            255
            );
    }

    public void HideChat()
    {
        Area_Scroll.SetActive(!Area_Scroll.activeSelf);
    }
    // ���⼭ ���ڰ� �Էµȴ�. 
    public void PostText(string s)
    {
        //�̰��� �ɼǻ���
        //<color=#FFFFFF>�г���</color>
        string chatText = "<color=#"+ColorUtility.ToHtmlStringRGB(idColor)+ ">" +PhotonNetwork.NickName +"</color>" +": " + s;
        //string chatText = PhotonNetwork.NickName + ": " + s;
        photonView.RPC("RPCPostText", RpcTarget.All, chatText);
        chats.text = "";
        chats.ActivateInputField();
    }
    public void OnclickedEnter()
    {
        PostText(chats.text);
    }
    #region ä�ð��� �ڵ� ����
    // ���� Content �� H
    float previousContentH;
    // ScrollView �� RectTransform
    public RectTransform trScrollView;
    [PunRPC]
    void RPCPostText(string chatText)
    {
        //0.�ٲ�� ���� Content H �� �ִ´�. 
        previousContentH = trContent.sizeDelta.y;

        GameObject text_Obj = Instantiate(chatItem, trContent);
        ChatItem chat=text_Obj.GetComponent<ChatItem>();
        chat.SetText(chatText);
        StartCoroutine(AutoScrollBottom());
    }
    IEnumerator AutoScrollBottom()
    {
        yield return null;
        //trScroll H���� Content H���� Ŀ����(��ũ�� ���� ����)
        if (trContent.sizeDelta.y > trScrollView.sizeDelta.y)
        { 
            //���� Content�� �ٴڿ� ����ִٸ�
            if (trContent.anchoredPosition.y >= previousContentH - trScrollView.sizeDelta.y)
            {
                //Content y �� �ٽ� ����
                trContent.anchoredPosition = new Vector2(0, trContent.sizeDelta.y - trScrollView.sizeDelta.y);
            }
        
        }
    }
    #endregion

    //public override void OnConnectedToMaster()
    //{
    //    base.OnConnectedToMaster();
        
    //    PhotonNetwork.JoinOrCreateRoom("00", new RoomOptions(), TypedLobby.Default);
    //}
    //public override void OnJoinedRoom()
    //{
    //    base.OnJoinedRoom();
    //    print("����");

    //}
}
