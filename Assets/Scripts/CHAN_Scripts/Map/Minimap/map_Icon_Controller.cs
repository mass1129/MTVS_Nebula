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
    //�� ������ �������� �޾ƿ´�. 
    //���� ������ �� 1�� ī�װ��� ���Ѵ�.

    // �������� �ٶ� ī�޶� ��ġ
    GameObject cam_Lookup;
    void Start()
    {
        cam_Lookup = GameObject.Find("SkyMap_Camera");
    }

    // Update is called once per frame
    void Update()
    {
        // ���⼭�� �������� �ٶ󺸴� ������ ������Ʈ ���ִ� �Լ��� ����. 
        // �̴ϸ� ������ �� �������� ���� �ٶ󺸵���
        if (Map_UIManager.instance.state_View == "minimap")
        {
            transform.forward = Vector3.up;
        }
        // �ϴø� ������ �� �������� skyī�޶�� �ٶ󺸵���
        else if (Map_UIManager.instance.state_View == "skymap")
        {
            transform.LookAt(cam_Lookup.transform);
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
}
