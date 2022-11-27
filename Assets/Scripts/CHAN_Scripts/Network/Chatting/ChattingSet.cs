using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UltimateClean;
using System;
using Random = UnityEngine.Random;
public class ChattingSet : MonoBehaviourPun
{
    
    //��ǲ �ʵ��� ���� �����´�.
    [Header("�Էº�")]
    public TMP_InputField chats;

    public TMP_Text chatsAmount;
    [HideInInspector]public int _chatAmount;
    public GameObject chatItem;


    [Header("ä�ÿ��� ���� ������")]
    // ���� Content �� H
    float previousContentH;
    public RectTransform trContent;
    // ScrollView �� RectTransform
    public RectTransform trScrollView;
    public GameObject Area_Scroll;
    public GameObject Area_Chat;
    [Header("��ư ���� ������")]
    public CleanButton btn_Enter;
    public CleanButton btn_HideChatMenu;
    [Header("��Ÿ")]
    Color32 idColor;
    [HideInInspector]public bool isOpened= false;

    public Animator imgAnim;
    public GameObject waveImg;
    void Start()
    {
        //�� ��ư�� ����� �Լ����� �Ҵ��Ѵ�.
        btn_HideChatMenu.onClick.AddListener(HideChat);
        btn_Enter.onClick.AddListener(OnclickedEnter);
        //����Ű�� ������ �� �Լ��� �ߵ��ǵ��� �����Ѵ�.
        chats.onSubmit.AddListener(PostText);
        isOpened = false;
        // ���� �г��� ���� �����ϱ����� ��������
            idColor = new Color32(
            (byte)Random.Range(0, 256),
            (byte)Random.Range(0, 256),
            (byte)Random.Range(0, 256),
            255
            );
        Area_Chat.GetComponent<CanvasGroup>().alpha = 0;
        Area_Chat.GetComponent<CanvasGroup>().blocksRaycasts = false;
       
        

    }
    #region ��ư ���� 
    // ä������ ������
    public void HideChat()
    {
        //Area_Scroll.SetActive(!Area_Scroll.activeSelf);
        Area_Chat.GetComponent<CanvasGroup>().alpha = 0;
        Area_Chat.GetComponent<CanvasGroup>().blocksRaycasts = false;
        _chatAmount = 0;
        isOpened = false;
    }
    // ��ư�� ������ �޼��� ������
    public void OnclickedEnter()
    {
        PostText(chats.text);
    }
    #endregion
    // ���⼭ ���ڰ� �Էµȴ�. 
    public void PostText(string s)
    {
        //�̰��� �ɼǻ���
        //<color=#FFFFFF>�г���</color>
        string chatText = "<color=#"+ColorUtility.ToHtmlStringRGB(idColor)+ ">" +PhotonNetwork.NickName +"</color>" +": " + s;
        //string chatText = PhotonNetwork.NickName + ": " + s;
        photonView.RPC("RPCPostText", RpcTarget.All, chatText);
        photonView.RPC("RPCPostCount", RpcTarget.Others);
        
        chats.text = "";
        //InputField�� �� Ȱ��ȭ �Ѵ�.
        chats.ActivateInputField();
    }
    #region ä�ð��� �ڵ� ����

    [PunRPC]
    void RPCPostText(string chatText)
    {
        //0.�ٲ�� ���� Content H �� �ִ´�. 
        previousContentH = trContent.sizeDelta.y;
        //�ؽ�Ʈ ������Ʈ ����
        GameObject text_Obj = Instantiate(chatItem, trContent);
        //������Ʈ�� ������Ʈ ������
        ChatItem chat = text_Obj.GetComponent<ChatItem>();
        // �ش� ������Ʈ�� ���ǵ� �Լ��� �ߵ���Ų��.
        chat.chatText.text = chatText;
        
        //�ؽ�Ʈ��ǥ���� ���� ��ũ�� ���̸� �ʰ����� ���
        //��ũ���� ��ġ�� �Ʒ��� ��ġ��Ų��.
        //�ڷ�ƾ ����
        StartCoroutine(AutoScrollBottom());
    }
    [PunRPC]
    void RPCPostCount()
    {
        if (isOpened ) return;
        _chatAmount++;
        chatsAmount.text = _chatAmount.ToString();
 
        imgAnim.enabled = true;

        waveImg.SetActive(true);
        
    }
    IEnumerator AutoScrollBottom()
    {
        //���� �ڵ��� �۾��� ��� ������ �ش� �ڷ�ƾ�� �����
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
}
