using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item_TVManager : MonoBehaviour
{
    public static Item_TVManager instance;
    public Transform prefab_Wall;
    public Transform window;
    public GameObject[] prefab_TVScreem;
    
    public bool done;
        

    private void Awake()
    {
        instance = this;
    }
    public Text introduceText;
    //TV�� �������� �������� �Ǻ�
    int  isTurn=-1;
    void Start()
    {
        window.position = prefab_Wall.position;
    }
    public void InsertScreenObject(bool b)
    {
        prefab_TVScreem = new GameObject[window.childCount];
        for (int i = 0; i < window.childCount; i++)
        {
            prefab_TVScreem[i]=window.GetChild(i).gameObject;
            prefab_TVScreem[i].SetActive(b);
            
        }
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
        if (isTurn == 1)
        {
            Copy_Window_Texture.instance.OnClicked(true);
        }
        else
        {
            Copy_Window_Texture.instance.OnClicked(false);
        }
        
        StartCoroutine(MoveTV(isTurn));
    }
    IEnumerator MoveTV(int i)
    {
        done = false;
        // TV�� ������ ���´�.
        // �ʱ⿡ ��ũ���� ���� ��ġ�ϵ���(y= -5�� ��ġ)
        InsertScreenObject(false);
        
        float multi = i * 10; 
        Vector3 SetPos = prefab_Wall.position + Vector3.up * multi;
        float distance = Vector3.Distance(prefab_Wall.position, SetPos);
        while (distance > 0.1f)
        {
            prefab_Wall.position = Vector3.Lerp(prefab_Wall.position, SetPos, Time.deltaTime * 2);
            distance=Vector3.Distance(prefab_Wall.position, SetPos);
            window.position = prefab_Wall.position;
            yield return null;
        }
        window.position -= transform.forward * 0.51f;
        //��ư�� ������ y=10 ��ŭ ��ǥ lerp�ϰ� �̵� 
        done = true;

    }
    // Ƽ�� ų �� �ִ��� ������ 
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
        //���⼭ TV  Text ���´�. 
       
}
