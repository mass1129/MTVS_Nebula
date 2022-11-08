using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChattingSet : MonoBehaviourPun
{
    public static ChattingSet instance;
    //인풋 필드의 값을 가져온다.
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
        //엔터키를 누르면 바로 입력이 넣어지도록 한다. 
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
    // 여기서 글자가 입력된다. 
    public void PostText(string s)
    {
        //이것은 옵션사항
        //<color=#FFFFFF>닉네임</color>
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
    #region 채팅관련 코드 모음
    // 이전 Content 의 H
    float previousContentH;
    // ScrollView 의 RectTransform
    public RectTransform trScrollView;
    [PunRPC]
    void RPCPostText(string chatText)
    {
        //0.바뀌기 전의 Content H 값 넣는다. 
        previousContentH = trContent.sizeDelta.y;

        GameObject text_Obj = Instantiate(chatItem, trContent);
        ChatItem chat=text_Obj.GetComponent<ChatItem>();
        chat.SetText(chatText);
        StartCoroutine(AutoScrollBottom());
    }
    IEnumerator AutoScrollBottom()
    {
        yield return null;
        //trScroll H보다 Content H값이 커지면(스크롤 가능 상태)
        if (trContent.sizeDelta.y > trScrollView.sizeDelta.y)
        { 
            //만약 Content가 바닥에 닿아있다면
            if (trContent.anchoredPosition.y >= previousContentH - trScrollView.sizeDelta.y)
            {
                //Content y 값 다시 설정
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
    //    print("입장");

    //}
}
