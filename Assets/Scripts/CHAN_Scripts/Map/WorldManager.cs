using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WorldManager : MonoBehaviour 
{
    // �ϴø� ���� �������� �׼��� �����ϴ� ��ũ��Ʈ
    // * �����鿡�� �Ҵ���� ��ǥ���� �ش� �ʿ� ������ �� �ֵ��� ��������
    // * ������ �Ѱ輱�� ���ؾ� ��, �̸� �Ӽ����� �ο�
    // * �ð������� ���� ��

    //�Ӽ�: ������ �ʺ�, ����, �Ѱ輱 ��� �������� ����, ���� ����Ʈ , ��� �ؽ�Ʈ 
 
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
            Debug.Log("���� ���");
            //������ ����ٰ� �ȳ�
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
        //Debug.Log("�÷��̾�� �� ���� �Ÿ�: "+distance);
        GetComponent<User_Move>().turn = true;
        transform.position -= dir * 10 * Time.deltaTime;
    }



    
}
