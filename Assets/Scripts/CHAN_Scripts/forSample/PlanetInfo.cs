using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetInfo :MonoBehaviour
{
    public int PlanetCount=400;
    // ���� �ݰ��� ����
    public float radius=250;
    [HideInInspector]public float[] eachSpeed;
    public float rotateSpeed=2.5f;
    public float minScale=0.5f;
    public float maxScale=10;

    public List<GameObject> planets = new List<GameObject>();

    // �ݰ氪 ���� �Լ���
    public void ControlRadius()
    {
        // �����̴� �ʱ� ��ġ�� �߰�
        // ��ũ�ѹٰ� �������� ���� �ݰ� ���� 
        // ��ũ�ѹٰ� ���������� ���� �ݰ� ���� 
    }
    // �༺ ���� ���� �Լ���
    public void ControlNOP()
    {

    }
    //�༺ ũ�� ���� �Լ���
    public void ControlScale()
    {

    }
    //�༺ �����ӵ� ���� �Լ���
    public void ControlSpeed()
    {

    }
}
