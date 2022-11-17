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

    public string avatarName;
    public string islandID;
    public bool hasAvatar;
    void Start()
    {
        //뒤로가기 버튼 활성화
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
        // 로그아웃
        SceneManager.LoadScene(0);
    }
    //다음씬으로 이동
    public void LoadNextScene()
    {
        PlayerPrefs.SetString("AvatarName", avatarName);
        PlayerPrefs.SetString("Island_ID", islandID);
        PlayerPrefs.SetString("Cur_Island", islandID);
        print("AvatarName: " + PlayerPrefs.GetString("AvatarName"));
        print("Island_ID: " + PlayerPrefs.GetString("Island_ID"));
        if (!hasAvatar)
        {
            // 아바타 생선씬으로 넘어간다.
            SceneManager.LoadScene(2);
        }
        else
        {
            SceneManager.LoadScene(3);
        }
    }

}
