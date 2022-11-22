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
    // �ϴú� ķ
    public GameObject skyCam;
    //�̴ϸʺ� ķ
    public GameObject minimapCam;
    //���⼭ ���� ��ư�� ������ ����� �ߵ��ǵ��� ���� ���� 
    public string state_View;
    void Start()
    {
        // �ʱ⿡ ��ư��� ������ �Ѵ�.
        // �ʱ⿡�� �̴ϸ� ��� �ٷ� ��ȯ�Ѵ�.
        OnMinimapView();
    }
    /// <summary>
    /// ��ī�̸� ��ȯ ��ư
    /// ���⼭ state_View ���¸� 'skymap'�� �ٲ۴�.
    /// </summary>
    public void OnSkyView()
    {
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
        state_View = "Island";
    }
    /// <summary>
    /// ������ ���͸� ���ִ� �Լ�
    /// </summary>
    public void Toggle_Filter_User()
    {
        // ���� �� �ش� ������ �ڽ� ��� �Ѵ�.
        // ���¸� ������ ��ȯ
        state_View = "User";
    }

}
