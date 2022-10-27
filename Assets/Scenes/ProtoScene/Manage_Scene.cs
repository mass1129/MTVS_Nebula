using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Manage_Scene : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    public GameObject Btn_Next;
    public GameObject Btn_Previous;
    string[] Scenes = { "1_Log in", "2_ Add Profile", "3_ Create Character", "4_ User World", "5_ Sky World" };
    int count = 0;
    void Start()
    {

    }


    void Update()
    {

    }
    // 버튼 클릭하면 씬이동
    public void MoveNextScene()
    {
        count++;
        SceneManager.LoadScene(Scenes[count]);
    }
    public void MovePreviousScene()
    {
        count--;
        SceneManager.LoadScene(Scenes[count]);
    }
}
