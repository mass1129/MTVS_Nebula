using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User_Camera : MonoBehaviour
{
    public float followSpeed;
    public Transform camHolderPos;
    bool turn;
    float wheelMultiplier;
    public float multi;
    Transform pos;
    // �÷��̾ �̵��ϸ� ī�޶� �ڵ����� �÷��̾�� ���󰡵��� ����
    // �ʱ⿡ ī�޶� ��ġ�� �������ش�. camHolder
    // camHolder�� �÷��̾��� �ڽ��̴�. 
    // ���� camHolder�� ��ġ�� �ٲ�ٸ�
    // ī�޶�� lerp �� �̿��ؼ� �Ѿư���.
    private void Start()
    {
        pos = camHolderPos;
        TranslateCamPos();
    }
    void Update()
    {
        // ī�޶�� ī�޶� Ȧ�� ���� �Ÿ� ���
        float distance = Vector3.Distance(transform.position, TranslateCamPos());
        // �� ���� �Ÿ��� �־�����  ī�޶� ��������� 
        if (distance >= 5)
        {
            turn = true;
        }
        // �� ���� �Ÿ��� �����Ÿ� ��������� ī�޶� ������� ����
        if (distance <= 0.1f)
        {
            turn = false;
        }
        if (turn&&!User_Move.instance.islandSelected)
        { 
            transform.position = Vector3.Lerp(transform.position, TranslateCamPos(), Time.deltaTime * followSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, camHolderPos.rotation, Time.deltaTime * followSpeed);
        }
        if(Input.GetKey(KeyCode.LeftControl))
        {
            wheelMultiplier += Input.GetAxisRaw("Mouse ScrollWheel");
            print(wheelMultiplier);
            camHolderPos.position += transform.forward * wheelMultiplier * multi;
            
        }
        
    }
    Vector3 TranslateCamPos()
    {
        Transform camPos = pos;
        Matrix4x4 trans = camPos.localToWorldMatrix;
        camPos.position = trans.GetPosition();
        return camPos.position;
    }
}
