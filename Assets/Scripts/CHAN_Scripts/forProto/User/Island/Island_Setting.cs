using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Island_Setting : MonoBehaviour
{

    //마우스 포인터가 해당 하늘섬에 접근했을때, 
    //아웃라인이 뜨고 유저 이름이 UI로 보이게 된다. 
    //마우스로 하늘섬을 클릭하면 
    //카메라 위치를 하늘섬 카메라 포지션으로 카메라를 lerp이동 시킨다. 
    //UI_Manager에서 설정한 UI를 띄우게 한다. (큐브형태로 구현은 아직 모르겠음)
    //UI 의 x 버튼 누르면 UI 닫기 
    GameObject islandObj;
    
    public void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // 마우스가 닿은 지점의 게임오브젝트를 가져온다.
        if (Physics.Raycast(ray, out hit, 500))
        {
            islandObj = hit.transform.gameObject;
            islandObj.GetComponent<Outline>().enabled = true;
            //여기서 마우스 클릭을 하면 프로필 UI가 나오도록 한다.
            if (Input.GetMouseButtonDown(0))
            {
                //UI창 띄우는 기능을 만들자 
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
        // 마우스 포인터(V2좌표에서 월드로 ray 쏘도록 한다. 
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
