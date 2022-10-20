using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Island_Setting : MonoBehaviour
{

    //���콺 �����Ͱ� �ش� �ϴü��� ����������, 
    //�ƿ������� �߰� ���� �̸��� UI�� ���̰� �ȴ�. 
    //���콺�� �ϴü��� Ŭ���ϸ� 
    //ī�޶� ��ġ�� �ϴü� ī�޶� ���������� ī�޶� lerp�̵� ��Ų��. 
    //UI_Manager���� ������ UI�� ���� �Ѵ�. (ť�����·� ������ ���� �𸣰���)
    //UI �� x ��ư ������ UI �ݱ� 
    GameObject islandObj;
    
    
    
    public GameObject profile_Image;

    private void Start()
    {
        
        
        
    }
    public void Update()
    {
        //���⼭ ���� ������ �Ÿ������� �޾ƿ;� �Ѵ�. 
        // ���� �÷��̾�� �� ���� �Ÿ��� �����Ÿ� �̳��� ���´ٸ�
       


    }
  
    IEnumerator MoveCamera()
    {
        float distance;
        Matrix4x4 wPos=islandObj.transform.GetChild(0).transform.localToWorldMatrix;
        Vector3 Wpos = wPos.GetPosition();
        while (true)
        {
            distance = Vector3.Distance(Camera.main.transform.position, Wpos);
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, Wpos, Time.deltaTime);
            if (distance < 10)
            {
                Camera.main.transform.position = Wpos;
                Camera.main.transform.LookAt(Wpos);
                break;
            }
            yield return null;
        }
        yield return null;
    }
}
