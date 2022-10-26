using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item_TVManager : MonoBehaviour
{
    public static Item_TVManager instance;
    public Transform prefab_Wall;
    public GameObject prefab_TVScreem;
    
    public bool done;
        

    private void Awake()
    {
        instance = this;
    }
    public Text introduceText;
    //TV가 켜졌는지 꺼졌는지 판별
    int  isTurn=-1;
    void Start()
    {
        prefab_TVScreem.SetActive(false);
        
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)&&done)
        {
            isTurn *=-1;
            OnSpaceBar();
        }
    }
    void OnSpaceBar()
    {
        StartCoroutine(MoveTV(isTurn));
    }
    IEnumerator MoveTV(int i)
    {
        done = false;
        // TV가 땅에서 나온다.
        // 초기에 스크린은 땅에 위치하도록(y= -5에 위치)
        if (isTurn == -1)
        {
            prefab_TVScreem.SetActive(false);
        }
        float multi = i * 10; 
        Vector3 SetPos = prefab_Wall.position + Vector3.up * multi;
        float distance = Vector3.Distance(prefab_Wall.position, SetPos);
        while (distance > 0.1f)
        {
            prefab_Wall.position = Vector3.Lerp(prefab_Wall.position, SetPos, Time.deltaTime * 2);
            distance=Vector3.Distance(prefab_Wall.position, SetPos);
            yield return null;
        }
        //버튼을 누르면 y=10 만큼 좌표 lerp하게 이동 
        // 이동하면서 목표 높이와 기존높이 차이 구하기 
        if (isTurn == 1)
        {
            prefab_TVScreem.SetActive(true);
        }
        done = true;

    }
    // 티비 킬 수 있는지 없는지 
    public void CanControlTV(bool b)
    {
        if (b==true)
        {
            if (isTurn==1)
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
