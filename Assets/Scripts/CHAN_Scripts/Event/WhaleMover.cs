using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhaleMover : MonoBehaviour
{
    //���� ��� ����Ǹ鼭 ����
    // ������ ���� (��������� ������ ���� ��ġ)
    // ������ �����Ǹ� ���� �������� ȸ��(������ ���ӵ��� ������ ȸ��)
    // ���� ������ �ӵ��� �̵��Ѵ�. 
    // ���� ���Ÿ��� ���Ÿ��� ���������ȿ� �����ϸ� �׶����� �ٽ� Ž���� �����ϰ� ������ ��ȯ

    //�� �ӵ� �Ӽ�
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

        // ���� Ÿ�ټ��� �Ÿ��� ��� �����Ѵ�. 
        CheckDistance();
    }
    void DefineDirection()
    {
        int randomKey = Random.Range(0, targets.childCount-1);
        //Island_Information iInfo = Island_Information.instance;
        //int randomKey = Random.Range(1, iInfo.Island_Dic.Count);
        //targetIslandPos = iInfo.Island_Dic[randomKey.ToString()].island_Pos * iInfo.dis_multiplier;
        //�ӽ� ���� ����
        //Vector3 _dir = (transform.position - targetIslandPos).normalized;
        //Vector3 _dir = (transform.position - targets.GetChild(randomKey).position).normalized;
        targetIslandPos = targets.GetChild(randomKey).position;
        rotGoal = Quaternion.LookRotation(targetIslandPos);

        
    }
    void CheckDistance()
    {
        float dis = Vector3.Distance(transform.position, targetIslandPos);
        Debug.Log(dis);
        // ���� ��ǥ�Ÿ� ���Ϸ� ������ �� ������ �ٲ۴�.
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
