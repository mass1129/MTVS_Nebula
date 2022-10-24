using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� ������ �Ÿ��� ����� ���� �� ���� �о�� ����� �����ϴ� ��ũ��Ʈ
/// </summary>
public class Island_Dynamics : MonoBehaviour
{
    // �ϴú� �� �ִ� ��� �ϴü��� ������ �����´�. 
    // ���� ������ �Ÿ��� �����Ѵ�. 
    // ���� �Ÿ��� ������ ������ ���� ������ ������ ����
    // ���� �Ÿ� �־��� �� ���� �־����� �������� ���� �ο��� �ش�. 
    GameObject[] Islands;
    public float setDistance;
    public float forceMultiplier;
    public string user_name;

    void Start()
    {
        // ������Ʈ �߿� "UserIsland" �±װ� �ٿ��� ������Ʈ�� �迭�� �߰��Ѵ�. 
        
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        foreach (string a in IslandInformation.instance.Island_Dic.Keys)
        {
            GameObject obj_a = IslandInformation.instance.Island_Dic[a].User_Obj;
            foreach (string b in IslandInformation.instance.Island_Dic.Keys)
            {
                GameObject obj_b = IslandInformation.instance.Island_Dic[b].User_Obj;
                // ���� a,b�� ���� �ٸ� ������Ʈ�� ���
                if (!a.Equals(b))
                {
                    
                    //�Ÿ���� ����
                    float distanceEachOther = Vector3.Distance(obj_a.transform.position, obj_b.transform.position);
                    //���� �Ÿ��� �������� ���Ϸ� �������� ���
                    if (distanceEachOther < setDistance)
                    {
                        //���� ������ ������ 
                        Vector3 dir = (obj_a.transform.position -obj_b.transform.position).normalized;
                        //�־��������� ���� �ο�
                        Rigidbody rb = obj_a.GetComponent<Rigidbody>();
                        rb.AddForce(dir * forceMultiplier * Time.deltaTime, ForceMode.Force);
                    }
                }
            }
        }
    }
}
