using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Map_UIManager : MonoBehaviour
{
    public static Map_UIManager instance;
    private void Awake()
    {
        instance = this;
    }
    #region ��ư ���� ����
    Button btn_Toggle_Island;
    Button btn_Toggle_Users;
    Button btn_Toggle_CloseTab;
    Button btn_ShowSkyMap;
    #endregion
    //��ī�̸� UI ������Ʈ
    public GameObject UI_Skymap;
    //�̴ϸ� UI ������Ʈ
    public GameObject UI_Miinimap;
    // �ϴú� ķ
    public GameObject skyCam;
    //�̴ϸʺ� ķ
    public GameObject minimapCam;
    //���⼭ ���� ��ư�� ������ ����� �ߵ��ǵ��� ���� ���� 
    public string state_View;
    void Start()
    {
        #region �� ��ư�� ��ġ�� ã�´�. 
        btn_Toggle_Island = GameObject.Find("Btn_Toggle_Island").GetComponent<Button>();
        btn_Toggle_Users = GameObject.Find("Btn_Toggle_Users").GetComponent<Button>();
        btn_Toggle_CloseTab = GameObject.Find("Btn_CloseTab").GetComponent<Button>();
        btn_ShowSkyMap = GameObject.Find("Btn_ShowSkyMap").GetComponent<Button>();
        #endregion
        #region �� ��ư�� ����� �����Ѵ�.
        btn_Toggle_Island.onClick.AddListener(Toggle_Filter_Island);
        btn_Toggle_Users.onClick.AddListener(Toggle_Filter_User);
        btn_Toggle_CloseTab.onClick.AddListener(OnMinimapView);
        btn_ShowSkyMap.onClick.AddListener(OnSkyView);
        #endregion
        // �ʱ⿡�� �̴ϸ� ��� �ٷ� ��ȯ�Ѵ�.
        OnMinimapView();
    }
    /// <summary>
    /// ��ī�̸� ��ȯ ��ư
    /// ���⼭ state_View ���¸� 'skymap'�� �ٲ۴�.
    /// </summary>
    public void OnSkyView()
    {
        UI_Skymap.SetActive(true);
        UI_Miinimap.SetActive(false);
        state_View = "skymap";
        skyCam.SetActive(true);
        minimapCam.SetActive(false);
        Debug.Log("��ī�� ��� ��ȯ");
    }
    /// <summary>
    /// �̴ϸ� ��ȯ ��ư
    /// ���⼭ state_View ���¸� 'minimap'�� �ٲ۴�.
    /// </summary>
    public void OnMinimapView()
    {
        UI_Skymap.SetActive(false);
        UI_Miinimap.SetActive(true);
        state_View = "minimap";
        skyCam.SetActive(false);
        minimapCam.SetActive(true);
        Debug.Log("�̴ϸ� ��� ��ȯ");
    }

    /// <summary>
    /// �ϴü��� ���͸��ϴ� �Լ�
    /// </summary>
    public void Toggle_Filter_Island()
    {
        // ���� �� �ش� ������ �ڽ� ��� �Ѵ�. 
        // ���¸� �ϴü����� ��ȯ
        Debug.Log("�ϴü��� ���̵��� ��ȯ");
    }
    /// <summary>
    /// ������ ���͸� ���ִ� �Լ�
    /// </summary>
    public void Toggle_Filter_User()
    {
        // ���� �� �ش� ������ �ڽ� ��� �Ѵ�.
        // ���¸� ������ ��ȯ
        Debug.Log("������ ���̵��� ��ȯ");

    }

}
