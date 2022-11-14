using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChattingSet : MonoBehaviourPun
{
    
    //��ǲ �ʵ��� ���� �����´�.
    [Header("�Էº�")]
    public InputField chats;
 
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
    public Button btn_Enter;
    public Button btn_HideChatMenu;
    [Header("��Ÿ")]
    Color32 idColor;


    void Start()
    {
        //�� ��ư�� ����� �Լ����� �Ҵ��Ѵ�.
        btn_HideChatMenu.onClick.AddListener(HideChat);
        btn_Enter.onClick.AddListener(OnclickedEnter);
        //����Ű�� ������ �� �Լ��� �ߵ��ǵ��� �����Ѵ�.
        chats.onSubmit.AddListener(PostText);
        // ���� �г��� ���� �����ϱ����� ��������
        idColor = new Color32(
            (byte)Random.Range(0, 256),
            (byte)Random.Range(0, 256),
            (byte)Random.Range(0, 256),
            255
            );
    }
    #region ��ư ���� 
    // ä������ ������
    public void HideChat()
    {
        //Area_Scroll.SetActive(!Area_Scroll.activeSelf);
        Area_Chat.SetActive(!Area_Chat.activeSelf);
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
        ChatItem chat=text_Obj.GetComponent<ChatItem>();
        // �ش� ������Ʈ�� ���ǵ� �Լ��� �ߵ���Ų��.
        chat.SetText(chatText);

        //�ؽ�Ʈ��ǥ���� ���� ��ũ�� ���̸� �ʰ����� ���
        //��ũ���� ��ġ�� �Ʒ��� ��ġ��Ų��.
        //�ڷ�ƾ ����
        StartCoroutine(AutoScrollBottom());
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
