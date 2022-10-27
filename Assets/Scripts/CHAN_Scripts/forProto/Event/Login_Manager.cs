using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Login_Manager : MonoBehaviour
{
    // 로그인 정보를 서버에 보낸다. 
    // 정보가 서버에 있는지 검사한다. 
    // 만약 정보가 서버에 있으면 유저 정보를 가져온다. 
    // 틀렸으면 잘못 입력했다는 경고( "옳바른 정보를 입력해 주세요")

    // 임시 아이디, 비밀번호 
    public string temp_Id;
    public string temp_Password;
    // 입력기
    public InputField Input_Id;
    public InputField Input_Pass;
    public Text errorMassage;
    void Start()
    {
        // 처음 에러메세지는 끈다. 
        errorMassage.enabled=false;
    }
    public void ClickedLogInBtn()
    {
        if (Input_Id.text == temp_Id && Input_Pass.text == temp_Password)
        {
            //다음 씬으로 입장
            errorMassage.enabled = false;
            SceneManager.LoadScene(1);
        }
        else
        {
            errorMassage.enabled = true;
            errorMassage.text = "올바른 정보를 입력해 주세요.";
        }
    }
}
