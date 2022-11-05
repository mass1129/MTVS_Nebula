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

    public float setDistance;
    public float forceMultiplier;
    public string user_name;

    void Start()
    {
        // 오브젝트 중에 "UserIsland" 태그가 붙여진 오브젝트를 배열에 추가한다. 
        
        
    }

    void FixedUpdate()
    {
        //정보들 모두 로드 전까지 잠시 대기 
        if (!IslandInformation.instance.Done) return;
        // 정보가 모두 로드 되면 물리 작용 시작
        //foreach (string a in IslandInformation.instance.Island_Dic.Keys)
        //{
        //    GameObject obj_a = IslandInformation.instance.Island_Dic[a].User_Obj;
        //    foreach (string b in IslandInformation.instance.Island_Dic.Keys)
        //    {
        //        GameObject obj_b = IslandInformation.instance.Island_Dic[b].User_Obj;
        //        // 만약 a,b가 서로 다른 오브젝트일 경우 그리고 서로 같은 카테고리일 경우
        //        if (!a.Equals(b)&&)
        //        {
                    
        //            //거리계산 시작
        //            float distanceEachOther = Vector3.Distance(obj_a.transform.position, obj_b.transform.position);
        //            //만약 거리가 일정범위 이하로 근접했을 경우
        //            if (distanceEachOther < setDistance)
        //            {
        //                //둘의 방향을 가져옴 
        //                Vector3 dir = (obj_a.transform.position -obj_b.transform.position).normalized;
        //                //멀어질때까지 힘을 부여
        //                Rigidbody rb = obj_a.GetComponent<Rigidbody>();
        //                rb.AddForce(dir * forceMultiplier * Time.deltaTime, ForceMode.Force);
        //            }
                   
        //        }
        //    }
        //}
        foreach (JsonInfo info_a in IslandInformation.instance.Island_Dic.Values)
        {
            GameObject obj_a = info_a.User_Obj;
            foreach (JsonInfo info_b in IslandInformation.instance.Island_Dic.Values)
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
