using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

/// <summary>
/// 섬과 섬끼리 거리가 가까워 졌을 때 서로 밀어내는 모션을 관리하는 스크립트
/// </summary>
public class Island_Dynamics : MonoBehaviourPun
{
    // 하늘뷰 상에 있는 모든 하늘섬의 정보를 가져온다. 
    // 섬과 섬끼리 거리를 측정한다. 
    // 만약 거리가 가까우면 방향을 섬과 섬끼리 방향을 산출
    // 일정 거리 멀어질 때 까지 멀어지는 방향으로 힘을 부여햐 준다. 

    public float setDistance=300;
    public float forceMultiplier=500;

    void Start()
    {
 
    }
    void FixedUpdate()
    {
        //정보들 모두 로드 전까지 잠시 대기 
        if (!Island_Information.instance.Done) return;
        foreach (JsonInfo info_a in Island_Information.instance.Island_Dic.Values)
        {
            GameObject obj_a = info_a.User_Obj;
            foreach (JsonInfo info_b in Island_Information.instance.Island_Dic.Values)
            {
                GameObject obj_b = info_b.User_Obj;
                //거리계산 시작
                float distanceEachOther = Vector3.Distance(obj_a.transform.position, obj_b.transform.position);
                // 만약 a,b가 서로 다른 오브젝트일 경우 
                if (!info_a.User_Obj.Equals(info_b.User_Obj) )
                {
                    //만약 서로 다른 하늘섬이 같은 카테고리인 경우
                    if (info_a.island_Type.Equals(info_b.island_Type))
                    {//만약 거리가 일정범위 이하로 근접했을 경우
                        if (distanceEachOther < setDistance)
                        {
                            //둘의 방향을 가져옴 
                            Vector3 dir = (obj_a.transform.position - obj_b.transform.position).normalized;
                            //멀어질때까지 힘을 부여
                            Rigidbody rb = obj_a.GetComponent<Rigidbody>();
                            rb.AddForce(dir * forceMultiplier * Time.deltaTime, ForceMode.Force);
                        }
                        else if (distanceEachOther > setDistance*2)
                        {
                            //둘의 방향을 가져옴 
                            Vector3 dir = (obj_a.transform.position - obj_b.transform.position).normalized;
                            //멀어질때까지 힘을 부여
                            Rigidbody rb = obj_a.GetComponent<Rigidbody>();
                            rb.AddForce(-dir * forceMultiplier * 0.3f * Time.deltaTime, ForceMode.Force);
                        }
                    }
                    else
                    {
                        if (distanceEachOther < setDistance)
                        {
                            //둘의 방향을 가져옴 
                            Vector3 dir = (obj_a.transform.position - obj_b.transform.position).normalized;
                            //멀어질때까지 힘을 부여
                            Rigidbody rb = obj_a.GetComponent<Rigidbody>();
                            rb.AddForce(dir * forceMultiplier * Time.deltaTime, ForceMode.Force);
                        }
                    }
                }
               
            }
        }
    }
}
