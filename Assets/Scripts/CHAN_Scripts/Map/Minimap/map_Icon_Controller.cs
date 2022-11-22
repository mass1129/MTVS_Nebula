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
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // ���⼭�� �������� �ٶ󺸴� ������ ������Ʈ ���ִ� �Լ��� ����. 
        //�������� �ٶ󺸴� ������ �׻� 
    }
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
