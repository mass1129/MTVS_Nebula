using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

/// <summary>
/// ���� ������ �Ÿ��� ����� ���� �� ���� �о�� ����� �����ϴ� ��ũ��Ʈ
/// </summary>
public class Island_Dynamics : MonoBehaviourPun
{
    // �ϴú� �� �ִ� ��� �ϴü��� ������ �����´�. 
    // ���� ������ �Ÿ��� �����Ѵ�. 
    // ���� �Ÿ��� ������ ������ ���� ������ ������ ����
    // ���� �Ÿ� �־��� �� ���� �־����� �������� ���� �ο��� �ش�. 

    public float setDistance;
    public float forceMultiplier;
    public string user_name;

    void Start()
    {
        // ������Ʈ �߿� "UserIsland" �±װ� �ٿ��� ������Ʈ�� �迭�� �߰��Ѵ�. 
        
        
    }

    void FixedUpdate()
    {
        //������ ��� �ε� ������ ��� ��� 
        if (!IslandInformation.instance.Done) return;
        // ������ ��� �ε� �Ǹ� ���� �ۿ� ����
        //foreach (string a in IslandInformation.instance.Island_Dic.Keys)
        //{
        //    GameObject obj_a = IslandInformation.instance.Island_Dic[a].User_Obj;
        //    foreach (string b in IslandInformation.instance.Island_Dic.Keys)
        //    {
        //        GameObject obj_b = IslandInformation.instance.Island_Dic[b].User_Obj;
        //        // ���� a,b�� ���� �ٸ� ������Ʈ�� ��� �׸��� ���� ���� ī�װ��� ���
        //        if (!a.Equals(b)&&)
        //        {
                    
        //            //�Ÿ���� ����
        //            float distanceEachOther = Vector3.Distance(obj_a.transform.position, obj_b.transform.position);
        //            //���� �Ÿ��� �������� ���Ϸ� �������� ���
        //            if (distanceEachOther < setDistance)
        //            {
        //                //���� ������ ������ 
        //                Vector3 dir = (obj_a.transform.position -obj_b.transform.position).normalized;
        //                //�־��������� ���� �ο�
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
                //�Ÿ���� ����
                float distanceEachOther = Vector3.Distance(obj_a.transform.position, obj_b.transform.position);
                // ���� a,b�� ���� �ٸ� ������Ʈ�� ��� 
                if (!info_a.User_Obj.Equals(info_b.User_Obj) )
                {
                    //���� ���� �ٸ� �ϴü��� ���� ī�װ��� ���
                    if (info_a.island_Type.Equals(info_b.island_Type))
                    {//���� �Ÿ��� �������� ���Ϸ� �������� ���
                        if (distanceEachOther < setDistance)
                        {
                            //���� ������ ������ 
                            Vector3 dir = (obj_a.transform.position - obj_b.transform.position).normalized;
                            //�־��������� ���� �ο�
                            Rigidbody rb = obj_a.GetComponent<Rigidbody>();
                            rb.AddForce(dir * forceMultiplier * Time.deltaTime, ForceMode.Force);
                        }
                        else if (distanceEachOther > setDistance*2)
                        {
                            //���� ������ ������ 
                            Vector3 dir = (obj_a.transform.position - obj_b.transform.position).normalized;
                            //�־��������� ���� �ο�
                            Rigidbody rb = obj_a.GetComponent<Rigidbody>();
                            rb.AddForce(-dir * forceMultiplier * 0.3f * Time.deltaTime, ForceMode.Force);
                        }
                    }
                    else
                    {
                        if (distanceEachOther < setDistance)
                        {
                            //���� ������ ������ 
                            Vector3 dir = (obj_a.transform.position - obj_b.transform.position).normalized;
                            //�־��������� ���� �ο�
                            Rigidbody rb = obj_a.GetComponent<Rigidbody>();
                            rb.AddForce(dir * forceMultiplier * Time.deltaTime, ForceMode.Force);
                        }
                    }

                    

                }
               
            }
        }
    }
}
