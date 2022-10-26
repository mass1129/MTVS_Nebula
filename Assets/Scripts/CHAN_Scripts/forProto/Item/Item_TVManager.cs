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
    //TV�� �������� �������� �Ǻ�
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
        // TV�� ������ ���´�.
        // �ʱ⿡ ��ũ���� ���� ��ġ�ϵ���(y= -5�� ��ġ)
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
        //��ư�� ������ y=10 ��ŭ ��ǥ lerp�ϰ� �̵� 
        // �̵��ϸ鼭 ��ǥ ���̿� �������� ���� ���ϱ� 
        if (isTurn == 1)
        {
            prefab_TVScreem.SetActive(true);
        }
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
