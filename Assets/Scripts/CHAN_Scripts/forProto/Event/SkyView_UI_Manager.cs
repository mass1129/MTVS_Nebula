using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 월드의 전반적인 UI를 제어하는 스크립트
/// </summary>
public class SkyView_UI_Manager : MonoBehaviour
{
    public static SkyView_UI_Manager instance;
    private void Awake()
    {
        instance = this;
    }
    //이미지
    public Image BackGround;
    public Text UserName;
    public Button AddFreind;
    public Button ExitButton;



    public void OnFriendMenu(bool o)
    {
        BackGround.transform.gameObject.SetActive(o);
        UserName.transform.gameObject.SetActive(o);
        AddFreind.transform.gameObject.SetActive(o);
        ExitButton.transform.gameObject.SetActive(o);
    }

    public void OffFriendMenu()
    {
        BackGround.transform.gameObject.SetActive(false);
        UserName.transform.gameObject.SetActive(false);
        AddFreind.transform.gameObject.SetActive(false);
        ExitButton.transform.gameObject.SetActive(false);
        User_Move.instance.islandSelected = false;
    }


}
