using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class map_Icon_Controller : MonoBehaviour
{
    [SerializeField]
    List<Material> Icon_Colors;
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
    }
    public void SetIconColor(string category)
    {
        //profileIsland���� 1��ī�װ� ������ �������� 1��ī�װ������ ������ ī�װ� ����Ʈ�� ���Ѵ�.
        for (int i = 0; i < categories.Count; i++)
        { 
            
        }
    }
}
