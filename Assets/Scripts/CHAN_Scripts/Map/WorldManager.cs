using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WorldManager : MonoBehaviour 
{
    // 하늘맵 씬의 전반적인 액션을 관리하는 스크립트
    // * 유저들에게 할당받은 좌표값을 해당 맵에 스폰할 수 있도록 만들어야함
    // * 월드의 한계선을 정해야 함, 이를 속성으로 부여
    // * 시간개념이 들어가야 함

    //속성: 월드의 너비, 높이, 한계선 경고 영역판정 비율, 유저 리스트 , 경고 텍스트 
 
    public float radius=560;
    public float warningZoneRatio;

    float warningZone_Radius;
    float DangerZone_Radius;
    public TMP_Text text_warning;
    //public Text warningText;

    private void Start()
    {
        text_warning.enabled = false;
        //warningText.enabled = false;
        warningZone_Radius = radius * (1 + warningZoneRatio * 0.01f) ;
        DangerZone_Radius = radius * 1.2f;
    }
    private void Update()
    {
        float distance = Vector3.Distance(Vector3.zero, transform.position);
        Debug.Log(distance);
        if (distance > warningZone_Radius)
        {
            text_warning.enabled = true;
            Debug.Log("영역 벗어남");
            //범위를 벗어났다고 안내
            if (distance > DangerZone_Radius)
            {
                control();
            }
            else
            {
                GetComponent<User_Move>().turn = false;
            }
        }
        else
        {
            text_warning.enabled = false;
        }
        
        
    }
    void control()
    {
        Vector3 dir = (transform.position - Vector3.zero).normalized;
        //Debug.Log("플레이어와 섬 사이 거리: "+distance);
        GetComponent<User_Move>().turn = true;
        transform.position -= dir * 10 * Time.deltaTime;
    }



    
}
