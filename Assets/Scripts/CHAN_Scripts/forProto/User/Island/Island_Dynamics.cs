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

    void Start()
    {
        // ������Ʈ �߿� "UserIsland" �±װ� �ٿ��� ������Ʈ�� �迭�� �߰��Ѵ�. 
        
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        foreach (GameObject a in IslandInformation.instance.UserObj.Values)
        {
            foreach (GameObject b in IslandInformation.instance.UserObj.Values)
            {
                // ���� a,b�� ���� �ٸ� ������Ʈ�� ���
                if (!a.Equals(b))
                {
                    //�Ÿ���� ����
                    float distanceEachOther = Vector3.Distance(a.transform.position, b.transform.position);
                    //���� �Ÿ��� �������� ���Ϸ� �������� ���
                    if (distanceEachOther < setDistance)
                    {
                        //���� ������ ������ 
                        Vector3 dir = (a.transform.position - b.transform.position).normalized;
                        //�־��������� ���� �ο�
                        Rigidbody rb = a.GetComponent<Rigidbody>();
                        rb.AddForce(dir * forceMultiplier * Time.deltaTime, ForceMode.Force);
                    }
                }
            }
        }
    }
}
