using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Profile_Main_Manager : MonoBehaviour
{
    public static Profile_Main_Manager instance;
    private void Awake()
    {
        instance = this;
    }
    public GameObject btn_backToStart;
    public GameObject btn_MoveNextScene;
    public GameObject Prefab_Profile;
    public Transform List_Profile;
    public GameObject obj;

    public bool hasAvatar;
    void Start()
    {
        //�ڷΰ��� ��ư Ȱ��ȭ
        btn_backToStart.SetActive(true);
        btn_MoveNextScene.SetActive(false);
        List_Profile = GameObject.Find("List_Profile").transform;



    }

    // Update is called once per frame
    public void AddProfileBlock()
    {
        obj = Instantiate(Prefab_Profile, List_Profile);
        obj.GetComponentInChildren<Profile_Manager>().transfer(obj);
        obj.SetActive(true);
        obj.GetComponentInChildren<Profile_Manager>().Initialize();
    }
    public void BackToPreviousScene()
    {
        SceneManager.LoadScene(0);
    }
    //���������� �̵�
    public void LoadNextScene()
    {

        string s = PlayerPrefs.GetString("AvatarName");
        string s1 = PlayerPrefs.GetString("Island_ID");
        print(s + s1);
        if (!hasAvatar)
        {
            // �ƹ�Ÿ ���������� �Ѿ��.
            SceneManager.LoadScene(2);
        }
        else
        {
            //���� ������ �� �ֵ��� (�������� ������ �Ѿ��.)
        }
    }

}
