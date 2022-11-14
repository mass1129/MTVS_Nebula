using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChattingSet : MonoBehaviourPun
{
    
    //인풋 필드의 값을 가져온다.
    [Header("입력부")]
    public InputField chats;
 
    public GameObject chatItem;
    [Header("채팅영역 관련 설정들")]
    // 이전 Content 의 H
    float previousContentH;
    public RectTransform trContent;
    // ScrollView 의 RectTransform
    public RectTransform trScrollView;
    public GameObject Area_Scroll;
    public GameObject Area_Chat;
    [Header("버튼 관련 설정들")]
    public Button btn_Enter;
    public Button btn_HideChatMenu;
    [Header("기타")]
    Color32 idColor;


    void Start()
    {
        //각 버튼에 사용할 함수들을 할당한다.
        btn_HideChatMenu.onClick.AddListener(HideChat);
        btn_Enter.onClick.AddListener(OnclickedEnter);
        //엔터키를 눌렀을 때 함수가 발동되도록 설정한다.
        chats.onSubmit.AddListener(PostText);
        // 유저 닉네임 색을 설정하기위한 색깔정의
        idColor = new Color32(
            (byte)Random.Range(0, 256),
            (byte)Random.Range(0, 256),
            (byte)Random.Range(0, 256),
            255
            );
    }
    #region 버튼 설정 
    // 채팅장을 숨겨줌
    public void HideChat()
    {
        //Area_Scroll.SetActive(!Area_Scroll.activeSelf);
        Area_Chat.SetActive(!Area_Chat.activeSelf);
    }
    // 버튼을 누르면 메세지 보내줌
    public void OnclickedEnter()
    {
        PostText(chats.text);
    }
    #endregion
    // 여기서 글자가 입력된다. 
    public void PostText(string s)
    {
        //이것은 옵션사항
        //<color=#FFFFFF>닉네임</color>
        string chatText = "<color=#"+ColorUtility.ToHtmlStringRGB(idColor)+ ">" +PhotonNetwork.NickName +"</color>" +": " + s;
        //string chatText = PhotonNetwork.NickName + ": " + s;
        photonView.RPC("RPCPostText", RpcTarget.All, chatText);
        chats.text = "";
        //InputField를 재 활성화 한다.
        chats.ActivateInputField();
    }
    #region 채팅관련 코드 모음

    [PunRPC]
    void RPCPostText(string chatText)
    {
        //0.바뀌기 전의 Content H 값 넣는다. 
        previousContentH = trContent.sizeDelta.y;
        //텍스트 오브젝트 생성
        GameObject text_Obj = Instantiate(chatItem, trContent);
        //오브젝트의 컴포넌트 가져옴
        ChatItem chat=text_Obj.GetComponent<ChatItem>();
        // 해당 컴포넌트에 정의된 함수를 발동시킨다.
        chat.SetText(chatText);

        //텍스트좌표가가 기존 스크롤 높이를 초과했을 경우
        //스크롤의 위치를 아래로 위치시킨다.
        //코루틴 시작
        StartCoroutine(AutoScrollBottom());
    }
    IEnumerator AutoScrollBottom()
    {
        //내부 코드의 작업이 모두 끝나고 해당 코루틴이 실행됨
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
}
