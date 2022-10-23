using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ������ ������ ���� ��ũ��Ʈ
/// </summary>
public class Profile_Manager : MonoBehaviour
{
    #region �Ӽ� ����
    //�ڷΰ��� ��ư
    public GameObject btn_backToStart;
    // ���� �̹���
    public Image create;
    // ������ �̹���
    public Image created;
    // ���� ��ư
    public GameObject btn_create;
    // �̹��� ���ε� ��ư
    public GameObject btn_UploadImage;
    // Ű���� ���� ��ư 
    public GameObject btn_SelectKeywords;
    // ���� �Ϸ� ��ư
    public GameObject btn_Done;
    [Header("Ű���� ����â")]
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
    #region �ʱ⿡ �ش� ���� ������ �� ����Ʈ�� ������ �� ������
    public void InitializeUIs()
    {
        
        //�ڷΰ��� ��ư Ȱ��ȭ
        btn_backToStart.SetActive(true);
        // create �̹��� Ȱ��ȭ
        create.enabled = true;
        // create ��ư Ȱ��ȭ
        btn_create.SetActive(true);
        //������ ��ư ��� ��Ȱ��ȭ 
        created.enabled = false;
        OnMainSelect(false);
        btn_Done.SetActive(false);
        //Ű����â ����
        OnKeywords(false);
        print("����");
    }
    #endregion
    #region ���� �÷��̾ btn_create �� �������� �۵���
    public void AddProfile()
    {
        //btn_create ��ư�� �������
        btn_create.SetActive(false);
        // ���ε�, Ű���� ��ư�� ������. 
        OnMainSelect(true);
    }
    void OnMainSelect(bool b)
    {
        //�̹��� ���ε� ��ư
        btn_UploadImage.SetActive(b);
        //Ű���� ��ư 
        btn_SelectKeywords.SetActive(b);
    }
    #endregion
    #region Ű���� ���� ��� ���
    public void AddKeywords()
    {
        OnMainSelect(false);
        // done ��ư ��Ȱ��ȭ
        btn_Done.SetActive(false);
        // Ű����1234 ��ư, Ű���� ���� �Ϸ� ��ư Ȱ��ȭ
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
