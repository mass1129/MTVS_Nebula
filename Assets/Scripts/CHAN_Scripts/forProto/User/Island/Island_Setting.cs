using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Island_Setting : MonoBehaviour
{

    //���콺 �����Ͱ� �ش� �ϴü��� ����������, 
    //�ƿ������� �߰� ���� �̸��� UI�� ���̰� �ȴ�. 
    //���콺�� �ϴü��� Ŭ���ϸ� 
    //ī�޶� ��ġ�� �ϴü� ī�޶� ���������� ī�޶� lerp�̵� ��Ų��. 
    //UI_Manager���� ������ UI�� ���� �Ѵ�. (ť�����·� ������ ���� �𸣰���)
    //UI �� x ��ư ������ UI �ݱ� 
    GameObject islandObj;
    
    public void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // ���콺�� ���� ������ ���ӿ�����Ʈ�� �����´�.
        if (Physics.Raycast(ray, out hit, 500))
        {
            islandObj = hit.transform.gameObject;
            islandObj.GetComponent<Outline>().enabled = true;
            //���⼭ ���콺 Ŭ���� �ϸ� ������ UI�� �������� �Ѵ�.
            if (Input.GetMouseButtonDown(0))
            {
                //UIâ ���� ����� ������ 
                UI_Manager.instance.OnFriendMenu(true);
                StartCoroutine(MoveCamera());
                User_Move.instance.islandSelected = true;
            }
        }
        else
        {
            if (!islandObj)
                return;
            islandObj.GetComponent<Outline>().enabled = false;
            

        }
        
       
    }
    void MousePointer()
    {
        // ���콺 ������(V2��ǥ���� ����� ray ��� �Ѵ�. 
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
