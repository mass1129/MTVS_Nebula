using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class map_Icon_Controller : MonoBehaviour
{
    [SerializeField]
    List<Color> Icon_Colors;
    [SerializeField]
    List<string> categories;
    public GameObject IslandText;
    public GameObject Image_Online;
    public GameObject Image_Offine;
    //�� ������ �������� �޾ƿ´�. 
    //���� ������ �� 1�� ī�װ��� ���Ѵ�.

    // �������� �ٶ� ī�޶� ��ġ
    void Start()
    {
        IslandText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // ���⼭�� �������� �ٶ󺸴� ������ ������Ʈ ���ִ� �Լ��� ����. 
        // �̴ϸ� ������ �� �������� ���� �ٶ󺸵���
        if (Map_UIManager.instance.state_View == "minimap")
        {
            transform.forward = Vector3.down;
            
        }
        // �ϴø� ������ �� �������� skyī�޶�� �ٶ󺸵���
        else if (Map_UIManager.instance.state_View == "skymap")
        {
            transform.LookAt(Map_UIManager.instance.skyCam.transform);
        }
    }

    /// <summary>
    /// ���� ���� �� �ĺ��� ���ؼ� ������ �������� �Ҵ�����ִ� �Լ�
    /// </summary>
    /// <param name="category">�������� ������ 1�� ī�װ� �����̴�. ������ �ν����Ϳ��� ���� ����</param>
    public void SetIconColor(string category)
    {
        //profileIsland���� 1��ī�װ� ������ �������� 1��ī�װ������ ������ ī�װ� ����Ʈ�� ���Ѵ�.
        for (int i = 0; i < categories.Count; i++)
        {
            if (category == categories[i])
            {
                gameObject.GetComponentInChildren<Image>().color = Icon_Colors[i];
            }
        }

    }

    /// <summary>
    /// ���콺�� �ش� �����ܿ� ���ٴ��� ��, ������ �̸��� ����.
    /// </summary>
    public void VisualText(bool b)
    {
        IslandText.SetActive(b);
        IslandText.GetComponent<Text>().text = GetComponentInParent<Island_Profile>().user_name + " �� ��";
    }

    public void SetOnlineIcon(bool b)
    {
        Image_Online.SetActive(b);
        Image_Offine.SetActive(!b);
    }

}
