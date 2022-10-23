using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 프로필 선택지 관련 스크립트
/// </summary>
public class Profile_Manager : MonoBehaviour
{
    #region 속성 모음
    //뒤로가기 버튼
    public GameObject btn_backToStart;
    // 생성 이미지
    public Image create;
    // 생성된 이미지
    public Image created;
    // 생성 버튼
    public GameObject btn_create;
    // 이미지 업로드 버튼
    public GameObject btn_UploadImage;
    // 키워드 선택 버튼 
    public GameObject btn_SelectKeywords;
    // 생성 완료 버튼
    public GameObject btn_Done;
    [Header("키워드 선택창")]
    public GameObject input_keyword_1;
    public GameObject input_keyword_2;
    public GameObject input_keyword_3;
    public GameObject input_keyword_4;
    public GameObject btn_keywordsDone;
    #endregion
    void Start()
    {
        InitializeUIs();
    }

 
    void Update()
    {
        
    }
    #region 초기에 해당 씬에 들어왔을 때 디폴트로 나오게 할 설정들
    public void InitializeUIs()
    {
        
        //뒤로가기 버튼 활성화
        btn_backToStart.SetActive(true);
        // create 이미지 활성화
        create.enabled = true;
        // create 버튼 활성화
        btn_create.SetActive(true);
        //나머지 버튼 모두 비활성화 
        created.enabled = false;
        OnMainSelect(false);
        btn_Done.SetActive(false);
        //키워드창 꺼짐
        OnKeywords(false);
        print("꺼짐");
    }
    #endregion
    #region 만약 플레이어가 btn_create 를 눌렀을때 작동됨
    public void AddProfile()
    {
        //btn_create 버튼이 사라지고
        btn_create.SetActive(false);
        // 업로드, 키워드 버튼이 켜진다. 
        OnMainSelect(true);
    }
    void OnMainSelect(bool b)
    {
        //이미지 업로드 버튼
        btn_UploadImage.SetActive(b);
        //키워드 버튼 
        btn_SelectKeywords.SetActive(b);
    }
    #endregion
    #region 키워드 관련 기능 목록
    public void AddKeywords()
    {
        OnMainSelect(false);
        // done 버튼 비활성화
        btn_Done.SetActive(false);
        // 키워드1234 버튼, 키워드 선택 완료 버튼 활성화
        OnKeywords(true);
        
    }
    void OnKeywords(bool b)
    {
        input_keyword_1.SetActive(b);
        input_keyword_2.SetActive(b);
        input_keyword_3.SetActive(b);
        input_keyword_4.SetActive(b);
        btn_keywordsDone.SetActive(b);
    }
    #endregion

}
