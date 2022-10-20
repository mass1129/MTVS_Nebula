using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 섬과 섬끼리 거리가 가까워 졌을 때 서로 밀어내는 모션을 관리하는 스크립트
/// </summary>
public class Island_Dynamics : MonoBehaviour
{
    // 하늘뷰 상에 있는 모든 하늘섬의 정보를 가져온다. 
    // 섬과 섬끼리 거리를 측정한다. 
    // 만약 거리가 가까우면 방향을 섬과 섬끼리 방향을 산출
    // 일정 거리 멀어질 때 까지 멀어지는 방향으로 힘을 부여햐 준다. 
    GameObject[] Islands;
    public float setDistance;
    public float forceMultiplier;

    void Start()
    {
        // 오브젝트 중에 "UserIsland" 태그가 붙여진 오브젝트를 배열에 추가한다. 
        
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        foreach (GameObject a in IslandInformation.instance.UserObj)
        {
            foreach (GameObject b in IslandInformation.instance.UserObj)
            {
                // 만약 a,b가 서로 다른 오브젝트일 경우
                if (!a.Equals(b))
                {
                    //거리계산 시작
                    float distanceEachOther = Vector3.Distance(a.transform.position, b.transform.position);
                    //만약 거리가 일정범위 이하로 근접했을 경우
                    if (distanceEachOther < setDistance)
                    {
                        //둘의 방향을 가져옴 
                        Vector3 dir = (a.transform.position - b.transform.position).normalized;
                        //멀어질때까지 힘을 부여
                        Rigidbody rb = a.GetComponent<Rigidbody>();
                        rb.AddForce(dir * forceMultiplier * Time.deltaTime, ForceMode.Force);
                    }
                }
            }
        }
    }
}
