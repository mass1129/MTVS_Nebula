using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Login_Manager : MonoBehaviour
{
    // �α��� ������ ������ ������. 
    // ������ ������ �ִ��� �˻��Ѵ�. 
    // ���� ������ ������ ������ ���� ������ �����´�. 
    // Ʋ������ �߸� �Է��ߴٴ� ���( "�ǹٸ� ������ �Է��� �ּ���")

    // �ӽ� ���̵�, ��й�ȣ 
    public string temp_Id;
    public string temp_Password;
    // �Է±�
    public InputField Input_Id;
    public InputField Input_Pass;
    public Text errorMassage;
    void Start()
    {
        // ó�� �����޼����� ����. 
        errorMassage.enabled=false;
    }
    public void ClickedLogInBtn()
    {
        if (Input_Id.text == temp_Id && Input_Pass.text == temp_Password)
        {
            //���� ������ ����
            errorMassage.enabled = false;
            SceneManager.LoadScene(1);
        }
        else
        {
            errorMassage.enabled = true;
            errorMassage.text = "�ùٸ� ������ �Է��� �ּ���.";
        }
    }
}
