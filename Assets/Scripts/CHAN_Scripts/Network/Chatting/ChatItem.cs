using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatItem : MonoBehaviour
{
    // �ؽ�Ʈ 
    Text chatText;
    //�ؽ�Ʈ ��ġ
    RectTransform rt;
    float preferredH;


    void Awake()
    {
        chatText = GetComponent<Text>();
        rt = GetComponent<RectTransform>();
    }
    private void Update()
    {
        //���� �ؽ�Ʈ�� �䱸�Ǵ� ���̿� ���� ���� �� 
        //�ؽ�Ʈ �ڽ� ũ�⸦ �������Ѵ�. 
        if (preferredH != chatText.preferredHeight)
        {
            // ä�� �ؽ�Ʈ ũ�⿡ �°� ContentSize�� �����Ѵ�.
            rt.sizeDelta = new Vector2(rt.sizeDelta.x, chatText.preferredHeight);
            preferredH = chatText.preferredHeight;
        }
    }
    public void SetText(string s)
    {
        chatText.text = s;
    }

}
