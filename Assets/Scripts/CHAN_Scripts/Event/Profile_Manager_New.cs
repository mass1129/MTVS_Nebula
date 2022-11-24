using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UserProfile;

public class Profile_Manager_New : MonoBehaviour
{
    //���� �� ����
    //1. ���� �г���
    //2. �̹���
    //3. Ű���� 4��
    public TMP_Text user_Nickname=null;
    public Texture user_Image = null;
    public TMP_Text[] keywords=new TMP_Text[4];
    public string user_IslandId;
    public new_ProfileInfo new_profileInfo = new new_ProfileInfo();
    public ProfileInfo temp_Info = new ProfileInfo();
    private void Awake()
    {
        user_Nickname = transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<TMP_Text>();
        //user_Image = transform.GetChild(0).GetChild(4).GetChild(0).GetComponent<RawImage>().texture;
        for (int i = 0; i < keywords.Length; i++)
        {
            keywords[i] = transform.GetChild(0).GetChild(1).GetChild(i).GetChild(0).GetComponent<TMP_Text>();
        }
    }
    // �ش� �������� Ŭ������ �� ������ �Լ� 
    public void OnNextSceneBtn()
    {
        Profile_Main_Manager.instance.btn_MoveNextScene.SetActive(true);
        Profile_Main_Manager.instance.avatarName = user_Nickname.text;
        Profile_Main_Manager.instance.islandID = temp_Info.User_Island_ID.ToString();
        if (temp_Info.texture_info == null)
        {
            // �ƹ�Ÿ ���������� �Ѿ��.
            Profile_Main_Manager.instance.hasAvatar = false;
        }
        else
        {
            //���� ������ �� �ֵ��� (�������� ������ �Ѿ��.)
            Profile_Main_Manager.instance.hasAvatar = true;
        }

    }
    public void Delate()
    {
        UserProfile_Utility.instance.Delete(user_Nickname.text);
        Profile_Main_Manager.instance.btn_MoveNextScene.SetActive(false);
        Destroy(transform.parent.gameObject);
    }
    // �ش� �������� close ��ư�� ������ ������ ������ �����Ǵ� �Լ�

}
