using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatItem : MonoBehaviour
{
    // 텍스트 
    Text chatText;
    //텍스트 위치
    RectTransform rt;
    float preferredH;


    void Awake()
    {
        chatText = GetComponent<Text>();
        rt = GetComponent<RectTransform>();
    }
    private void Update()
    {
        //만약 텍스트가 요구되는 높이와 맞지 않을 때 
        //텍스트 박스 크기를 재조정한다. 
        if (preferredH != chatText.preferredHeight)
        {
            // 채팅 텍스트 크기에 맞게 ContentSize를 변경한다.
            rt.sizeDelta = new Vector2(rt.sizeDelta.x, chatText.preferredHeight);
            preferredH = chatText.preferredHeight;
        }
    }
    public void SetText(string s)
    {
        chatText.text = s;
    }

}
