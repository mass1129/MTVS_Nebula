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

    public float setDistance=300;
    public float forceMultiplier=500;

    void Start()
    {
 
    }
    void FixedUpdate()
    {
        //������ ��� �ε� ������ ��� ��� 
        if (!Island_Information.instance.Done) return;
        foreach (JsonInfo info_a in Island_Information.instance.Island_Dic.Values)
        {
            GameObject obj_a = info_a.User_Obj;
            foreach (JsonInfo info_b in Island_Information.instance.Island_Dic.Values)
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
