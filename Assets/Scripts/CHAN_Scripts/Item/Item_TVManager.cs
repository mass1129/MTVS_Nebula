//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class Item_TVManager : MonoBehaviour
//{
//    public static Item_TVManager instance;
//    public Transform prefab_Wall;
//    public Transform window;
//    public GameObject[] prefab_TVScreem;
//    Vector3 InitialPos;
//    public bool done;
//    bool moving;
        

//    private void Awake()
//    {
//        instance = this;
//    }
//    public Text introduceText;
//    //TV가 켜졌는지 꺼졌는지 판별
//    public bool isTurn;
//    void Start()
//    {
//        InitialPos = prefab_Wall.position;
//        window.position = prefab_Wall.position;
//    }
//    public void InsertScreenObject(bool b)
//    {
//        prefab_TVScreem = new GameObject[window.childCount];
//        for (int i = 0; i < window.childCount; i++)
//        {
//            prefab_TVScreem[i]=window.GetChild(i).gameObject;
//            prefab_TVScreem[i].SetActive(b);
            
//        }
//    }

//    void Update()
//    {
//        if (Input.GetKeyDown(KeyCode.Space) && done&& !moving)
//        {
//            isTurn = !isTurn;
//            OnSpaceBar();
//        }
//    }
//    public void OnSpaceBar()
//    {
//        if (isTurn)
//        {
//            Copy_Window_Texture.instance.OnClicked(true);
//        }
//        else
//        {
//            Copy_Window_Texture.instance.OnClicked(false);
//        }
//        StartCoroutine(MoveTV(isTurn));
//    }
//    IEnumerator MoveTV(bool b)
//    {
//        moving = true;
//        // TV가 땅에서 나온다.
//        // 초기에 스크린은 땅에 위치하도록(y= -5에 위치)
//        InsertScreenObject(false);
//        Vector3 SetPos = b == true ? InitialPos + Vector3.up * 10 : InitialPos;
//        float distance = Vector3.Distance(prefab_Wall.position, SetPos);
//        while (distance > 0.1f)
//        {
//            prefab_Wall.position = Vector3.Lerp(prefab_Wall.position, SetPos, Time.deltaTime * 2);
//            distance=Vector3.Distance(prefab_Wall.position, SetPos);
//            window.position = prefab_Wall.position;
//            yield return null;
//        }
//        prefab_Wall.position = SetPos;
//        window.position -= window.transform.forward * 0.51f;
//        //버튼을 누르면 y=10 만큼 좌표 lerp하게 이동 
//        moving = false;

//    }
//    // 티비 킬 수 있는지 없는지 
//    public void CanControlTV(bool b)
//    {
//        if (b==true)
//        {
//            if (isTurn)
//            {
//                introduceText.text = "Turn Off Screen";
//            }
//            else
//            {
//                introduceText.text = "Turn On Screen";
//            }
//        }
//        else
//            introduceText.text = null;
//    }
//        //여기서 TV  Text 나온다. 
       
//}
