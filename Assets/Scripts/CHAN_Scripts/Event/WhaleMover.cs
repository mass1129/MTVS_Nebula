using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhaleMover : MonoBehaviour
{
    //생성 모션 재생되면서 등장
    // 방향을 결정 (결정방법은 랜덤한 섬의 위치)
    // 방향이 결정되면 그쪽 방향으로 회전(일정한 각속도를 가지며 회전)
    // 고래는 일정한 속도로 이동한다. 
    // 단지 고래거리와 섬거리가 일정범위안에 도달하면 그때부터 다시 탐색을 시작하고 방향을 전환

    //고래 속도 속성
    public float speed;
    public float setDistance;
    Vector3 targetIslandPos;
    Quaternion rotGoal;
    Transform targets;
    void Start()
    {
        targets = GameObject.Find("Islands").transform;
        StartCoroutine(delay());
        DefineDirection();
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, rotGoal, Time.deltaTime);
        transform.position += transform.forward * speed * Time.deltaTime;

        // 고래와 타겟섬의 거리를 계속 측정한다. 
        CheckDistance();
    }
    void DefineDirection()
    {
        int randomKey = Random.Range(0, targets.childCount-1);
        //Island_Information iInfo = Island_Information.instance;
        //int randomKey = Random.Range(1, iInfo.Island_Dic.Count);
        //targetIslandPos = iInfo.Island_Dic[randomKey.ToString()].island_Pos * iInfo.dis_multiplier;
        //임시 방향 결정
        //Vector3 _dir = (transform.position - targetIslandPos).normalized;
        //Vector3 _dir = (transform.position - targets.GetChild(randomKey).position).normalized;
        targetIslandPos = targets.GetChild(randomKey).position;
        rotGoal = Quaternion.LookRotation(targetIslandPos);

        
    }
    void CheckDistance()
    {
        float dis = Vector3.Distance(transform.position, targetIslandPos);
        Debug.Log(dis);
        // 만약 목표거리 이하로 들어왔을 시 방향을 바꾼다.
        if(dis< setDistance)
        {
            DefineDirection();
        }
    }
    IEnumerator delay()
    {
        yield return new WaitForSeconds(3);
    }



    
}
