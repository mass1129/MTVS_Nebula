using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item_TVManager : MonoBehaviour
{
    public static Item_TVManager instance;

    private void Awake()
    {
        instance = this;
    }
    public Text introduceText;
    //TV�� �������� �������� �Ǻ�
    int  isTurn=1;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isTurn *=-1;
        }
    }
    void OnSpaceBar()
    { 
        // �ڷ�ƾ 
    }
    IEnumerator MoveTV()
    {
        yield return null;
    }
    // Ƽ�� ų �� �ִ��� ������ 
    public void CanControlTV(bool b)
    {
        if (b==true)
        {
            if (isTurn==-1)
            {
                introduceText.text = "Turn Off Screen";
            }
            else
            {
                introduceText.text = "Turn On Screen";
            }
        }
        else
            introduceText.text = null;
    }
        //���⼭ TV  Text ���´�. 
       
}
