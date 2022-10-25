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
    //TV가 켜졌는지 꺼졌는지 판별
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
        // 코루틴 
    }
    IEnumerator MoveTV()
    {
        yield return null;
    }
    // 티비 킬 수 있는지 없는지 
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
        //여기서 TV  Text 나온다. 
       
}
